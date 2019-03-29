using Crims.Data.Models;
using Crims.Data.Contracts;
using Repository.Pattern.Repositories;
using Service.Pattern;

namespace Crims.Data.Services
{
    public class ApprovalService : Service<Approval>, IApprovalService
    {
        private readonly IRepositoryAsync<Approval> _repository;
        public ApprovalService(IRepositoryAsync<Approval> repository) : base(repository)
        {
            _repository = repository;
        }

        public new void Insert(Approval entity)
        {
            //entity.ApprovalId = EntityIdGenerator.GenerateEntityId();
            _repository.Insert(entity);
        }
    }
}
