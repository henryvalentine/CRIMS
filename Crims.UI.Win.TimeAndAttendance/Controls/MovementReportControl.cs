﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TandAProject.Services;
using Microsoft.Reporting.WinForms;

namespace TandAProject.Controls
{
    public partial class MovementReportControl : UserControl
    {
        public MovementReportControl()
        {
            InitializeComponent();
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            string startDate = dateTimePickerStart.Value.ToString("yyyy-MM-dd");
            string endDate = dateTimePickerEnd.Value.ToString("yyyy-MM-dd");
            this.AttendanceDetailsObjectBindingSource.DataSource = DataServices.GeMovementReport(startDate, endDate);

            ReportParameter FromDateParam = new ReportParameter("StartDate", startDate);
            ReportParameter ToDateParam = new ReportParameter("EndDate", endDate);

            List<ReportParameter> param = new List<ReportParameter>();
            param.Add(FromDateParam);
            param.Add(ToDateParam);
            reportViewer1.LocalReport.SetParameters(param);

            this.reportViewer1.RefreshReport();

        }
    }
}
