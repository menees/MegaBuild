namespace MegaBuild;

#region Using Directives

using System;
using System.Drawing;

#endregion

public sealed class OutputAddedEventArgs : EventArgs
{
	#region Constructors

	public OutputAddedEventArgs(string message, int indent, Color color)
		: this(message, indent, color, false, Guid.Empty)
	{
	}

	public OutputAddedEventArgs(string message, int indent, Color color, bool highlight, Guid outputId)
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

	#region Public Methods

	public bool IsFormatEqual(OutputAddedEventArgs? args)
	{
		bool result = args != null
			&& this.Color == args.Color
			&& this.Highlight == args.Highlight
			&& this.Indent == args.Indent
			&& this.OutputId == args.OutputId;
		return result;
	}

	#endregion
}