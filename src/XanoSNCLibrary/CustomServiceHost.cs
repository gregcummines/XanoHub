using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace XanoSNCLibrary
{
    public class CustomServiceHost : ServiceHost
    {
        public CustomServiceHost()
            : base()
        {

        }
        public CustomServiceHost(Type serviceType, params Uri[] baseAddresses)
            : base(serviceType, baseAddresses)
        {
        }

        protected override void OnOpening()
        {
            //this.Description.Behaviors.Clear();
            this.Description.Behaviors.Add(new DependencyResolverServiceBehavior());
            base.OnOpening();
        }
    }
}
