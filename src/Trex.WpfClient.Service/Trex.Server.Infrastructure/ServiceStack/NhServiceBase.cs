using System;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;

namespace Trex.Server.Infrastructure.ServiceStack
{
    /// <summary>
    /// 
    /// </summary>
    [Authenticate]
    public abstract class NhServiceBase : IService
    {
        protected object TryExecute(Func<object, object> action, object request)
        {
            try
            {
                return action.Invoke(request);
            }
            catch (Exception exception)
            {
                var requestDetails = ObjectDumper.DumpToString(request);
                throw new Exception(string.Format("\n{0} details:\n{1}", request.GetType().Name, requestDetails), exception);
            }
        }
    }

    /// <summary>
    /// Authenticated service using ServiceStack post
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Authenticate]
    public abstract class NhServiceBasePost<T> : NhServiceBase, IPost<T>
    {
        public object Post(T request)
        {
            return TryExecute(req => Send((T) req), request);
        }

        protected abstract object Send(T request);
    }

    /// <summary>
    /// Authenticated service using ServiceStack get
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Authenticate]
    public abstract class NhServiceBaseGet<T> : NhServiceBase, IGet<T>
    {
        public object Get(T request)
        {
            return TryExecute(req => Send((T)req), request);
        }

        protected abstract object Send(T request);
    }
}
