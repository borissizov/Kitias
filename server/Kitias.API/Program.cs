using Kitias.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace Kitias.API
{
	/// <summary>
	/// Start point of app
	/// </summary>
	public class Program
	{
		/// <summary>
		/// Startup function
		/// </summary>
		/// <param name="args">Neccessary args of enviroment</param>
		/// <returns>Asynchronys</returns>
		public async static Task Main(string[] args)
		{
			var host = CreateHostBuilder(args).Build();
			using var scope = host.Services.CreateScope();
			var services = scope.ServiceProvider;
			var dbContext = services.GetRequiredService<DataContext>();

			await dbContext.Database.MigrateAsync();
			await host.RunAsync();
		}

		/// <summary>
		/// Create programm initializer
		/// </summary>
		/// <param name="args">Neccessary args of enviroment</param>
		/// <returns>Programm initializer</returns>
		public static IHostBuilder CreateHostBuilder(string[] args) =>
		  Host.CreateDefaultBuilder(args)
			.ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
	}
}
