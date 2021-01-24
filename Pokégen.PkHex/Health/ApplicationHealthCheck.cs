using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Pokégen.PkHex.Health
{
	public class ApplicationHealthCheck : IHealthCheck
	{
		public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new()) 
			=> Task.FromResult(HealthCheckResult.Healthy("UP"));
	}
}
