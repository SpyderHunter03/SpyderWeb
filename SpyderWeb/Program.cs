using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SpyderWeb.Bootstrap;
using SpyderWeb.Configurations;

namespace SpyderWeb
{
    public class Program
    {
        private static void Main (string[] args) => MainAsync (args).ConfigureAwait (false).GetAwaiter ().GetResult ();

        public static async Task MainAsync (string[] args)
        {
            Console.WriteLine ($"Args:{Environment.NewLine}{string.Join($",{Environment.NewLine}", args)}");

            await StartApplication ();

            // if (args.Length > 0)
            // {
            //     var inputArgs = new Credentials();
            //     for (var i = 0; i < args.Length; i++)
            //     {
            //         var arg = args[i];
            //         switch(arg)
            //         {
            //             case "-d":
            //             case "--discordToken":
            //                 if (args.Length > i+1)
            //                 {
            //                     var discordToken = args[++i];
            //                     if (!discordToken.StartsWith('-'))
            //                     {
            //                         inputArgs.DiscordToken = discordToken;
            //                     }
            //                 }
            //                 break;
            //             default:
            //                 //Ignore
            //                 break;
            //         }
            //     }

            //     if (inputArgs.DiscordToken?.Length > 0)
            //     {
            //         await StartApplication();
            //     }
            // }
        }

        private static async Task StartApplication ()
        {
            var serviceCollection = ApplicationServiceProvider.ConfigureServices ();

            //create service provider
            var serviceProvider = serviceCollection.BuildServiceProvider (true);

            //entry to run app
            var app = serviceProvider.GetService<IApp> ();

            app.Run ();

            await Task.Delay (-1);
        }
    }
}