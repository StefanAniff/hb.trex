#region

using System;

#endregion

namespace Trex.ServiceContracts
{
    public partial class Task : IEntity, IEquatable<Task>
    {
        public Task()
        {
            Guid = Guid.NewGuid();
        }

        public bool IsSynced { get; set; }

        public string FullyQualifiedName
        {
            get
            {
                string returnName = TaskName;
                if (Project != null)
                {
                    returnName = string.Concat(returnName, " - " + Project.ProjectName);
                    if (Project.Customer != null)
                        returnName = string.Concat(returnName, "(" + Project.Customer.CustomerName + ")");
                }
                return returnName;
            }
        }

        #region IEntity Members

        public bool IsValidChild(IEntity entity)
        {
            return entity is TimeEntry;
        }

        public int Id
        {
            get { return TaskID; }
            set { TaskID = value; }
        }

        #endregion

        #region IEquatable<Task> Members

        /// <summary>
        /// 	Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns> true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false. </returns>
        /// <param name="other"> An object to compare with this object. </param>
        public bool Equals(Task other)
        {
            return this.Guid == other.Guid;
        }

        #endregion

        /// <summary>
        /// 	Creates the specified Task by the specified parameters.
        /// </summary>
        /// <param name="guid"> The GUID. </param>
        /// <param name="id"> The id. </param>
        /// <param name="name"> The name. </param>
        /// <param name="description"> The description. </param>
        /// <param name="project"> The project. </param>
        /// <param name="createDate"> The create date. </param>
        /// <returns> </returns>
        public static Task Create(Guid guid, int id, string name, string description, Project project,
                                  DateTime createDate, bool isSynced, string syncResponse)
        {
            //if (project == null)
            //    throw new MandatoryParameterMissingException("Error creating task. Project cannot be null");

            //if (name == null)
            //    throw new MandatoryParameterMissingException("Error creating task. Name cannot be null");

            var task = new Task
                           {
                               Guid = guid,
                               Id = id,
                               TaskName = name,
                               Description = description,
                               Project = project,
                               CreateDate = createDate,
                               IsSynced = isSynced,
                               //SyncResponse = syncResponse
                           };
            return task;
        }

        public override bool Equals(object obj)
        {
            var task = obj as Task;

            if (task == null)
                return false;

            if (TaskID == 0)
                return Guid == task.Guid;

            return task.TaskID == TaskID;
        }

        public override int GetHashCode()
        {
            return Guid.GetHashCode();
        }
    }
}