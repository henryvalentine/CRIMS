using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Crims.UI.Win.Enroll.Forms;
using Microsoft.VisualBasic.ApplicationServices;

namespace Crims.UI.Win.Enroll
{
    static class Program
    {
        static FormMain _mainForm;
        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            _mainForm = new FormMain();
            ProcessInit.Run(_mainForm, NewInstanceHandler);
        }
        
        public static void NewInstanceHandler(object sender, StartupNextInstanceEventArgs e)
        {
            //var args = e.CommandLine.ToArray();
        }
        
        public class ProcessInit : WindowsFormsApplicationBase
        {
            private ProcessInit()
            {
                base.IsSingleInstance = true;
            }

            public static void Run(Form f, StartupNextInstanceEventHandler startupHandler)
            {
                var app = new ProcessInit { MainForm = f };
                app.StartupNextInstance += startupHandler;
                app.Run(Environment.GetCommandLineArgs());
            }
        }
    }
    
}
