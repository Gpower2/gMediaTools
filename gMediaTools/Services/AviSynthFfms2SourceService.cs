using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gMediaTools.Services
{
    public class AviSynthFfms2SourceService : IAviSynthSourceService
    {
        public string GetAviSynthSource(string filename)
        {
            StringBuilder sb = new StringBuilder();

            // Find cache file
            string cacheFilename = GetFilename(filename, "ffindex");

            // Find timecodes file
            string timeCodesFilename = GetFilename(filename, "tcodes.txt");

            sb.Append($"FFVideoSource(source = \"{filename}\", ");
            sb.Append($"track = -1, cache = true, cachefile = \"{cacheFilename}\", ");
            sb.Append($"fpsnum = -1, fpsden = 1, threads = -1, timecodes = \"{timeCodesFilename}\", seekmode = 1)");

            return sb.ToString();
        }

        private string GetFilename(string baseFilename, string extension)
        {
            // Set the initial filename with the new extension
            string newFilename = $"{baseFilename}.{extension}";

            // Check if this file already exists and create a new one
            int alreadyExistingFilecounter = 0;
            while (File.Exists(newFilename))
            {
                alreadyExistingFilecounter++;
                newFilename = $"{baseFilename}.{alreadyExistingFilecounter}.{extension}";
            }

            return newFilename;
        }
    }
}
