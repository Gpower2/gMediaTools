﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gMediaTools.Services.AviSynth.VideoSource
{
    public class AviSynthDirectShowVideoSourceService : IAviSynthVideoSourceService
    {
        public string GetAviSynthVideoSource(string filename, bool overWriteScriptFile)
        {
            return $"DirectShowSource(\"{filename}\", seek = true, video = true, audio = false, convertfps = false)";
        }
    }
}
