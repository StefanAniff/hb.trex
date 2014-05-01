namespace Trex.Common.DataTransferObjects.ProjectAdministration
{
    /// <summary>
    /// Dto for project administration.
    /// In HB's domain there is always only one task in every project
    /// </summary>
    public class ProjectTaskDto
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Inactive { get; set; }
        public UserDto Manager { get; set; }
    }

    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Initials { get; set; }
    }
}