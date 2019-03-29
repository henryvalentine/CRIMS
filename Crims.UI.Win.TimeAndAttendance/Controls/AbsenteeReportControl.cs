using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using TandAProject.Services;

namespace TandAProject.Controls
{
    public partial class AbsenteeReportControl : UserControl
    {
        public AbsenteeReportControl()
        {
            InitializeComponent();
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            string startDate = dateTimePickerStart.Value.ToString("yyyy-MM-dd");

            this.AttendanceDetailsObjectBindingSource.DataSource = DataServices.GeAbsenteeReport(startDate, startDate);

            ReportParameter FromDateParam = new ReportParameter("StartDate", startDate);

            List<ReportParameter> param = new List<ReportParameter>();
            param.Add(FromDateParam);
            reportViewer1.LocalReport.SetParameters(param);

            this.reportViewer1.RefreshReport();

        }
    }
}
