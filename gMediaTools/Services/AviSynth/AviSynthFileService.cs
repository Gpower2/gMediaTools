using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gMediaTools.Models.AviSynth;

namespace gMediaTools.Services.AviSynth
{
    public class AviSynthFileService
    {
        public AviSynthFile OpenAviSynthScriptFile(string scriptFileName)
        {
            return OpenAviSynthScriptFile(scriptFileName, AvsVideoColorspace.Unknown);
        }

        public AviSynthFile OpenAviSynthScriptFile(string scriptFileName, AvsVideoColorspace forceColorSpace)
        {
            var clip = new AviSynthClip("Import", scriptFileName, forceColorSpace);

            return new AviSynthFile(clip);
        }

        public AviSynthFile ParseAviSynthScript(string scriptContent)
        {
            return ParseAviSynthScript(scriptContent, AvsVideoColorspace.Unknown);
        }

        public AviSynthFile ParseAviSynthScript(string scriptContent, AvsVideoColorspace forceColorSpace)
        {
            var clip = new AviSynthClip("Eval", scriptContent, forceColorSpace);

            return new AviSynthFile(clip);
        }
    }
}
