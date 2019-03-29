using Crims.Data.Models;
using Crims.Data.Contracts;
using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using Crims.Core.Utils;

namespace Crims.Data.Services
{
    public class FingerprintReasonService : Service<FingerprintReason>, IFingerprintReasonService
    {
        private readonly IRepositoryAsync<FingerprintReason> _repository;
        public FingerprintReasonService(IRepositoryAsync<FingerprintReason> repository) : base(repository)
        {
            _repository = repository;
        }

        public new void Insert(FingerprintReason entity)
        {
            _repository.Insert(entity);
        }
    }
}
