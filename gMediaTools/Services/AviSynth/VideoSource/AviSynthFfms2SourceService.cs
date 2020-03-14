﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gMediaTools.Extensions;

namespace gMediaTools.Services.AviSynth.VideoSource
{
    public class AviSynthFfms2SourceService : IAviSynthVideoSourceService
    {
        public string GetAviSynthVideoSource(string fileName)
        {
            // Find cache file
            string cacheFilename = $"{fileName}.ffindex".GetNewFileName();

            // Find timecodes file
            string timeCodesFilename = $"{fileName}.tcodes.txt".GetNewFileName();

            return GetScript(fileName, cacheFilename, timeCodesFilename);
        }

        public string GetAviSynthVideoSourceForTimeCodes(string fileName, string timeCodesFileName)
        {
            // Find cache file
            string cacheFilename = $"{fileName}.ffindex".GetNewFileName();

            return GetScript(fileName, cacheFilename, timeCodesFileName);
        }

        private string GetScript(string fileName, string cacheFileName, string timeCodesFileName)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"FFVideoSource(source = \"{fileName}\", ");
            sb.Append($"track = -1, cache = true, cachefile = \"{cacheFileName}\", ");
            sb.Append($"fpsnum = -1, fpsden = 1, threads = -1, timecodes = \"{timeCodesFileName}\", seekmode = 1)");

            return sb.ToString();
        }

    }
}
