using Crims.Data.Models;
using Crims.Data.Contracts;
using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using Crims.Core.Utils;

namespace Crims.Data.Services
{
    public class ProjectCustomFieldService : Service<ProjectCustomField>, IProjectCustomFieldService
    {
        private readonly IRepositoryAsync<ProjectCustomField> _repository;
        public ProjectCustomFieldService(IRepositoryAsync<ProjectCustomField> repository) : base(repository)
        {
            _repository = repository;
        }

        public override void Insert(ProjectCustomField entity)
        {
          _repository.Insert(entity);
        }
    }
}
