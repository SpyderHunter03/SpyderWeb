using AutoMapper;
using OverwatchAPI;
using SpyderWeb.Models;

namespace SpyderWeb.Overwatch
{
    public class OverwatchProfile : Profile
    {
        public OverwatchProfile()
        {
            CreateMap<Player, OverwatchPlayerModel>()
                .ForMember(m => m.GamePlatform, x => x.MapFrom(m => m.Platform))
                .ReverseMap();
        }
    }
}
