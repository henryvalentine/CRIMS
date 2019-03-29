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
    public partial class LatenessReportControl : UserControl
    {
        public LatenessReportControl()
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
            string resumptionTime = new AppSettings().ResumptionTime;
            string resumptionGrace = new AppSettings().ResumptionGraceTime;
            this.AttendanceDetailsObjectBindingSource.DataSource = DataServices.GetLatenessReport(resumptionGrace, startDate, endDate);

            ReportParameter FromDateParam = new ReportParameter("StartDate", startDate);
            ReportParameter ToDateParam = new ReportParameter("EndDate", endDate);
            ReportParameter resumptionTimeParam = new ReportParameter("ResumptionTime", resumptionTime);
            ReportParameter resumptionGraceParam = new ReportParameter("ResumptionGrace", resumptionGrace);

            List<ReportParameter> param = new List<ReportParameter>();
            param.Add(FromDateParam);
            param.Add(ToDateParam);
            param.Add(resumptionTimeParam);
            param.Add(resumptionGraceParam);
            reportViewer1.LocalReport.SetParameters(param);

            this.reportViewer1.RefreshReport();
        }

        private void LatenessReportControl_Load(object sender, EventArgs e)
        {
            //RefreshReport();
        }
    }
}
