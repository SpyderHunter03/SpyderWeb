using AutoMapper;
using OverwatchAPI;
using SpyderWeb.Models;
using System.Threading.Tasks;

namespace SpyderWeb.Overwatch
{
    public class OverwatchService : IOverwatchService
    {
        private readonly IMapper _mapper;

        public OverwatchService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<OverwatchPlayerModel> GetOverwatchPlayerModel(string name)
        {
            using (var owClient = new OverwatchClient())
            {
                Player player = await owClient.GetPlayerAsync(name);
                return _mapper.Map<OverwatchPlayerModel>(player);
            }
        }
    }

    public interface IOverwatchService
    {
        Task<OverwatchPlayerModel> GetOverwatchPlayerModel(string name);
    }
}
