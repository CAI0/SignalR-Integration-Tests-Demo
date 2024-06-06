using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Lolo.IntegrationTests;

public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		builder.UseUrls();
		builder.ConfigureServices(services =>
		{
			// Add any test-specific services here if needed
		});
	}
}
