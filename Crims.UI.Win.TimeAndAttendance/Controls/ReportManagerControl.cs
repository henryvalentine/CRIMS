using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TandAProject.Services;

namespace TandAProject.Controls
{
    public partial class ReportManagerControl: UserControl
    {
        AppSettings AppSettings = new AppSettings();

        //Initialise the Notification Deligate
        public ApplicationStateChangeNotifyer StateNotifyer;
        public ApplicationMessageNotifyer MessageNotifyer;

        public ReportManagerControl()
        {
            InitializeComponent();
        }

        private void AddControl(UserControl control)
        {
            centerPanel1.Controls.Add(control);
            control.Dock = DockStyle.Fill;
            control.Visible = true;
        }

        private void RemoveControls()
        {
            foreach (Control c in centerPanel1.Controls)
            {
                centerPanel1.Controls.Remove(c);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StateNotifyer(ApplicationController.State.Idle);
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StateNotifyer(ApplicationController.State.Setup);
        }

        private void attendanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveControls();
            AttendanceDetailsReportControl control = new AttendanceDetailsReportControl();
            AddControl(control);
        }

        private void attendanceSummaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveControls();
            AttendanceSummaryReportControl control = new AttendanceSummaryReportControl();
            AddControl(control);
        }

        private void movementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveControls();
            MovementReportControl control = new MovementReportControl();
            AddControl(control);
        }

        private void latenessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveControls();
            LatenessReportControl control = new LatenessReportControl();
            AddControl(control);
        }

        private void absenteeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveControls();
            AbsenteeReportControl control = new AbsenteeReportControl();
            AddControl(control);
        }

        private void absconderReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveControls();
            AbsconderReportControl control = new AbsconderReportControl();
            AddControl(control);
        }
    }
}
