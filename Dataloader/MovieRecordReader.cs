using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;

namespace Octogami.SixDegreesOfNetflix.Dataloader
{
    public interface IMovieRecordReader
    {
        List<MovieAndActorRecord> ReadRecords(string filePath);
    }

    public class MovieRecordReader : IMovieRecordReader
    {
        public List<MovieAndActorRecord> ReadRecords(string filePath)
        {
            List<MovieAndActorRecord> records = new List<MovieAndActorRecord>();
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var list = csv.GetRecords<MovieAndActorRecord>().ToList();
                return list;
            }
        }
    }
}