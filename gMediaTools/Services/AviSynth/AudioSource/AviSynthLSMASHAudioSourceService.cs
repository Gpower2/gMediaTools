using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gMediaTools.Services.AviSynth.AudioSource
{
    public class AviSynthLSMASHAudioSourceService : IAviSynthAudioSourceService
    {
        public string GetAviSynthAudioSource(string fileName, int trackNumber, bool overWriteScriptFile)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"LSMASHAudioSource(source = \"{fileName}\", ");
            sb.Append($"track = {trackNumber}, rate = 0)");

            return sb.ToString();
        }
    }
}
