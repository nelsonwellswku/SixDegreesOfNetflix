using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Octogami.SixDegreesOfNetflix.Application.Domain
{
    public class ActorPath
    {
        public string Name { get; set; }

        public MoviePath ActedIn { get; set; }
    }
}
