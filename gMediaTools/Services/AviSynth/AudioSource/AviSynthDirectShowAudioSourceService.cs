using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gMediaTools.Services.AviSynth.AudioSource
{
    public class AviSynthDirectShowAudioSourceService : IAviSynthAudioSourceService
    {
        public string GetAviSynthAudioSource(string fileName, int trackNumber, bool overWriteScriptFile)
        {
            return $"DirectShowSource(\"{fileName}\", seek = true, video = false, audio = true, convertfps = false)";
        }
    }
}
