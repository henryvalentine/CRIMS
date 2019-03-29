using Crims.Data.Models;
using Crims.Data.Contracts;
using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using Crims.Core.Utils;

namespace Crims.Data.Services
{
    public class CustomDataService : Service<CustomData>, ICustomDataService
    {
        private readonly IRepositoryAsync<CustomData> _repository;
        public CustomDataService(IRepositoryAsync<CustomData> repository) : base(repository)
        {
            _repository = repository;
        }

        public new void Insert(CustomData entity)
        {
            if (string.IsNullOrEmpty(entity.CustomDataId))
            {
                entity.CustomDataId = EntityIdGenerator.GenerateEntityId();
            }
            _repository.Insert(entity);
        }
    }
}
