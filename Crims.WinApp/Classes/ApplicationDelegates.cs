using Crims.UI.Win.Enroll.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using Neurotec.Biometrics;
using Neurotec.IO;

namespace Crims.UI.Win.Enroll.Classes
{

    public delegate void ApplicationStateChangeNotifyer(ApplicationState state);
    public delegate void ApplicationMessageNotifyer(string msg);
    public delegate void CapturePhotoNotifyer(Image image, byte[] imageTemplate, bool OKCancel = false);
    public delegate void CaptureSignatureNotifyer(Image image, bool OKCancel = false);
    public delegate bool CaptureFingerNotifyer(NFinger finger, NSubject fingerSubject, FingerDescription fingerDesc, bool OKCancel = false);
    public delegate void LoginNotifyer(UserAccountModel UserProfile, bool OKCancel = false);
    public delegate void BusyNotifyer(string message, AppNofityerState notifyerState);
    
    public enum AppNofityerState
    {
        busy = 1,
        done = 2
    }
}
