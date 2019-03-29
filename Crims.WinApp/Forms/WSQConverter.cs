using Crims.UI.Win.Enroll.DataServices;
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
    public partial class WSQConverter : Form
    {
        public WSQConverter()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string query = richTextBox1.Text;
            string connectionString = richTextBox2.Text;
            string saveFileDir = richTextBox3.Text;
            var customData = await Task.Run(() =>
            {
                try
                {
                    return DatabaseOpperations.QueryAndConvertToWQS(query,connectionString,saveFileDir );

                }
                catch (Exception)
                {
                    throw;
                }
            });
            MessageBox.Show("Completed");
        }

        private void WSQ_Converter_Load(object sender, EventArgs e)
        {

        }
    }
}
