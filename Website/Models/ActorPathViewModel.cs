using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Octogami.SixDegreesOfNetflix.Website.Models
{
    public class ActorPathViewModel
    {
        public string Name { get; set; }

        public MoviePathViewModel ActedIn { get; set; }
    }
}
