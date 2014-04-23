
using System;
using System.Collections.Generic;

namespace Trex.Server.Core.Model
{
    /// <summary>
    /// Project object for NHibernate mapped table 'Projects'.
    /// </summary>
    public class Project : EntityBase, IComparable<Project>
    {

        #region Constructors

        public Project()
        {

            Tasks = new List<Task>();
            Guid = Guid.NewGuid();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Project"/> class.
        /// </summary>
        /// <param name="projectName">Name of the project.</param>
        /// <param name="company">The customer.</param>
        /// <param name="createdBy">The created by.</param>
        /// <param name="createDate">The create date.</param>
        /// <param name="inactive">if set to <c>true</c> [inactive].</param>
        public Project(string projectName, Company company, User createdBy, DateTime createDate, DateTime? changeDate, bool inactive, bool isEstimatesEnabled)
            : this()
        {
            ProjectName = projectName;
            CreatedBy = createdBy;
            CreateDate = createDate;
            Inactive = inactive;
            Company = company;
            ChangeDate = changeDate;
            IsEstimatesEnabled = isEstimatesEnabled;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        public virtual int ProjectID { get; set; }

        /// <summary>
        /// Gets or sets the GUID.
        /// </summary>
        /// <value>The GUID.</value>
        public virtual Guid Guid { get; set; }

        /// <summary>
        /// Gets or sets the name of the project.
        /// </summary>
        /// <value>The name of the project.</value>
        public virtual string ProjectName { get; set; }


        /// <summary>
        /// Gets or sets the customer.
        /// </summary>
        /// <value>The customer.</value>
        public virtual Company Company { get; set; }

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        /// <value>The created by.</value>
        public virtual User CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the create date.
        /// </summary>
        /// <value>The create date.</value>
        public virtual DateTime CreateDate { get; set; }

        public virtual DateTime? ChangeDate { get; set; }

        public virtual User ChangedBy { get; set; }


        /// <summary>
        /// Gets or sets the tasks.
        /// </summary>
        /// <value>The tasks.</value>
        public virtual IList<Task> Tasks { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether the project is inactive.
        /// </summary>
        /// <value><c>true</c> if [in active]; otherwise, <c>false</c>.</value>
        public virtual bool Inactive { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether this instance is estimates enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is estimates enabled; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsEstimatesEnabled { get; set; }



        #endregion



        #region IComparable<Project> Members

        public virtual int CompareTo(Project other)
        {
            return this.ProjectID.CompareTo(other.ProjectID);
        }

        #endregion

        public override int EntityId
        {
            get { return ProjectID; }
        }
    }
}