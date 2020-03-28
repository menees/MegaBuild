namespace MegaBuild
{
	#region Using Directives

	using System;
	using System.Drawing;

	#endregion

	public sealed class OutputAddedEventArgs : EventArgs
	{
		#region Constructors

		internal OutputAddedEventArgs(string message, int indent, Color color, bool highlight, Guid outputId)
		{
			this.Message = message;
			this.Indent = indent;
			this.Color = color;
			this.Highlight = highlight;
			this.OutputId = outputId;
		}

		#endregion

		#region Public Properties

		public Color Color { get; }

		public bool Highlight { get; }

		public int Indent { get; }

		public string Message { get; }

		public Guid OutputId { get; }

		#endregion
	}
}