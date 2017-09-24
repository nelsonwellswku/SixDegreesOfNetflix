using AutoMapper;
using Octogami.SixDegreesOfNetflix.Application.Domain;
using Octogami.SixDegreesOfNetflix.Website.Models;

namespace Octogami.SixDegreesOfNetflix.Website.Mapping
{
    public class ActorPathProfile : Profile
    {
        public ActorPathProfile()
        {
            CreateMap<ActorPath, ActorPathViewModel>();
            CreateMap<MoviePath, MoviePathViewModel>();
        }
    }
}