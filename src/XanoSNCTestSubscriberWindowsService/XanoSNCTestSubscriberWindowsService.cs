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
using XanoSNCTestSubscriberLibrary;

namespace XanoSNCTestSubscriberWindowsService
{
    public partial class XanoSNCTestSubscriberWindowsService : ServiceBase
    {
        private ServiceHost wcfXanoSNCTestSubscriber;

        public XanoSNCTestSubscriberWindowsService()
        {
            InitializeComponent();
        }

        public void DebugThisService()
        {
            OnStart(null);
        }

        protected override void OnStart(string[] args)
        {
            wcfXanoSNCTestSubscriber = new ServiceHost(typeof(XanoSNCTestSubscriber));
            wcfXanoSNCTestSubscriber.Open();
        }

        protected override void OnStop()
        {
            if (wcfXanoSNCTestSubscriber != null && wcfXanoSNCTestSubscriber.State == CommunicationState.Opened)
            {
                wcfXanoSNCTestSubscriber.Close();
            }
        }
    }
}
