using System.Threading.Tasks;
using Octogami.SixDegreesOfNetflix.Application.Domain;

namespace Octogami.SixDegreesOfNetflix.Application.Data
{
    public interface IActorRepository
    {
        Task SaveActorAsync(Actor actor);
    }
}