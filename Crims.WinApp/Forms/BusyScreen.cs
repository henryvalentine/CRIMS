using Crims.UI.Win.Enroll.Classes;
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
    public partial class BusyScreen : Form
    {
        private BusyNotifyer _BusyNotifyer;
        public BusyScreen(BusyNotifyer BusyNotifyer)
        {
            _BusyNotifyer = BusyNotifyer;
            InitializeComponent();
        }
    }
}
