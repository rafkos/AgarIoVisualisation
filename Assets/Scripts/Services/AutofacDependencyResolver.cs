using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Autofac;

namespace Assets.Scripts.Services
{
    public class AutofacDependencyResolver : IDependencyResolver
    {
        private readonly IContainer _container;

        public AutofacDependencyResolver()
        {
            _container = new ContainerBuilder().Build();
        }

        public AutofacDependencyResolver(IContainer container)
        {
            _container = container;
        }

        public T GetService<T>()
        {
            return _container.IsRegistered<T>() ? _container.Resolve<T>() : default(T);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return ((IEnumerable)_container.Resolve(typeof (IEnumerable<>).MakeGenericType(serviceType))).Cast<object>();
        }
    }
}