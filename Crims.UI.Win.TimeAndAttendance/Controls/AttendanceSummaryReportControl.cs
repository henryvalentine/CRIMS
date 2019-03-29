using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TandAProject.Services;
using Microsoft.Reporting.WinForms;
using TandAProject.BusinessObjects;

namespace TandAProject.Controls
{
    public partial class AttendanceSummaryReportControl : UserControl
    {
        public AttendanceSummaryReportControl()
        {
            InitializeComponent();
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            RefreshReport();
        }

        private void RefreshReport()
        {
            string startDate = dateTimePickerStart.Value.ToString("yyyy-MM-dd");
            string endDate = dateTimePickerEnd.Value.ToString("yyyy-MM-dd");
            string expextedWorkHours = string.Empty;

            this.AttendanceDetailsObjectBindingSource.DataSource = DataServices.GetTimeSpentSummary(startDate,endDate,out expextedWorkHours);

            ReportParameter FromDateParam = new ReportParameter("StartDate", startDate);
            ReportParameter ToDateParam = new ReportParameter("EndDate", endDate);
            ReportParameter ExpectedHours = new ReportParameter("ExpectedHours", expextedWorkHours);

            List<ReportParameter> param = new List<ReportParameter>();
            param.Add(FromDateParam);
            param.Add(ToDateParam);
            param.Add(ExpectedHours);

            reportViewer1.LocalReport.SetParameters(param);

            this.reportViewer1.RefreshReport();
        }

        private void AttendanceSummaryReportControl_Load(object sender, EventArgs e)
        {

        }
    }
}
