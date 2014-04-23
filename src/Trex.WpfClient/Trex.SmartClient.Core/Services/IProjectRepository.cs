using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trex.SmartClient.Core.Model;

namespace Trex.SmartClient.Core.Services
{
    public interface IProjectRepository
    {
        Project GetById(int id);
        List<Project> GetAll();
        List<Project> GetBySearchStringAndCompany(string searchString, Company company);
        List<Project> GetBySearchString(string searchString);
        void AddOrUpdate(List<Project> projects);


        List<Project> GetAllActive();
    }
}