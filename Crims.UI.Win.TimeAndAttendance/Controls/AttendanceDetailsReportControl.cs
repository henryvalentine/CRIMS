using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TandAProject.BusinessObjects;
using TandAProject.Services;
using Microsoft.Reporting.WinForms;

namespace TandAProject.Controls
{
    public partial class AttendanceDetailsReportControl : UserControl
    {
        public AttendanceDetailsReportControl()
        {
            InitializeComponent();
        }

        private void AttendanceReportControl_Load(object sender, EventArgs e)
        {

        }

        private void viewer1_Load(object sender, EventArgs e)
        {
            
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            string startDate = dateTimePickerStart.Value.ToString("yyyy-MM-dd");
            string endDate = dateTimePickerEnd.Value.ToString("yyyy-MM-dd");
            string resumptionTime = new AppSettings().ResumptionTime;
            string resumptionGrace = new AppSettings().ResumptionGraceTime;

            this.AttendanceDetailsObjectBindingSource.DataSource = DataServices.GetAttendanceDetailsReport(startDate,endDate);

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

        private void AttendanceDetailsObjectBindingSource_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
