namespace MegaBuild
{
	#region Using Directives

	using System;
	using System.Collections.Generic;
	using System.Diagnostics.CodeAnalysis;
	using System.IO;
	using System.Runtime.InteropServices;
	using Menees;
	using Media = System.Media;

	#endregion

	[StepDisplay("Sound", "Plays a system sound, a .wav file, or a user-defined beep.", "Images.SoundStep.ico")]
	[SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Called by Reflection.")]
	internal sealed class SoundStep : ExecutableStep
	{
		#region Private Data Members

		private SoundStyle style = SoundStyle.SystemSound;
		private SystemSound systemSound = SystemSound.Default;
		private int duration = 250;
		private int frequency = 1000;
		private string wavFile = string.Empty;

		#endregion

		#region Constructors

		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Called by Reflection.")]
		public SoundStep(Project project, StepCategory category, StepTypeInfo info)
			: base(project, category, info, ExecSupports.None)
		{
		}

		#endregion

		#region Public Properties

		public int Duration
		{
			get
			{
				return this.duration;
			}

			set
			{
				this.SetValue(ref this.duration, value);
			}
		}

		public int Frequency
		{
			get
			{
				return this.frequency;
			}

			set
			{
				this.SetValue(ref this.frequency, value);
			}
		}

		public override string StepInformation
		{
			get
			{
				string result;

				switch (this.Style)
				{
					case SoundStyle.SystemSound:
						result = string.Format("{0} sound", this.SystemSound);
						break;

					case SoundStyle.WavFile:
						result = Path.GetFileName(this.WavFile);
						break;

					case SoundStyle.Beep:
						result = string.Format("{0} Hz, {1} ms", this.Frequency, this.Duration);
						break;

					default:
						result = base.StepInformation;
						break;
				}

				return result;
			}
		}

		public SoundStyle Style
		{
			get
			{
				return this.style;
			}

			set
			{
				this.SetValue(ref this.style, value);
			}
		}

		public SystemSound SystemSound
		{
			get
			{
				return this.systemSound;
			}

			set
			{
				this.SetValue(ref this.systemSound, value);
			}
		}

		public string WavFile
		{
			get
			{
				return this.wavFile;
			}

			set
			{
				this.SetValue(ref this.wavFile, value);
			}
		}

		#endregion

		#region Public Methods

		public override bool Execute(StepExecuteArgs args)
		{
			this.Project.OutputLine("Playing " + this.StepInformation);

			bool result = false;
			switch (this.Style)
			{
				case SoundStyle.SystemSound:
					result = PlaySound(this.SystemSound);
					break;

				case SoundStyle.WavFile:
					result = PlaySound(this.WavFile);
					break;

				case SoundStyle.Beep:
					result = PlaySound(this.Frequency, this.Duration);
					break;
			}

			return result;
		}

		public override void GetStepEditorControls(ICollection<StepEditorControl> controls)
		{
			base.GetStepEditorControls(controls);
			controls.Add(new SoundStepCtrl() { Step = this });
		}

		#endregion

		#region Protected Methods

		protected internal override void Load(XmlKey key)
		{
			base.Load(key);
			this.Style = key.GetValue("Style", this.Style);
			this.SystemSound = key.GetValue("SystemSound", this.SystemSound);
			this.WavFile = key.GetValue("WavFile", this.WavFile);
			this.Frequency = key.GetValue("Frequency", this.Frequency);
			this.Duration = key.GetValue("Duration", this.Duration);
		}

		protected internal override void Save(XmlKey key)
		{
			base.Save(key);
			key.SetValue("Style", this.Style);
			key.SetValue("SystemSound", this.SystemSound);
			key.SetValue("WavFile", this.WavFile);
			key.SetValue("Frequency", this.Frequency);
			key.SetValue("Duration", this.Duration);
		}

		#endregion

		#region Private Methods

		private static bool PlaySound(SystemSound sound)
		{
			bool result = true;

			switch (sound)
			{
				case SystemSound.Default:
				case SystemSound.Simple:
					Media.SystemSounds.Beep.Play();
					break;

				case SystemSound.Information:
					Media.SystemSounds.Asterisk.Play();
					break;

				case SystemSound.Error:
					Media.SystemSounds.Hand.Play();
					break;

				case SystemSound.Question:
					Media.SystemSounds.Question.Play();
					break;

				case SystemSound.Warning:
					Media.SystemSounds.Exclamation.Play();
					break;

				default:
					result = false;
					break;
			}

			return result;
		}

		private static bool PlaySound(string wavFileName)
		{
			using (var player = new Media.SoundPlayer(wavFileName))
			{
				bool result = false;
				try
				{
					player.PlaySync();
					result = true;
				}
				catch (TimeoutException)
				{
				}
				catch (InvalidOperationException)
				{
				}
				catch (FileNotFoundException)
				{
				}

				return result;
			}
		}

		private static bool PlaySound(int frequency, int duration)
		{
			bool result = false;

			try
			{
				Console.Beep(frequency, duration);
				result = true;
			}
			catch (ArgumentOutOfRangeException)
			{
			}

			return result;
		}

		#endregion
	}
}