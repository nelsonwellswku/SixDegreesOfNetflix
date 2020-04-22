using System.Collections.Generic;

namespace Octogami.SixDegreesOfNetflix.Application.Feature.GetPathBetweenActors
{
    public class Name
    {
        public string id { get; set; }
        public string value { get; set; }
    }

    public class Title
    {
        public string id { get; set; }
        public string value { get; set; }
    }

    public class Year
    {
        public string id { get; set; }
        public string value { get; set; }
    }

    public class Properties
    {
        public List<Name> Name { get; set; }
        public List<Title> Title { get; set; }
        public List<Year> Year { get; set; }
    }

    public class Object
    {
        public string id { get; set; }
        public string label { get; set; }
        public string type { get; set; }
        public Properties properties { get; set; }
    }

    public class RootObject
    {
        public List<Object> objects { get; set; }
    }
}