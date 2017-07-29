using System.Text.RegularExpressions;

namespace Octogami.SixDegreesOfNetflix.Application.NetflixRoulette
{
    public class NetflixRouletteRequest
    {
        private string _title;
        private int _year;

        private readonly Regex _inputValidation = new Regex(@"^[a-z \.]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public string Title
        {
            get => _title;
            set
            {
                if (Actor != null)
                {
                    throw new NetflixRouletteException("Cannot search by title when already searching by actor.");
                }

                if (Director != null)
                {
                    throw new NetflixRouletteException("Cannot search by title when already searching by director.");
                }

                if (!_inputValidation.IsMatch(value))
                {
                    throw new NetflixRouletteException("Title may only contain alphanumeric characters, spaces, and periods.");
                }

                _title = value;
            }
        }

        public int Year
        {
            get => _year;
            set
            {
                if (_year < 1900 || _year > 2050)
                {
                    throw new NetflixRouletteException("Year must be between 1900 and 2050.")
                    {
                        ErrorCode = ErrorCode.InvalidInput
                    };
                }

                _year = value;
            }
        }

        public string Actor { get; set; }

        public string Director { get; set; }
    }
}
