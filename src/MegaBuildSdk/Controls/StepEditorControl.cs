namespace MegaBuild;

#region Using Directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Menees.Windows.Forms;

#endregion

public partial class StepEditorControl : ExtendedUserControl
{
	#region Constructors

	public StepEditorControl()
	{
		this.InitializeComponent();
	}

	#endregion

	#region Public Properties

	public virtual string DisplayName
	{
		get
		{
			// We can't make this property abstract because we're a base UserControl, and the Forms Designer doesn't support abstract bases.
			throw new NotSupportedException();
		}
	}

	#endregion

	#region Public Methods

	public virtual void OnCancel()
	{
	}

	public virtual bool OnOk()
	{
		// We can't make this method abstract because we're a base UserControl, and the Forms Designer doesn't support abstract bases.
		throw new NotSupportedException();
	}

	public virtual void OnReadyToDisplay()
	{
	}

	#endregion
}
