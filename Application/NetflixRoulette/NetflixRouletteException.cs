using System;

namespace Octogami.SixDegreesOfNetflix.Application.NetflixRoulette
{
    public class NetflixRouletteException : ApplicationException
    {
        public NetflixRouletteException(string message) : base(message)
        {
        }

        public NetflixRouletteException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public ErrorCode ErrorCode { get; set; }
    }
}