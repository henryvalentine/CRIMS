using Crims.Data.Models;
using Crims.Data.Contracts;
using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using Crims.Core.Utils;

namespace Crims.Data.Services
{
    public class ProjectCustomListDataService : Service<ProjectCustomListData>, IProjectCustomListDataService
    {
        private readonly IRepositoryAsync<ProjectCustomListData> _repository;
        public ProjectCustomListDataService(IRepositoryAsync<ProjectCustomListData> repository) : base(repository)
        {
            _repository = repository;
        }

        public override void Insert(ProjectCustomListData entity)
        {
          _repository.Insert(entity);
        }
    }
}
