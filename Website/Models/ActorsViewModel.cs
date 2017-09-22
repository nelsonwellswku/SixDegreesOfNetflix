using Octogami.SixDegreesOfNetflix.Application.Domain;

namespace Octogami.SixDegreesOfNetflix.Website.Models
{
    public class ActorsViewModel
    {
        public string ActorOne { get; set; }
        public string ActorTwo { get; set; }

        public ActorPathViewModel ActorPath { get; set; }
    }
}