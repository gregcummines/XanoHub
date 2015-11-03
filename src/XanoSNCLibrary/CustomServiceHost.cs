using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace XanoSNCLibrary
{
    /// <summary>
    /// Class used to override the construction process of the WCF service such
    /// that I can provide a dependency injection hook
    /// See article: http://blogs.msdn.com/b/carlosfigueira/archive/2011/05/31/wcf-extensibility-iinstanceprovider.aspx
    /// </summary>
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
