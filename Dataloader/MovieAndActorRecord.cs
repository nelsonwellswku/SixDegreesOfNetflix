using CsvHelper.Configuration.Attributes;

namespace Octogami.SixDegreesOfNetflix.Dataloader
{
    public class MovieAndActorRecord
    {
        public string TitleId { get; set; }

        public string NameId { get; set; }

        [Name("Movie Title")]
        public string MovieTitle { get; set; }

        public string Year { get; set; }

        public string Actor { get; set; }

        public string Characters { get; set; }
    }
}