using System;
using System.Collections.Generic;

namespace Octogami.SixDegreesOfNetflix.Application.Domain
{
    public class Actor
    {
        public Actor()
        {
            MoviesActedIn = new List<string>();
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public List<string> MoviesActedIn { get; set; }
    }
}