using Crims.Data.Models;
using Crims.Data.Contracts;
using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using Crims.Core.Utils;

namespace Crims.Data.Services
{
    public class AppSettingService : Service<AppSetting>, IAppSettingService
    {
        private readonly IRepositoryAsync<AppSetting> _repository;
        public AppSettingService(IRepositoryAsync<AppSetting> repository) : base(repository)
        {
            _repository = repository;
        }
        public new void Insert(AppSetting entity)
        {
            if (string.IsNullOrEmpty(entity.Id))
            {
                entity.Id = EntityIdGenerator.GenerateEntityId();
            }
            _repository.Insert(entity);
        }


    }
}
