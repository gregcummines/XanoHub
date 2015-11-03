using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using XanoSNCLibrary;

namespace XanoSNCWindowsService
{
    public partial class XanoSNCWindowsService : ServiceBase
    {
        private CustomServiceHost wcfXanoSNCService; 

        public XanoSNCWindowsService()
        {
            InitializeComponent();
        }

        public void DebugThisService()
        {
            OnStart(null);
        }

        protected override void OnStart(string[] args)
        {
            wcfXanoSNCService = new CustomServiceHost(typeof(XanoSNCService));
            wcfXanoSNCService.Open();
        }

        protected override void OnStop()
        {
            if (wcfXanoSNCService != null && wcfXanoSNCService.State == CommunicationState.Opened)
            {
                wcfXanoSNCService.Close();
            }
        }
    }
}
