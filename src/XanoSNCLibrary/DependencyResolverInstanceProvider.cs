using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;

namespace XanoSNCLibrary
{
    class DependencyResolverInstanceProvider : IInstanceProvider
    {
        public DependencyResolverInstanceProvider(Type serviceType)
        {
        }

        #region IInstanceProvider Members
        public object GetInstance(InstanceContext instanceContext)
        {
            return GetInstance(instanceContext, null);
        }

        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            IXanoSNCRepository repository = new XanoSNCRepository();
            return new XanoSNCService(repository);
        }

        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
            if (instance is IDisposable)
                ((IDisposable)instance).Dispose();
        }
        #endregion
    }
}
