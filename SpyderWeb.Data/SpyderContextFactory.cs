using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Linq;

namespace SpyderWeb.Data
{
    public class SpyderContextFactory : IDesignTimeDbContextFactory<SpyderContext>
    {
        //public SpyderContext CreateDbContext(DbContextFactoryOptions options)
        //{
        //    var config = new ConfigurationBuilder()
        //        //config -> /config.json
        //        // cwd -> /src/Spyder.Data/
        //        .SetBasePath(Path.Combine(options.ContentRootPath, "../.."))
        //        .AddJsonFile("config.json")
        //        .Build();

        //    var optionsBuilder = new DbContextOptionsBuilder<SpyderContext>()
        //        .UseNpgsql(config["db"]);
        //    return new SpyderContext(optionsBuilder.Options);

        //}

        public SpyderContext CreateDbContext(string[] args)
        {
            

            var config = new ConfigurationBuilder()
                // config -> /config.json
                // cwd -> /src/Spyder.Data/
                .SetBasePath(GetConfigRoot())
                .AddJsonFile("config.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<SpyderContext>()
                .UseNpgsql(config["db"]);
            return new SpyderContext(optionsBuilder.Options);
        }

        public static string GetConfigRoot()
        {
            var cwd = Directory.GetCurrentDirectory();
            var sln = Directory.GetFiles(cwd).Any(f => f.Contains("SpyderWeb.sln"));
            return sln ? cwd : Path.Combine(cwd, "../..");
        }
    }
}
