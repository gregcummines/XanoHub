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
using XanoHubLibrary;

namespace XanoHubWindowsService
{
    public partial class XanoHubWindowsService : ServiceBase
    {
        private ServiceHost wcfXanoHubService; 

        public XanoHubWindowsService()
        {
            InitializeComponent();
        }

        public void DebugThisService()
        {
            OnStart(null);
        }

        protected override void OnStart(string[] args)
        {
            wcfXanoHubService = new ServiceHost(typeof(XanoHubService));
            wcfXanoHubService.Open();
        }

        protected override void OnStop()
        {
            if (wcfXanoHubService != null && wcfXanoHubService.State == CommunicationState.Opened)
            {
                wcfXanoHubService.Close();
            }
        }
    }
}
