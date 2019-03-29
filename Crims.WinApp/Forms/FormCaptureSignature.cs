using Crims.UI.Win.Enroll.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Crims.UI.Win.Enroll.Forms
{
    public partial class FormCaptureSignature : Form
    {
        private CaptureSignatureNotifyer _CaptureSignatureNotifyer;

        public FormCaptureSignature()
        {
            InitializeComponent();
        }

        public FormCaptureSignature(CaptureSignatureNotifyer _CaptureSignatureNotifyer)
        {
            this._CaptureSignatureNotifyer = _CaptureSignatureNotifyer;
            InitializeComponent();

        }

        private void cmdSign_Click(object sender, EventArgs e)
        {
            sigPlusNET1.SetTabletState(1);
        }

        private void cmdStop_Click(object sender, EventArgs e)
        {
            sigPlusNET1.SetTabletState(0);
        }

        private void cmdClear_Click(object sender, EventArgs e)
        {
            sigPlusNET1.ClearTablet();
        }

        private void cmdSaveImage_Click(object sender, EventArgs e)
        {
            sigPlusNET1.SetImageXSize(500);
            sigPlusNET1.SetImageYSize(150);
            sigPlusNET1.SetJustifyMode(5);

            Image myimage = sigPlusNET1.GetSigImage();
            _CaptureSignatureNotifyer(myimage, true);
            this.Dispose();
        }

        private void cmdClose_Click(object sender, EventArgs e)
        {
            _CaptureSignatureNotifyer(null);
            this.Dispose();
        }

        private void FormCaptureSignature_Shown(object sender, EventArgs e)
        {
            sigPlusNET1.SetTabletState(1);
        }
    }
}
