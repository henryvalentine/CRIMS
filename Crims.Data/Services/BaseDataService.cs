using Crims.Data.Models;
using Crims.Data.Contracts;
using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Linq;
using System.Net;
using Crims.Core.Utils;

namespace Crims.Data.Services
{
    public class BaseDataService : Service<BaseData>, IBaseDataService
    {
        private readonly IRepositoryAsync<BaseData> _repository;
        public BaseDataService(IRepositoryAsync<BaseData> repository) : base(repository)
        {
            _repository = repository;
        }

        public new void Insert(BaseData entity)
        {
            //entity.BaseDataId = EntityIdGenerator.GenerateEntityId();
            _repository.Insert(entity);
        }

        public BaseData GetBaseData(string enrollmentId)
        {
            var baseDatas = _repository.Query(b => b.EnrollmentId == enrollmentId).Select().ToList();
            if (!baseDatas.Any())
            {
                return new BaseData();
            }
            return baseDatas[0];
        }

    }
}
