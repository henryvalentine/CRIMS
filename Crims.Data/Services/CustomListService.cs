using Crims.Data.Models;
using Crims.Data.Contracts;
using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using Crims.Core.Utils;

namespace Crims.Data.Services
{
    public class CustomListService : Service<CustomList>, ICustomListService
    {
        private readonly IRepositoryAsync<CustomList> _repository;
        public CustomListService(IRepositoryAsync<CustomList> repository) : base(repository)
        {
            _repository = repository;
        }

        public new void Insert(CustomList entity)
        {
            if (string.IsNullOrEmpty(entity.CustomListId))
            {
                entity.CustomListId = EntityIdGenerator.GenerateEntityId();
            }
            _repository.Insert(entity);
        }

    }
}
