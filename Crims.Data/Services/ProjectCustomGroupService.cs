using Crims.Data.Models;
using Crims.Data.Contracts;
using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using Crims.Core.Utils;

namespace Crims.Data.Services
{
    public class ProjectCustomGroupService : Service<ProjectCustomGroup>, IProjectCustomGroupService
    {
        private readonly IRepositoryAsync<ProjectCustomGroup> _repository;
        public ProjectCustomGroupService(IRepositoryAsync<ProjectCustomGroup> repository) : base(repository)
        {
            _repository = repository;
        }

        public override void Insert(ProjectCustomGroup entity)
        {
          _repository.Insert(entity);
        }
    }
}
