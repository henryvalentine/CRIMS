using Crims.Data.Models;
using Crims.Data.Contracts;
using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using Crims.Core.Utils;

namespace Crims.Data.Services
{
    public class CustomListDataService : Service<CustomListData>, ICustomListDataService
    {
        private readonly IRepositoryAsync<CustomListData> _repository;
        public CustomListDataService(IRepositoryAsync<CustomListData> repository) : base(repository)
        {
            _repository = repository;
        }

        public new void Insert(CustomListData entity)
        {
            if (string.IsNullOrEmpty(entity.CustomListDataId))
            {
                entity.CustomListDataId = EntityIdGenerator.GenerateEntityId();
            }
            _repository.Insert(entity);
        }
    }
}
