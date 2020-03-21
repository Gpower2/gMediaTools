using gMediaTools.Extensions;
using gMediaTools.Models.MediaAnalyze;
using gMediaTools.Services.AviSynth;
using gMediaTools.Services.ProcessRunner;
using gMediaTools.Services.ProcessRunner.x264;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gMediaTools.Services.Encoder
{
    public class VideoEncoderService
    {
        public void Encode(MediaAnalyzeInfo mediaAnalyzeInfo, string x264FileName)
        {
            // Get AviSynth script
            AviSynthScriptService aviSynthScriptService = ServiceFactory.GetService<AviSynthScriptService>();

            string avsScript = aviSynthScriptService.CreateAviSynthVideoScript(mediaAnalyzeInfo);

            string outputFile = $"{mediaAnalyzeInfo.Filename}.reencode.mkv".GetNewFileName();

            X264ProcessRunnerService service = ServiceFactory.GetService<X264ProcessRunnerService>();

            var parameters = service.GetAllParameters(x264FileName);

            parameters
                .ResetParameters()
                .IncludeParameterWithValue("infile", avsScript)
                .IncludeParameterWithValue("output", outputFile)
                .IncludeParameterWithValue("muxer", "mkv");

            DefaultProcessRunnerService defaultProcessRunnerService = ServiceFactory.GetService<DefaultProcessRunnerService>();

            defaultProcessRunnerService.RunProcess(parameters, new Action<System.Diagnostics.Process, string>((process, line) => Debug.WriteLine(line)));
        }
    }
}
