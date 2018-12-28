using System.Threading.Tasks;

namespace SpyderWeb.FacebookCore.Interfaces
{
    public interface IFacebookService
    {
        Task<Account> GetAccountAsync(string accessToken);
        Task PostOnWallAsync(string accessToken, string message);
    }
}
