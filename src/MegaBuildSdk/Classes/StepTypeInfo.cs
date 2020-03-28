namespace MegaBuild
{
	#region Using Directives

	using System;
	using System.Drawing;
	using System.IO;
	using System.Resources;
	using System.Windows.Forms;

	#endregion

	public sealed class StepTypeInfo
	{
		#region Constructors

		public StepTypeInfo(Type stepType)
		{
			this.StepType = stepType;

			StepDisplayAttribute[] display = (StepDisplayAttribute[])stepType.GetCustomAttributes(typeof(StepDisplayAttribute), true);
			if (display.Length > 0)
			{
				this.Name = display[0].Name;
				this.Description = display[0].Description;
				this.ImageIndex = this.LoadIconResource(display[0].IconResourceName);
			}
			else
			{
				this.Name = stepType.Name;
				this.Description = stepType.FullName;
				this.ImageIndex = -1;
			}
		}

		#endregion

		#region Public Properties

		public string Description { get; }

		public int ImageIndex { get; }

		public string Name { get; }

		public Type StepType { get; }

		#endregion

		#region Private Methods

		private int LoadIconResource(string iconResourceName)
		{
			int result = -1;

			if (iconResourceName.Length > 0)
			{
				// Load the icon resource by name, put it in Manager.StepImages, and return the image's index.
				using (Icon icon = new Icon(this.StepType, iconResourceName))
				{
					ImageList.ImageCollection images = Manager.StepImages.Images;
					images.Add(icon);
					result = images.Count - 1;
				}
			}

			return result;
		}

		#endregion
	}
}