using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Kad.PMSDemo.EntityFrameworkCore;

namespace Kad.PMSDemo.HealthChecks
{
    public class PMSDemoDbContextHealthCheck : IHealthCheck
    {
        private readonly DatabaseCheckHelper _checkHelper;

        public PMSDemoDbContextHealthCheck(DatabaseCheckHelper checkHelper)
        {
            _checkHelper = checkHelper;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            if (_checkHelper.Exist("db"))
            {
                return Task.FromResult(HealthCheckResult.Healthy("PMSDemoDbContext connected to database."));
            }

            return Task.FromResult(HealthCheckResult.Unhealthy("PMSDemoDbContext could not connect to database"));
        }
    }
}
