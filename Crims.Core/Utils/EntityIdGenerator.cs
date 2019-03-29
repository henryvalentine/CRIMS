using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crims.Core.Utils
{
    public static class EntityIdGenerator
    {
        public static string GenerateEntityId()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}
