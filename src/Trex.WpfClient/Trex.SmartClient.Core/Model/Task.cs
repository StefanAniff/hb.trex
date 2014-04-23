using System;
using Trex.SmartClient.Core.Exceptions;

namespace Trex.SmartClient.Core.Model
{
    public class Task : IEquatable<Task>, IComparable<Task>
    {
        private Task()
        {
        }

        public Guid Guid { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsSynced { get; set; }
        public string SyncResponse { get; set; }
        public string Description { get; set; }
        public Project Project { get; set; }
        public bool Inactive { get; set; }
        public TimeRegistrationTypeEnum TimeRegistrationType { get; set; }

        public string FullyQualifiedName
        {
            get
            {
                string returnName = Name;
                if (Project != null)
                {
                    returnName = string.Concat(returnName, " - " + Project.Name);
                    if (Project.Company != null)
                        returnName = string.Concat(returnName, "(" + Project.Company.Name + ")");
                }
                return returnName;
            }
        }

        public static Task Create(Guid guid, int id, string name, string description, Project project, DateTime createDate, bool isSynced, string syncResponse, bool inactive, TimeRegistrationTypeEnum timeRegistrationType)
        {
            if (project == null)
            {
                throw new MandatoryParameterMissingException("Error creating task. Project cannot be null");
            }

            if (name == null)
            {
                throw new MandatoryParameterMissingException("Error creating task. Name cannot be null");
            }

            var task = new Task
                {
                    Guid = guid,
                    Id = id,
                    Name = name,
                    Description = description,
                    Project = project,
                    CreateDate = createDate,
                    IsSynced = isSynced,
                    SyncResponse = syncResponse,
                    Inactive = inactive,
                    TimeRegistrationType = timeRegistrationType
                };
            return task;
        }



        public bool Equals(Task other)
        {
            return Guid == other.Guid;
        }

        public int CompareTo(Task other)
        {
            return String.CompareOrdinal(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            return Guid == ((Task) obj).Guid;
        }

        public override int GetHashCode()
        {
            return Guid.GetHashCode();
        }
    }

    public enum TimeRegistrationTypeEnum
    {
        Standard = 0,
        Projection = 1,
        Vacation,
    }
}