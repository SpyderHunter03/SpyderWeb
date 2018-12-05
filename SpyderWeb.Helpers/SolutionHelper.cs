using System;
using System.IO;
using System.Linq;

namespace SpyderWeb.Helpers
{
    public class SolutionHelper
    {
        public static string GetConfigRoot()
        {
            //Get whether the app is being launched from / (deployed) or /src/SpyderWeb (debug)

            var cwd = Directory.GetCurrentDirectory();
            Console.WriteLine(cwd);
            var sln = Directory.GetFiles(cwd).Any(f => f.Contains(".sln"));
            Console.WriteLine(string.Join(", ", Directory.GetFiles(cwd)));
            if (sln) return cwd;

            var firstFileName = (Directory.GetFiles(cwd)[0]);
            var indexOfBin = firstFileName.IndexOf("bin");
            var path = firstFileName.Substring(0, indexOfBin - 1);
            return Path.Combine(path, "..");
        }
    }
}
