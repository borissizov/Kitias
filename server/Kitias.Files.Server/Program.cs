using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace Kitias.Files.Server
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
		public static async Task Main(string[] args)
		{
			var host = CreateHostBuilder(args).Build();

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
