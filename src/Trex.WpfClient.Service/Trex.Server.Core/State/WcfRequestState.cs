using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Trex.Server.Infrastructure.State
{
    public class WcfRequestState : IRequestState
    {
        [ThreadStatic]
        static IDictionary<string, object> _serviceStackState;
        public static IDictionary<string, object> ServiceStackState
        {
            get { return _serviceStackState; }
            set { _serviceStackState = value; }
        }

        private static IDictionary<string, object> State
        {
            get
            {
                if (OperationContext.Current != null)
                {
                    var extension = OperationContext.Current.Extensions.Find<StateExtension>();

                    if (extension == null)
                    {
                        extension = new StateExtension();
                        OperationContext.Current.Extensions.Add(extension);
                    }
                    return extension.State;
                }
                return ServiceStackState ?? (ServiceStackState = new Dictionary<string, object>());
            }
        }

        public T Get<T>(string key)
        {
            if (State.ContainsKey(key))
            {
                return (T)State[key];
            }

            return default(T);
        }

        public void Store(string key, object something)
        {
            State[key] = something;
        }
    }
}