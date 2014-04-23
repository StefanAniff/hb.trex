


namespace Trex.ServiceContracts
{
    public partial class Project:IEntity
    {
        public Project()
        {
            Guid = System.Guid.NewGuid();
        }

        public bool IsValidChild(IEntity entity)
        {
            return entity is Task;
        }

        public int Id
        {
            get { return ProjectID; }
            set { ProjectID = value; }
        }

        public override bool Equals(object obj)
        {
            var project = obj as Project;

            if (project == null)
                return false;

            if (ProjectID == 0)
                return Guid == project.Guid;

            return project.ProjectID == ProjectID;

        }

        public override int GetHashCode()
        {
            return ProjectID.GetHashCode();
        }

        /// <summary>
        /// Creates a project by the specified parameters
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="name">The name.</param>
        /// <param name="company">The company.</param>
        /// <returns></returns>
        /// <exception cref="MandatoryParameterMissingException"></exception>
        public static Project Create(int id, string name, Customer company)
        {
            //if (company == null)
            //    throw new MandatoryParameterMissingException("Error creating project. Company cannot be null");

            //if (name == null)
            //    throw new MandatoryParameterMissingException("Error creating project. Name cannot be null");

            return new Project() { Id = id, ProjectName = name, Customer = company };

        }

    }
}
