using Crims.Data.Models;
using Crims.Data.Contracts;
using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using Crims.Core.Utils;

namespace Crims.Data.Services
{
    public class CustomGroupService : Service<CustomGroup>, ICustomGroupService
    {
        private readonly IRepositoryAsync<CustomGroup> _repository;
        public CustomGroupService(IRepositoryAsync<CustomGroup> repository) : base(repository)
        {
            _repository = repository;
        }

        public override void Insert(CustomGroup entity)
        {
            if (string.IsNullOrEmpty(entity.CustomGroupId))
            {
                entity.CustomGroupId = EntityIdGenerator.GenerateEntityId();
            }
            _repository.Insert(entity);
        }
    }
}
