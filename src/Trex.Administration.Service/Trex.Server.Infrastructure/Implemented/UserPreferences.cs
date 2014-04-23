using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trex.Server.Core;
using System.Web.Profile;
using System.Web;
using Trex.Server.Core.Services;

namespace Trex.Server.Infrastructure.Implemented
{
    public class UserPreferences : IUserPreferences
    {
        

        public UserPreferences()
        {

        }

        #region IUserPreferences Members


        public bool ShowAllTasks
        {
            get
            {
                return (bool)HttpContext.Current.Profile["UserFilterShowAllTasks"];
            }
            set
            {
                HttpContext.Current.Profile.SetPropertyValue("UserFilterShowAllTasks", value);
            }
        }

        public bool ShowOnlyTasksCreatedByUser
        {
            get
            {
                return (bool)HttpContext.Current.Profile["UserFilterShowOnlyTasksCreatedByUser"];
            }
            set
            {
                HttpContext.Current.Profile.SetPropertyValue("UserFilterShowOnlyTasksCreatedByUser",value);
            }
        }

        public bool ShowOnlyTasksWithTimeEntriesCreatedByUser
        {
            get
            {
                return (bool)HttpContext.Current.Profile["UserFilterShowOnlyTasksWithTimeEntriesByUser"];
            }
            set
            {
                HttpContext.Current.Profile.SetPropertyValue("UserFilterShowOnlyTasksWithTimeEntriesByUser", value);
            }
        }

        public void Save()
        {
            HttpContext.Current.Profile.Save();
        }

        #endregion
    }
}
