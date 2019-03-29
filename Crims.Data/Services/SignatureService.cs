using Crims.Data.Models;
using Crims.Data.Contracts;
using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using Crims.Core.Utils;

namespace Crims.Data.Services
{
    public class SignatureService : Service<Signature>, ISignatureService
    {
        private readonly IRepositoryAsync<Signature> _repository;
        public SignatureService(IRepositoryAsync<Signature> repository) : base(repository)
        {
            _repository = repository;
        }

        //public new void Insert(Signature entity)
        //{
        //    //entity.SignatureId = EntityIdGenerator.GenerateEntityId();
        //    _repository.Insert(entity);
        //}
    }
}
