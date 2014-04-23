using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Practices.Unity;
using NHibernate.Event;
using Trex.Server.Core.Model;
using log4net;

namespace Trex.Server.Infrastructure.Implemented
{
    public class GenericEntityChangeTracker : IPostInsertEventListener, IPostUpdateEventListener, IPostDeleteEventListener
    {
        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        readonly IUnityContainer _unityContainer;

        public GenericEntityChangeTracker(IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;
        }

        public void OnPostInsert(PostInsertEvent @event)
        {
            Dispatch(typeof(IOnEntityCreated<>), "Created", @event.Entity);
        }

        public void OnPostUpdate(PostUpdateEvent @event)
        {
            Dispatch(typeof(IOnEntityUpdated<>), "Updated", @event.Entity);
        }

        public void OnPostDelete(PostDeleteEvent @event)
        {
            Dispatch(typeof(IOnEntityDeleted<>), "Deleted", @event.Entity);
        }

        void Dispatch(Type openGenericHandlerType, string methodToDispatch, object instance)
        {
            if (instance == null)
            {
                Log.WarnFormat("w00t! The instance was null!");
                return;
            }

            var entityType = instance.GetType();

            if (!typeof(EntityBase).IsAssignableFrom(entityType))
            {
                return;
            }

            try
            {
                var handlerType = openGenericHandlerType.MakeGenericType(entityType);
                var handlers = _unityContainer.ResolveAll(handlerType);

                foreach (var handler in handlers)
                {
                    try
                    {
                        handler.GetType()
                               .GetMethod(methodToDispatch)
                               .Invoke(handler, new[] { instance });
                    }
                    catch (TargetInvocationException exception)
                    {
                        throw new ApplicationException(
                            string.Format(
                                "An error occurred while dispatching the {0} method on the {1} handler for the entity instance {2}",
                                methodToDispatch, handler.GetType(), instance), exception.InnerException);
                    }
                }
            }
            catch (Exception exception)
            {
                //throw new ApplicationException(string.Format("An exception occurred while dispatching {0} ({1})",
                //                                             instance, methodToDispatch), exception);
                Log.ErrorFormat(@"
Don't worry, this is just logging! The error has been swallowed and is not visible to any users.

An error occurred while dispatching {0} ({1}): {2}",
                                instance, methodToDispatch, exception);
            }
        }
    }

    public interface IOnEntityCreated<TEntity> where TEntity : EntityBase
    {
        void Created(TEntity entity);
    }

    public interface IOnEntityUpdated<TEntity> where TEntity : EntityBase
    {
        void Updated(TEntity entity);
    }

    public interface IOnEntityDeleted<TEntity> where TEntity : EntityBase
    {
        void Deleted(TEntity entity);
    }
}
