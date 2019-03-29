using Neurotec.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TandAProject.Services
{
    public class ApplicationController
    {
        public enum State
        {
            Startup,
            Loading,
            LoadJobCompleted,
            Idle,
            Capturing,
            Captured_Good,
            Captured_Bad,
            Identifying,
            Identified,
            Identify_failed,
            Verifying,
            Verified,
            Verify_Failed,
            Dispose,
            No_User_Found,
            Attendance_EI,
            Login_Success,
            Login_Failed,
            Identified_Low_Match,
            Settings,
            Setup,
            Report,
            SyncTool,
            Login,
            Unknown,
            LoadJobFailed
        };

        public ApplicationController()
        {
        }
        private ApplicationController.State _applicationState;

        public ApplicationController.State ApplicationState
        {
            get { return _applicationState; }
            set { _applicationState = value; }
        }
    }
}
