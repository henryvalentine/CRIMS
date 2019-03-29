using Crims.Data.Models;
using Crims.Data.Contracts;
using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using Crims.Core.Utils;

namespace Crims.Data.Services
{
    public class ProjectCustomListService : Service<ProjectCustomList>, IProjectCustomListService
    {
        private readonly IRepositoryAsync<ProjectCustomList> _repository;
        public ProjectCustomListService(IRepositoryAsync<ProjectCustomList> repository) : base(repository)
        {
            _repository = repository;
        }

        public override void Insert(ProjectCustomList entity)
        {
          _repository.Insert(entity);
        }
    }
}
