using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SpyderWeb.Helpers
{
    public class SolutionHelper
    {
        private static string _configRoot { get; set; } = string.Empty;

        public SolutionHelper()
        {
            _configRoot = string.Empty;
        }

        public static string GetConfigRoot()
        {
            // Get cached information first
            if (_configRoot != string.Empty) return _configRoot;

            // If it isn't cached, go get it:

            // Get whether the app is being launched from / (deployed) or /src/SpyderWeb (debug)
            var dir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            Console.WriteLine($"Current Assembly Directory: {dir}");
            Console.WriteLine($"Current Assembly Directory Files:{Environment.NewLine}{string.Join($",{Environment.NewLine}", Directory.GetFiles(dir))}");
            if (Directory.GetFiles(dir).Any(f => f.Contains("config.json")))
                return _configRoot = dir;

            throw new Exception($"Config.json not found in {dir}");
        }
    }
}
