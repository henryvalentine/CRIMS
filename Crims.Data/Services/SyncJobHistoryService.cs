using Crims.Data.Models;
using Crims.Data.Contracts;
using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using Crims.Core.Utils;

namespace Crims.Data.Services
{
    public class SyncJobHistoryService : Service<SyncJobHistory>, ISyncJobHistoryService
    {
        private readonly IRepositoryAsync<SyncJobHistory> _repository;
        public SyncJobHistoryService(IRepositoryAsync<SyncJobHistory> repository) : base(repository)
        {
            _repository = repository;
        }

        public new void Insert(SyncJobHistory entity)
        {
            _repository.Insert(entity);
        }
    }
}
