using Crims.Data.Models;
using Crims.Data.Contracts;
using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using Crims.Core.Utils;

namespace Crims.Data.Services
{
    public class FingerprintTemplateService : Service<FingerprintTemplate>, IFingerprintTemplateService
    {
        private readonly IRepositoryAsync<FingerprintTemplate> _repository;
        public FingerprintTemplateService(IRepositoryAsync<FingerprintTemplate> repository) : base(repository)
        {
            _repository = repository;
        }

        public new void Insert(FingerprintTemplate entity)
        {
            _repository.Insert(entity);
        }
    }
}
