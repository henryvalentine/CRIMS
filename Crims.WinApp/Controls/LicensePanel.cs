using System;
using System.Drawing;
using System.Windows.Forms;
using Neurotec.Licensing;
using System.Text;

namespace Crims.UI.Win.Enroll.Controls
{
	public partial class LicensePanel : UserControl
	{
		#region Public constructor

		public LicensePanel()
		{
			InitializeComponent();
		}

		#endregion

		#region Private fields

		private const int Port = 5000;
		private const string Address = "/local";
		private string _requiredComponents = string.Empty;
		private string _optionalComponents = string.Empty;

		private const int FullHeight = 60;
		private const int MinHeight = 30;

		#endregion

		#region Public properties

		public string RequiredComponents
		{
			get
			{
				return _requiredComponents;
			}
			set
			{
				_requiredComponents = value;
				rtbComponents.SelectionColor = Color.Black;
				rtbComponents.Text = GetRequiredComponentsString();
				string optional = GetOptionalComponentsString();
				if (!string.IsNullOrEmpty(optional))
				{
					rtbComponents.AppendText(", " + optional);
				}
			}
		}

		public string OptionalComponents
		{
			get
			{
				return _optionalComponents;
			}
			set
			{
				_optionalComponents = value;
				rtbComponents.SelectionColor = Color.Black;
				rtbComponents.Text = GetRequiredComponentsString();
				string optional = GetOptionalComponentsString();
				if (!string.IsNullOrEmpty(optional))
				{
					rtbComponents.AppendText(", " + optional);
				}
			}
		}

		#endregion

		#region Private methods

		private void LicensePanelLoad(object sender, EventArgs e)
		{
			if (!DesignMode)
			{
				RefreshComponentsStatus();
			}
		}

		private string GetRequiredComponentsString()
		{
			if (_requiredComponents != null)
				return _requiredComponents.Replace(",", ", ");
			else
				return string.Empty;
		}

		private string GetOptionalComponentsString()
		{
			if (_optionalComponents == null) return string.Empty;

			string[] components = _optionalComponents.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
			if (components.Length == 0)
				return string.Empty;
			else
			{
				StringBuilder result = new StringBuilder();
				for (int i = 0; i < components.Length; i++)
				{
					result.Append(components[i]);
					result.Append("(optional)");
					if (i != components.Length - 1)
						result.Append(", ");
				}
				return result.ToString();
			}
		}

		private void RefreshRequired()
		{
			string text = rtbComponents.Text;
			try
			{
				rtbComponents.Text = string.Empty;
				int obtainedCount = 0;
				string[] requiredComponents = RequiredComponents.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
				for (int i = 0; i < requiredComponents.Length; i++)
				{
					string item = requiredComponents[i];
					if (NLicense.IsComponentActivated(item))
					{
						rtbComponents.SelectionColor = Color.Green;
						rtbComponents.AppendText(item);
						obtainedCount++;
					}
					else
					{
						rtbComponents.SelectionColor = Color.Red;
						rtbComponents.AppendText(item);
					}
					if (i != requiredComponents.Length - 1)
					{
						rtbComponents.SelectionColor = Color.Black;
						rtbComponents.AppendText(", ");
					}
				}

				if (obtainedCount == requiredComponents.Length)
				{
					lblStatus.Text = "Component licenses obtained";
					lblStatus.ForeColor = Color.Green;
				}
				else
				{
					lblStatus.Text = "Not all required licenses obtained";
					lblStatus.ForeColor = Color.Red;
				}
			}
			catch
			{
				rtbComponents.SelectionColor = Color.Black;
				rtbComponents.Text = text;
				throw;
			}
		}

		private void RefreshOptional()
		{
			string text = rtbComponents.Text;
			try
			{
				rtbComponents.SelectionColor = Color.Black;
				rtbComponents.AppendText(", ");
				string[] components = OptionalComponents.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
				for (int i = 0; i < components.Length; i++)
				{
					string item = components[i];
					rtbComponents.SelectionColor = NLicense.IsComponentActivated(item) ? Color.Green : Color.Red;
					rtbComponents.AppendText(string.Format("{0}(optional)", item));

					if (i != components.Length - 1)
					{
						rtbComponents.SelectionColor = Color.Black;
						rtbComponents.AppendText(", ");
					}
				}
			}
			catch
			{
				rtbComponents.SelectionColor = Color.Black;
				rtbComponents.Text = text;
				throw;
			}
		}

		#endregion

		#region Public methods

		public void RefreshComponentsStatus()
		{
			try
			{
				RefreshRequired();
				RefreshOptional();
			}
			catch (Exception ex)
			{
				lblStatus.Text = string.Format("Failed to check components activation status. Error message: {0}", ex.Message);
				lblStatus.ForeColor = Color.Red;
			}
		}

        #endregion

        private void lblStatus_Click(object sender, EventArgs e)
        {

        }
    }
}
