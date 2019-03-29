using Crims.Data.Models;
using Crims.Data.Contracts;
using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crims.Core.Utils;

namespace Crims.Data.Services
{
    public class PhotographService : Service<Photograph>, IPhotographService
    {
        private readonly IRepositoryAsync<Photograph> _repository;
        public PhotographService(IRepositoryAsync<Photograph> repository) : base(repository)
        {
            _repository = repository;
        }

        public override void Insert(Photograph entity)
        {
            if (string.IsNullOrEmpty(entity.PhotographId))
            {
                entity.PhotographId = EntityIdGenerator.GenerateEntityId();
            }
            
            base.Insert(entity);
        }
        
    }
}
