using Crims.Data.Models;
using Crims.Data.Contracts;
using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using Crims.Core.Utils;

namespace Crims.Data.Services
{
    public class FingerprintImageService : Service<FingerprintImage>, IFingerprintImageService
    {
        private readonly IRepositoryAsync<FingerprintImage> _repository;
        public FingerprintImageService(IRepositoryAsync<FingerprintImage> repository) : base(repository)
        {
            _repository = repository;
        }

        public new void Insert(FingerprintImage entity)
        {
            _repository.Insert(entity);
        }
    }
}
