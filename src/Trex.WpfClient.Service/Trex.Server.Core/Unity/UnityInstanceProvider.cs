﻿using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using Microsoft.Practices.Unity;

namespace VMDe.Core.Unity
{
    public class UnityInstanceProvider : IInstanceProvider
    {
        public IUnityContainer Container { set; get; }
        public Type ServiceType { set; get; }

        public UnityInstanceProvider()
            : this(null)
        {
        }

        public UnityInstanceProvider(Type type)
        {
            ServiceType = type;
        }

        #region IInstanceProvider Members

        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            return Container.Resolve(ServiceType);
        }

        public object GetInstance(InstanceContext instanceContext)
        {
            return GetInstance(instanceContext, null);
        }
        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {

        }
        #endregion
    }
}