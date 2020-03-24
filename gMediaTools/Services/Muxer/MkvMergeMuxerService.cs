using gMediaTools.Extensions;
using gMediaTools.Models.Muxer;
using gMediaTools.Services.ProcessRunner;
using gMediaTools.Services.ProcessRunner.MkvMerge;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gMediaTools.Services.Muxer
{
    public class MkvMergeMuxerService
    {
        public int Mux(IMuxer muxer, IMuxerSettings settings, out string outputFileName)
        {
            MkvMergeProcessRunnerService service = ServiceFactory.GetService<MkvMergeProcessRunnerService>();

            outputFileName = $"{settings.VideoSourceFileName}.muxed.{settings.FileExtension}".GetNewFileName();

            var parameters = service.GetAllParameters(muxer.MuxerFileName, 2);

            parameters
                .ResetParameters()
                .IncludeParameterWithValue("output", outputFileName)

                .IncludeParameterWithNoValue("options0", "no-audio")
                .IncludeParameterWithValue("file0", "file", settings.VideoSourceFileName)

                .IncludeParameterWithNoValue("options1", "no-video")
                .IncludeParameterWithNoValue("options1", "no-subtitles")
                .IncludeParameterWithNoValue("options1", "no-chapters")
                .IncludeParameterWithNoValue("options1", "no-attachments")
                .IncludeParameterWithNoValue("options1", "no-global-tags")
                .IncludeParameterWithValue("file1", "file", settings.AudioSourceFileName);

            DefaultProcessRunnerService defaultProcessRunnerService = ServiceFactory.GetService<DefaultProcessRunnerService>();

            return defaultProcessRunnerService.RunProcess(parameters, new Action<Process, string>((process, line) => Debug.WriteLine(line)));
        }
    }
}
