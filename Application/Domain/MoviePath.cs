namespace Octogami.SixDegreesOfNetflix.Application.Domain
{
    public class MoviePath
    {
        public string Title { get; set; }

        public ActorPath With { get; set; }
    }
}