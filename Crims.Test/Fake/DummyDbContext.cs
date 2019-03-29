using Crims.Data.Models;
using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crims.Test.Fake
{
    public class DummyDbContext : FakeDbContext
    {
        public DummyDbContext()
        {
            AddFakeDbSet<Project, ProjectDbSet>();
            AddFakeDbSet<CustomGroup, CustomGroupDbSet>();
        }
    }
}
