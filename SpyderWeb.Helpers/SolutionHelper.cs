using System;
using System.IO;
using System.Linq;

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
            var cwd = Directory.GetCurrentDirectory();
            Console.WriteLine(cwd);
            var sln = Directory.GetFiles(cwd).Any(f => f.Contains(".sln"));
            Console.WriteLine(string.Join(", ", Directory.GetFiles(cwd)));
            if (sln) return cwd;

            foreach (var file in Directory.GetDirectories(cwd))
            {
                var indexOfBin = file.IndexOf("bin");
                if (indexOfBin < 0)
                    continue;
                var path = file.Substring(0, indexOfBin - 1);
                return _configRoot = Path.Combine(path, "..");
            }
            
            throw new Exception($"Bin file not found in {cwd}");
        }
    }
}
