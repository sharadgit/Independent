using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;

namespace DGuide.Infrastructure
{
    public class DGuideConfiguration : DbConfiguration
    {
        public DGuideConfiguration()
        {
#if DEBUG
            DbInterception.Add(new DGuideInterceptorLogging());
#endif
        }
    }
}
