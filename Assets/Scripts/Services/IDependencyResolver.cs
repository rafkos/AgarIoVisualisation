using System;
using System.Collections.Generic;

namespace Assets.Scripts.Services
{
    public interface IDependencyResolver
    {
        T GetService<T>();

        IEnumerable<object> GetServices(Type serviceTypes);
    }
}