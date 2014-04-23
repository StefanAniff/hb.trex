using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using NHibernate;
using NHibernate.Engine;
using NHibernate.Event;
using NHibernate.Event.Default;
using NHibernate.Persister.Entity;
using VMDe.Models.Model;
using VMDe.Models.ModelInterfaces;

namespace VMDe.Infrastructure.UnitOfWork
{
    internal class AuditEventListener : IPostUpdateEventListener, IPostInsertEventListener
    {
        private const string NoValueString = "*No Value*";
        private const string UnknownString = "*Unknown Value*";

        private static string GetStringValueFromStateArray(object[] stateArray, int position)
        {
            var value = stateArray[position];

            if (value is EntityBase)
            {

            }

            var val = value == null || value.ToString() == string.Empty
                          ? NoValueString
                          : value.ToString();
            return val;



        }

        private void WriteAudit(object entity, object[] oldState, IEntityPersister persister, object[] state, IEventSource eventSource, object id)
        {
            IStatelessSession session = null;
            ITransaction transaction = null;

            try
            {

                if (entity is AuditLogEntry)
                {
                    return;
                }
                var entityFullName = entity.GetType().FullName;
                if (oldState == null)
                {
                    //Hack to overcome bad saving practice!
                    oldState = Enumerable.Repeat(UnknownString as object, state.Length).ToArray();
                    //throw new ArgumentNullException("No old state available for entity type '" + entityFullName + "'. Make sure you're loading it into Session before modifying and saving it.");
                }


                bool insert = oldState.Length < 1;
                //ISessionImplementor implementor = eventSource.GetSession(EntityMode.Poco).SessionFactory.OpenStatelessSession().GetSessionImplementation();
                int[] dirtyFieldIndexes = insert ? Enumerable.Range(0, state.Length).ToArray() : persister.FindDirty(state, oldState, entity, eventSource);
                var auditEntryType = insert ? "Insert" : "Update";
                string username = Environment.UserName;

                var haveModifiedBy = entity as IHaveModifiedBy;
                if (haveModifiedBy != null)
                {
                    username = haveModifiedBy.LastModifiedBy;
                }

                session = eventSource.GetSession(EntityMode.Poco).SessionFactory.OpenStatelessSession();
                session.BeginTransaction();
                foreach (var dirtyFieldIndex in dirtyFieldIndexes)
                {
                    string oldValue = NoValueString;
                    if (!insert)
                        oldValue = GetStringValueFromStateArray(oldState, dirtyFieldIndex);
                    var newValue = GetStringValueFromStateArray(state, dirtyFieldIndex);
                    var propertyName = persister.PropertyNames[dirtyFieldIndex];
                    if (oldValue == newValue || propertyName == "LastModifiedBy" || propertyName == "LastModifiedDate")
                    {
                        continue;
                    }

                    session.Insert(new AuditLogEntry
                    {
                        EntityShortName = entity.GetType().Name,
                        FieldName = propertyName,
                        OldValue = oldValue,
                        NewValue = newValue,
                        Username = username,
                        EntityId = (int)id,
                        AuditEntryType = auditEntryType,
                        Timestamp = DateTime.Now
                    });
                }
                session.Transaction.Commit();
                session.Close();

            }
            catch (Exception)
            {
                //Don't let audit log make us fail.
                //if (session != null)
                //{
                //    if (session.Transaction.IsActive) session.Transaction.Rollback();
                //    session.Close();
                //    session = null;
                //}
                //if (eventSource != null)
                //{
                //    //if (transaction != null && transaction.IsActive) eventSource.Transaction.Rollback();
                //    //if (eventSource.Transaction.IsActive) eventSource.Transaction.Rollback();
                //    //eventSource.Close();
                //}
            }
        }

        public void OnPostUpdate(PostUpdateEvent postevent)
        {
            WriteAudit(postevent.Entity, postevent.OldState, postevent.Persister, postevent.State, postevent.Session, postevent.Id);
        }

        public void OnPostInsert(PostInsertEvent postevent)
        {
            WriteAudit(postevent.Entity, new object[0], postevent.Persister, postevent.State, postevent.Session, postevent.Id);
        }
    }
    [Serializable]
    public class FlushFixEventListener : DefaultFlushEventListener
    {

        public override void OnFlush(FlushEvent @event)
        {
            try
            {
                base.OnFlush(@event);
            }
            catch (AssertionFailure)
            {
                // throw away
            }
        }
    }
    public class AutoFlushFixEventListener : DefaultAutoFlushEventListener
    {
        public override void OnAutoFlush(AutoFlushEvent @event)
        {
            try
            {
                base.OnAutoFlush(@event);
            }
            catch (AssertionFailure)
            {
            }
        }
    }
}
