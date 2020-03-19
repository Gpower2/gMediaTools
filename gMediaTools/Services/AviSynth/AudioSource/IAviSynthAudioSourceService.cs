using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gMediaTools.Services.AviSynth.AudioSource
{
    public interface IAviSynthAudioSourceService
    {
        string GetAviSynthAudioSource(string fileName, int trackNumber, bool overWriteScriptFile);
    }
}
