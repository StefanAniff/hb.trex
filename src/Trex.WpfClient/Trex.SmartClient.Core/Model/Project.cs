using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trex.SmartClient.Core.Exceptions;

namespace Trex.SmartClient.Core.Model
{
    public class Project
    {
        private Project() { }

        public int Id { get; set; }
        public string Name { get; set; }
        public Company Company { get; set; }
        public bool Inactive { get; set; }

        /// <summary>
        /// Creates a project by the specified parameters
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="name">The name.</param>
        /// <param name="company">The company.</param>
        /// <returns></returns>
        /// <exception cref="MandatoryParameterMissingException"></exception>
        public static Project Create(int id, string name, Company company,bool inactive)
        {
            if (company == null)
                throw new MandatoryParameterMissingException("Error creating project. Company cannot be null");

            if (name == null)
                throw new MandatoryParameterMissingException("Error creating project. Name cannot be null");

            return new Project() { Id = id, Name = name, Company = company ,Inactive = inactive};

        }

        #region Equality members

        protected bool Equals(Project other)
        {
            return Id == other.Id && string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Project)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Id * 397) ^ (Name != null ? Name.GetHashCode() : 0);
            }
        }

        #endregion

    }
}
