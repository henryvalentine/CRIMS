using Neurotec.Licensing;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TandAProject.Utils;

namespace TandAProject
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            const string Address = "/local";
            const string Port = "5000";
            const string Components = "Biometrics.FingerExtraction,Biometrics.FingerMatching,Devices.FingerScanners,Images.WSQ";
            try
            {
               NLicense.ObtainComponents(Address, Port, Components);

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace.ToString());                              
                Logger.logToFile(ex, AppSettings.ErrorLogPath);
            }
            finally
            {
               NLicense.ReleaseComponents(Components);
            }
        }
    }
}
