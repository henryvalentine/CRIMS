using Crims.Data.Models;
using Repository.Pattern.Ef6;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Crims.Test.Fake
{
    public class FakeDataSets
    {

    }

    public class ProjectDbSet : FakeDbSet<Project>
    {
        public override Project Find(params object[] keyValues)
        {
            return this.SingleOrDefault(t => t.TableId == (int)keyValues.FirstOrDefault());
        }

        public override Task<Project> FindAsync(CancellationToken cancellationToken, params object[] keyValues)
        {
            return new Task<Project>(() => Find(keyValues));
        }
    }

    public class CustomGroupDbSet : FakeDbSet<CustomGroup>
    {
        public override CustomGroup Find(params object[] keyValues)
        {
            return this.SingleOrDefault(t => t.TableId == (int)keyValues.FirstOrDefault());
        }

        public override Task<CustomGroup> FindAsync(CancellationToken cancellationToken, params object[] keyValues)
        {
            return new Task<CustomGroup>(() => Find(keyValues));
        }
    }
}
