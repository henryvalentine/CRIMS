using Neurotec.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TandAProject.Services
{
    public delegate void ApplicationStateChangeNotifyer(ApplicationController.State state);
    public delegate void ApplicationMessageNotifyer(string msg);
    public delegate void ActiveTemplateNotifyer(NBuffer template);
    public delegate void ActiveIDNumberNotifyer(string IDNumber);

}
