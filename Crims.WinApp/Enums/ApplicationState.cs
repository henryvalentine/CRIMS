using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Crims.UI.Win.Enroll.Enums
{
    public enum ApplicationState
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
    }

}
