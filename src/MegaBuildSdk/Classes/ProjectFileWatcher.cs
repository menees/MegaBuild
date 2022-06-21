namespace MegaBuild
{
	#region Using Directives

	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Threading;
	using System.Threading.Tasks;
	using System.Windows.Forms;
	using Menees;

	#endregion

	internal class ProjectFileWatcher : IDisposable
	{
		#region Private Data Members

		private static readonly TimeSpan InternalUpdateDelay = TimeSpan.FromSeconds(2);
		private static readonly TimeSpan ExternalUpdateDelay = TimeSpan.FromSeconds(0.1);

		private readonly Project project;
		private readonly FileSystemWatcher fileWatcher = new();
		private readonly object monitor = new();

		private DateTime ignoreUntil;
		private int updateCount;
		private bool isPromptShowing;

		#endregion

		#region Constructors

		public ProjectFileWatcher(Project project)
		{
			this.project = project;
			this.project.FileNameSet += this.Project_FileNameSet;
			this.fileWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.CreationTime;
			this.fileWatcher.Changed += this.FileWatcher_Changed;
			this.fileWatcher.Error += this.FileWatcher_Error;
		}

		#endregion

		#region Public Properties

		public bool IsEnabled
		{
			get => this.fileWatcher.EnableRaisingEvents;
			set
			{
				// Increment even if IsEnabled isn't changing since the caller must be doing some type of update.
				Interlocked.Increment(ref this.updateCount);
				this.fileWatcher.EnableRaisingEvents = value;
			}
		}

		#endregion

		#region Public Methods

		public IDisposable BeginUpdate()
		{
			this.IsEnabled = false;
			return new Disposer(() =>
			{
				lock (this.monitor)
				{
					this.ignoreUntil = DateTime.UtcNow + InternalUpdateDelay;
				}

				this.IsEnabled = true;
			});
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
			this.fileWatcher.Dispose();
			this.project.FileNameSet -= this.Project_FileNameSet;
		}

		#endregion

		#region Private Methods

		private void Project_FileNameSet(object? sender, EventArgs e)
		{
			string fileName = this.project.FileName;

			// The FileSystemWatcher.Path property defaults to an empty string. However, it can't be (re)set to empty explicitly,
			// and it can't be set to a directory that doesn't exist. Either of those throw an ArgumentException.
			// https://github.com/dotnet/runtime/blob/715c71ae6af5775402e54d3b8ab6c4a888da3cb5/src/libraries/System.IO.FileSystem.Watcher/src/System/IO/FileSystemWatcher.cs#L259
			string path = (fileName.IsNotWhiteSpace() ? Path.GetDirectoryName(fileName) : null) ?? string.Empty;
			if (path.IsNotWhiteSpace() && Directory.Exists(path))
			{
				this.fileWatcher.Path = path;
				this.fileWatcher.Filter = Path.GetFileName(fileName);
				this.IsEnabled = true;
				Debug.WriteLine($"*** Watching {this.fileWatcher.Path} with filter {this.fileWatcher.Filter} ***");
			}
			else
			{
				this.IsEnabled = false;
			}
		}

		private void FileWatcher_Changed(object sender, FileSystemEventArgs e)
		{
			// We can receive this notification multiple times during a file update (e.g.,
			// on first write, mid-write, and at file close). It comes in on a worker thread,
			// so we'll use threading timer callbacks to wait until the change notifications
			if (e.ChangeType == WatcherChangeTypes.Changed)
			{
				lock (this.monitor)
				{
					if (DateTime.UtcNow >= this.ignoreUntil)
					{
						int updateCount = Interlocked.Increment(ref this.updateCount);

						// Create a new timer that only fires once, and let the callback dispose it.
						CallbackState state = new() { UpdateCount = updateCount };
						System.Threading.Timer timer = new(this.ChangedTimer_Callback, state, Timeout.Infinite, Timeout.Infinite);
						state.Timer = timer;
						timer.Change(ExternalUpdateDelay, Timeout.InfiniteTimeSpan);
					}
				}
			}
		}

		private void FileWatcher_Error(object sender, ErrorEventArgs e)
		{
			Exception ex = e.GetException();
			string message = ApplicationInfo.IsDebugBuild ? ex.ToString() : ex.Message;
			this.project.OutputLine(message, OutputColors.Error);
		}

		private void ChangedTimer_Callback(object? state)
		{
			bool notifyProject = false;
			if (state is CallbackState callbackState)
			{
				callbackState.Timer?.Dispose();
				notifyProject = this.updateCount == callbackState.UpdateCount;
			}

			if (notifyProject)
			{
				this.project.Form?.BeginInvoke(() =>
				{
					if (!this.isPromptShowing)
					{
						try
						{
							if (this.project.Building)
							{
								this.isPromptShowing = true;
								MessageBox.Show(
									this.project.Form,
									$"The \"{this.project.Title}\" project has changed on disk. You'll need to manually reload it after the build finishes.",
									"Project Changed On Disk",
									MessageBoxButtons.OK,
									MessageBoxIcon.Warning);
							}
							else
							{
								string prompt = $"The \"{this.project.Title}\" project has changed on disk and needs to be reloaded. "
									+ "However, you have unsaved changes in memory too.\n\n"
									+ $"Do you want to save the in-memory changes to a new file (using Save As) "
									+ $"so the changes to \"{this.project.Title}\" on disk are preserved?";
								this.isPromptShowing = true;
								if (this.project.CanClose(prompt, saveAs: true) == DialogResult.Yes)
								{
									this.isPromptShowing = false;

									// If the user clicked "No, don't save changes" , then CanClose
									// will return DialogResult.Yes even though it didn't save.
									// So we need to call Close to make sure the Modified flag is
									// cleared, so Open won't show another CanClose prompt.
									string fileName = this.project.FileName;
									this.project.Close();
									this.project.Open(fileName);
								}
							}
						}
						finally
						{
							this.isPromptShowing = false;
						}
					}
				});
			}
		}

		#endregion

		#region Private Types

		// This has to be a reference type (and not a ValueTuple), so we can pass a reference
		// into Timer's constructor and then make this point to the Timer we constructed.
		private sealed class CallbackState
		{
			public System.Threading.Timer? Timer { get; set; }

			public int UpdateCount { get; set; }
		}

		#endregion
	}
}
