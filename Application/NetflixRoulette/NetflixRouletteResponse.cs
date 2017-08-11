// ReSharper disable InconsistentNaming

using System;
using System.Collections.Generic;
using System.Linq;

namespace Octogami.SixDegreesOfNetflix.Application.NetflixRoulette
{
    public class NetflixRouletteResponse
    {
        private readonly Lazy<List<string>> _lazyCast;

        public NetflixRouletteResponse()
        {
            _lazyCast = new Lazy<List<string>>(() => show_cast.Split(',').Select(x => x.Trim()).ToList());
        }

        public int unit { get; set; }
        public int show_id { get; set; }
        public string show_title { get; set; }
        public string release_year { get; set; }
        public string rating { get; set; }
        public string category { get; set; }
        public string show_cast { get; set; }
        public string director { get; set; }
        public string summary { get; set; }
        public string poster { get; set; }
        public int mediatype { get; set; }

        public List<string> CastMembers => _lazyCast.Value;
    }
}
