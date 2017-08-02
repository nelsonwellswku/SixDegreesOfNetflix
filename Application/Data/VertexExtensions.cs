using System;
using Microsoft.Azure.Graphs.Elements;

namespace Octogami.SixDegreesOfNetflix.Application.Data
{
    public static class VertexExtensions
    {
        public static Guid IdGuid(this Vertex vertex)
        {
            return Guid.Parse((string) vertex.Id);
        }
    }
}