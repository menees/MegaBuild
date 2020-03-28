namespace MegaBuild
{
	#region Using Directives

	using System;
	using System.Drawing;
	using System.Windows.Forms;

	#endregion

	[ToolboxBitmap(typeof(Button))]
	public sealed class NonSelectButton : Button
	{
		#region Constructors

		public NonSelectButton()
		{
			this.SetStyle(ControlStyles.Selectable, false);
		}

		#endregion
	}
}