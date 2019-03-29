using Crims.Data.Models;
using Crims.Data.Contracts;
using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using Crims.Core.Utils;

namespace Crims.Data.Services
{
    public class CustomFieldTypeService : Service<CustomFieldType>, ICustomFieldTypeService
    {
        private readonly IRepositoryAsync<CustomFieldType> _repository;
        public CustomFieldTypeService(IRepositoryAsync<CustomFieldType> repository) : base(repository)
        {
            _repository = repository;
        }

        public new void Insert(CustomFieldType entity)
        {
            if (string.IsNullOrEmpty(entity.FieldTypeId))
            {
                entity.FieldTypeId = EntityIdGenerator.GenerateEntityId();
            }
            _repository.Insert(entity);
        }
       
    }
}
