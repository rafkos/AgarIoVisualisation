using System;
using System.Reflection;
using JetBrains.Annotations;

namespace Assets.Scripts.Services
{
    internal class WeakActionReference
    {
        private readonly MethodInfo _methodToCall;
        private readonly WeakReference _weakReference;

        public WeakActionReference([NotNull] object target, [NotNull] MethodInfo methodToCall)
        {
            _methodToCall = methodToCall;
            if (target == null)
            {
                throw new ArgumentNullException("action");
            }
            if (methodToCall == null)
            {
                throw new ArgumentNullException("methodToCall");
            }

            _weakReference = new WeakReference(target);
        }

        public bool IsAlive
        {
            get
            {
                return _weakReference.IsAlive;
            }
        }

        public void Invoke(IEvent publishEvent)
        {
            if (!IsAlive)
            {
                return;
            }

            _methodToCall.Invoke(_weakReference.Target, new object[] { publishEvent });
        }
    }
}