﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gMediaTools.Extensions;

namespace gMediaTools.Services.AviSynth.VideoSource
{
    public class AviSynthFfms2VideoSourceService : IAviSynthVideoSourceService
    {
        public string GetAviSynthVideoSource(string fileName, bool overWriteScriptFile)
        {
            // Find cache file
            string cacheFileName = $"{fileName}.ffindex";
            if (!overWriteScriptFile)
            {
                cacheFileName = cacheFileName.GetNewFileName();
            }

            // Find timecodes file
            string timeCodesFileName = $"{fileName}.tcodes.txt";
            if (!overWriteScriptFile) 
            {
                timeCodesFileName = timeCodesFileName.GetNewFileName();
            }

            return GetScript(fileName, cacheFileName, timeCodesFileName);
        }

        public string GetAviSynthVideoSource(string fileName, string cacheFileName, string timeCodesFileName)
        {
            return GetScript(fileName, cacheFileName, timeCodesFileName);
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
