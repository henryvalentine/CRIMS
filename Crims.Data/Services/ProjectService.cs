using Crims.Data.Models;
using Crims.Data.Contracts;
using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crims.Data.Services
{
    public class ProjectService : Service<Project>, IProjectService
    {
        private readonly IRepositoryAsync<Project> _repository;
        //public ProjectService()
        //{

        //}
        public ProjectService(IRepositoryAsync<Project> repository) : base(repository)
        {
            _repository = repository;
        }
       
        public override void Insert(Project entity)
        {
            entity.DateCreated = DateTime.Now;

            //Set Expiry to 50 Years time if the Expiry date Supplied is less than todays's date.
            entity.LicenseExpiryDate = entity.LicenseExpiryDate < entity.DateCreated ? DateTime.Now.AddYears(50) : entity.LicenseExpiryDate;
            base.Insert(entity);
        }

        public Project GetProject(string projectCode)
        {
            var projects = _repository.Query(b => b.ProjectCode == projectCode).Select().ToList();
            if (!projects.Any())
            {
                return new Project();
            }
            return projects[0];
        }

    }
}
