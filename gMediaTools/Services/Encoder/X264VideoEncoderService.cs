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
    public class X264VideoEncoderService
    {
        public int Encode(MediaAnalyzeInfo mediaAnalyzeInfo, string x264FileName, Action<string> logAction, Action<string> progressAction, out string outputFileName)
        {
            // Get AviSynth script
            AviSynthScriptService aviSynthScriptService = ServiceFactory.GetService<AviSynthScriptService>();

            string avsScript = aviSynthScriptService.CreateAviSynthVideoScript(mediaAnalyzeInfo);

            outputFileName = $"{mediaAnalyzeInfo.Filename}.reencode.mkv".GetNewFileName();

            X264ProcessRunnerService service = ServiceFactory.GetService<X264ProcessRunnerService>();

            var parameters = service.GetAllParameters(x264FileName);

            // Pass 1
            parameters
                .ResetParameters()
                .IncludeParameterWithValue("infile", avsScript)
                .IncludeParameterWithValue("output", outputFileName)

                .IncludeParameterWithValue("pass", "1")

                .IncludeParameterWithValue("preset", "placebo")               
                .IncludeParameterWithValue("bitrate", Math.Ceiling(mediaAnalyzeInfo.TargetVideoBitrateInKbps).ToString("#0"))
                .IncludeParameterWithValue("deblock", "-1:-1")

                .IncludeParameterWithValue("bframes", "3")
                .IncludeParameterWithValue("ref", "3")
                .IncludeParameterWithValue("qpmin", "10")
                .IncludeParameterWithValue("qpmax", "51")

                .IncludeParameterWithValue("vbv-bufsize", "50000")
                .IncludeParameterWithValue("vbv-maxrate", "50000")
                .IncludeParameterWithValue("ratetol", "2.0")
                .IncludeParameterWithValue("rc-lookahead", "40")
                .IncludeParameterWithValue("merange", "16")
                .IncludeParameterWithValue("me", "umh")
                .IncludeParameterWithValue("subme", "6")
                .IncludeParameterWithValue("trellis", "1")

                .IncludeParameterWithNoValue("no-dct-decimate")

                .IncludeParameterWithValue("muxer", "mkv");


            logAction?.Invoke($"Encoding {mediaAnalyzeInfo.Filename} with x264 1st pass...");

            DefaultProcessRunnerService defaultProcessRunnerService = ServiceFactory.GetService<DefaultProcessRunnerService>();

            defaultProcessRunnerService.RunProcess(parameters, new Action<Process, string>((process, line) => progressAction?.Invoke(line)));

            // Pass 2
            parameters.IncludeParameterWithValue("pass", "2");

            logAction?.Invoke($"Encoding {mediaAnalyzeInfo.Filename} with x264 2nd pass...");

            return defaultProcessRunnerService.RunProcess(parameters, new Action<Process, string>((process, line) => progressAction?.Invoke(line)));
        }
    }
}
