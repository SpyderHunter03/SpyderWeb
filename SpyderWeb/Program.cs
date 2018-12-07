using Microsoft.Extensions.DependencyInjection;
using SpyderWeb.Bootstrap;
using System.Threading.Tasks;

namespace SpyderWeb
{
    public class Program
    {
        private static void Main(string[] args)
            => MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();

        public static async Task MainAsync(string[] args)
        {
            var serviceCollection = ApplicationServiceProvider.ConfigureServices();

            //create service provider
            var serviceProvider = serviceCollection.BuildServiceProvider(true);

            //entry to run app
            var app = serviceProvider.GetService<IApp>();

            app.Run();

            await Task.Delay(-1);
        }
    }
}