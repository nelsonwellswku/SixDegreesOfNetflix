using System;

namespace Octogami.SixDegreesOfNetflix.Application.NetflixRoulette
{
    public class NetflixRouletteException : InvalidOperationException
    {
        public NetflixRouletteException()
        {
        }

        public NetflixRouletteException(string message) : base(message)
        {
        }

        public NetflixRouletteException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}