using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using Trex.Server.Core.Services;
using Trex.Server.DataAccess;
using Trex.Server.Infrastructure.BaseClasses;
using Trex.ServiceContracts;

namespace Trex.Server.Infrastructure.Implemented
{
    public class TimeEntryService : LogableBase, ITimeEntryService
    {
        private readonly ITrexContextProvider _entityContext;
        public TimeEntryService(ITrexContextProvider contextProvider)
        {
            _entityContext = contextProvider;
        }

        public List<TimeEntry> GetTimeEntriesByPeriodAndUser(User user, DateTime startDate, DateTime endDate)
        {
            using (var db = _entityContext.TrexEntityContext)
            {
                var endDatePlusOne = endDate.Date.AddDays(1);
                var timeEntries = from te in db.TimeEntries
                                  where te.StartTime >= startDate.Date
                                        && te.EndTime < endDatePlusOne
                                        && te.UserID == user.UserID
                                  select te;
                return timeEntries.ToList();
            }
        }

        public List<TimeEntryView> GetTimeEntriesByPeriod(DateTime startDate, DateTime endDate)
        {
            var endDatePlusOne = endDate.Date.AddDays(1);
            using (var db = _entityContext.TrexEntityContext)
            {
                var timeEntries = from te in db.TimeEntryViews
                                  where te.StartTime >= startDate.Date
                                        && te.EndTime < endDatePlusOne
                                  select te;
                return timeEntries.ToList();
            }
        }

        public TimeEntry GetTimeEntryByTimeEntry(TimeEntry timeEntry)
        {
            using (var db = _entityContext.TrexEntityContext)
            {
                if (timeEntry.TimeEntryID == 0)
                {
                    if (
                        db.UsersCustomers.Any(
                            x => x.UserID == timeEntry.UserID && x.CustomerID == timeEntry.Task.Project.CustomerID))
                    {
                        timeEntry.Price =
                            db.UsersCustomers.First(
                                x => x.UserID == timeEntry.UserID && x.CustomerID == timeEntry.Task.Project.CustomerID).
                                Price;
                        return timeEntry;
                    }
                    else
                    {
                        return timeEntry;
                    }

                }

                var te =
                    db.TimeEntries.Include("Task.Project.Customer").First(
                        t => t.TimeEntryID == timeEntry.TimeEntryID);

                return te;
            }
        }

        public TimeEntry SaveTimeEntry(TimeEntry timeEntry)
        {
            timeEntry.ChangeTracker.ChangeTrackingEnabled = true;
            using (var db = _entityContext.TrexEntityContext)
            {
                timeEntry.ChangeDate = DateTime.Now;

                if (timeEntry.TimeEntryID != 0)
                    db.TimeEntries.ApplyChanges(timeEntry);
                else
                {
                    timeEntry.ChangeTracker.ChangeTrackingEnabled = false;

                    timeEntry.CreateDate = DateTime.Now;
                    timeEntry.User = null;
                    timeEntry.User1 = null;
                    db.TimeEntries.AddObject(timeEntry);
                    db.TimeEntries.ApplyChanges(timeEntry);

                    timeEntry.ChangeTracker.ChangeTrackingEnabled = true;
                }
                db.SaveChanges(SaveOptions.DetectChangesBeforeSave | SaveOptions.AcceptAllChangesAfterSave);
            }

            return timeEntry;
        }

        public void ExcludeTimeEntry(TimeEntry timeEntry)
        {
            timeEntry.InvoiceId = null;
            timeEntry.Invoice = null;
            SaveTimeEntry(timeEntry);

            
        }

        public bool DeleteTimeEntry(TimeEntry timeEntry)
        {
            using (var db = _entityContext.TrexEntityContext)
            {
                // var timeEntry = entityContext.TimeEntries.SingleOrDefault(t => t.TimeEntryID == timeEntryId);
                db.TimeEntries.Attach(timeEntry);

                db.TimeEntries.DeleteObject(timeEntry);
                db.SaveChanges();
            }

            return true;
        }

        public List<TimeEntryType> GetGlobalTimeEntryTypes()
        {
            using (var db = _entityContext.TrexEntityContext)
            {
                var timeEntryTypes = from timeEntryType in db.TimeEntryTypes
                                     where timeEntryType.Customer == null
                                     select timeEntryType;
                return timeEntryTypes.ToList();
            }
        }

        public ServerResponse UpdateTimeEntryPrice(int projectId)
        {
            try
            {
                using (var db = _entityContext.TrexEntityContext)
                {
                    var time = (from timeEntry in db.TimeEntries
                                join task in db.Tasks on timeEntry.TaskID equals task.TaskID
                                join project in db.Projects on task.ProjectID equals projectId
                                where project.ProjectID == projectId
                                select timeEntry).ToList();

                    var fixedPrice = (from project1 in db.Projects
                                      where project1.ProjectID == projectId
                                      select project1).First();

                    var totaltime = time.Where(f => f.Billable).Sum(r => r.BillableTime);

                    if (fixedPrice.FixedPrice != null)
                    {
                        var newprice = Math.Round((double)fixedPrice.FixedPrice / totaltime, 3);

                        foreach (var timeEntry in time)
                        {
                            timeEntry.Price = newprice;
                            SaveTimeEntry(timeEntry);
                        }
                    }
                    return new ServerResponse("TImeEntry price updated", true);
                }

            }
            catch (Exception e)
            {
                LogError(e);
                return new ServerResponse("Failed to update price", false);
            }
        }

        public List<TimeEntryType> GetAllTimeEntryTypes()
        {
            using (var db = _entityContext.TrexEntityContext)
            {
                return db.TimeEntryTypes.ToList();
            }
        }

        public TimeEntryType SaveTimeEntryType(TimeEntryType timeEntryType)
        {
            using (var db = _entityContext.TrexEntityContext)
            {
                db.TimeEntryTypes.ApplyChanges(timeEntryType);
                db.SaveChanges();
            }

            return timeEntryType;
        }
    }
}
