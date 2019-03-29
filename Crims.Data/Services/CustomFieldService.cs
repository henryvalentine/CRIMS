using Crims.Data.Models;
using Crims.Data.Contracts;
using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using Crims.Core.Utils;

namespace Crims.Data.Services
{
    public class CustomFieldService : Service<CustomField>, ICustomFieldService
    {
        private readonly IRepositoryAsync<CustomField> _repository;
        public CustomFieldService(IRepositoryAsync<CustomField> repository) : base(repository)
        {
            _repository = repository;
        }

        public override void Insert(CustomField entity)
        {
            if (string.IsNullOrEmpty(entity.CustomFieldId))
            {
                entity.CustomFieldId = EntityIdGenerator.GenerateEntityId();
            }
          _repository.Insert(entity);
        }
    }
}
