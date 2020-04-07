using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using gMediaTools.Extensions;
using gMediaTools.Factories;
using gMediaTools.Models.MediaAnalyze;
using gMediaTools.Services.AviSynth.VideoSource;
using gMediaTools.Services.TimeCodes;
using gMediaTools.Models;
using gMediaTools.Services.AviSynth.AudioSource;

namespace gMediaTools.Services.AviSynth
{
    public class AviSynthScriptService
    {
        private readonly AviSynthSourceFactory _aviSynthSourceFactory = ServiceFactory.GetService<AviSynthSourceFactory>();
        
        private readonly TimeCodesProviderService _timeCodesProviderService = ServiceFactory.GetService<TimeCodesProviderService>();
        
        private readonly TimeCodesParserService _timeCodesParserService = ServiceFactory.GetService<TimeCodesParserService>();
        
        private readonly AviSynthVfrToCfrConversionService _aviSynthVfrToCfrConversionService = ServiceFactory.GetService<AviSynthVfrToCfrConversionService>();

        public string CreateAviSynthVideoScript(MediaAnalyzeInfo mediaInfo, bool overWriteScriptFile = true)
        {
            if (mediaInfo == null)
            {
                throw new ArgumentNullException(nameof(mediaInfo));
            }
            if (string.IsNullOrWhiteSpace(mediaInfo.Filename))
            {
                throw new ArgumentException("No filename was provided!", nameof(mediaInfo.Filename));
            }

            // Get the AVS script filename
            string avsScriptFilename = $"{mediaInfo.Filename}.video.avs";
            // Check if we need to create a new script file
            if (!overWriteScriptFile) 
            {
                avsScriptFilename = avsScriptFilename.GetNewFileName();
            }

            StringBuilder avsScriptBuilder = new StringBuilder();

            // Decide on the Source filter
            //=============================
            string fileContainerFormat = mediaInfo.FileContainerFormat.Trim().ToLower();
            // Get the Source Service
            IAviSynthVideoSourceService sourceService = _aviSynthSourceFactory.GetAviSynthSourceService(fileContainerFormat);

            avsScriptBuilder.AppendLine(sourceService.GetAviSynthVideoSource(mediaInfo.Filename, false));

            // Decide what to do with VFR
            // Note: Windows Media files may report CFR frame rate mode, but in reality they are VFR inside
            //=============================
            if (fileContainerFormat.Equals("windows media") || mediaInfo.VideoInfo.FrameRateMode == VideoFrameRateMode.VFR)
            {
                // Create timecodes file
                var timecodesFileName = _timeCodesProviderService.GetTimecodesFileName(mediaInfo.Filename);

                // Read timecodes file
                var videoFrameList = _timeCodesParserService.ParseTimeCodes(timecodesFileName);

                // Create the VFR to CFR AviSynth script
                var timeCodesAviSynthScript = _aviSynthVfrToCfrConversionService.GetConvertVfrToCfrScript(videoFrameList, new List<VideoFrameSection>());

                // Append it to the main script
                avsScriptBuilder.AppendLine(timeCodesAviSynthScript);
            }

            // Decide if we need resize
            //=============================
            if (mediaInfo.TargetVideoWidth != mediaInfo.VideoInfo.Width
                || mediaInfo.TargetVideoHeight != mediaInfo.VideoInfo.Height)
            {
                avsScriptBuilder.AppendLine($"Lanczos4resize({mediaInfo.TargetVideoWidth}, {mediaInfo.TargetVideoHeight})");
            }

            // Decide if we need colorspace conversion
            //=============================
            if (!mediaInfo.VideoInfo.ColorSpace.Trim().ToLower().Equals("yv12"))
            {
                avsScriptBuilder.AppendLine("ConvertToYV12()");
            }

            // Write the file
            using (StreamWriter sw = new StreamWriter(avsScriptFilename, false, Encoding.GetEncoding(1253)))
            {
                sw.Write(avsScriptBuilder.ToString());
            }

            return avsScriptFilename;
        }

        public string CreateAviSynthAudioScript(MediaAnalyzeInfo mediaInfo, bool overWriteScriptFile = true, IAviSynthAudioSourceService audioSourceService = null)
        {
            if (mediaInfo == null)
            {
                throw new ArgumentNullException(nameof(mediaInfo));
            }
            if (string.IsNullOrWhiteSpace(mediaInfo.Filename))
            {
                throw new ArgumentException("No filename was provided!", nameof(mediaInfo.Filename));
            }

            // Get the AVS script filename
            string avsScriptFilename = $"{mediaInfo.Filename}.audio.avs";
            // Check if we need to create a new script file
            if (!overWriteScriptFile)
            {
                avsScriptFilename = avsScriptFilename.GetNewFileName();
            }

            StringBuilder avsScriptBuilder = new StringBuilder();

            IAviSynthAudioSourceService sourceService;

            // Check if no audio source service was provided
            if (audioSourceService == null)
            {
                // Use AviSynthLWLibavAudioSourceService Source filter to get the audio
                //=============================
                // Currently it has a bug in the latest versions that do not produce clips with correct audio length
                // Last good working version: r920-20161216
                // Use FFMS2 till then which seems to produce correct results
                // UPDATE: LSMASH 20200322 fixes the audio issues, switch to AviSynthLWLibavAudioSourceService
                // UPDATE: LSMASH still seems to have problems with some Windows Meadia Audio, so use FFMS2 for those files
                // UPDATE: FFMS2 also struggles with those particular audio formats, switch to DirectShowSource
                string fileContainerFormat = mediaInfo.FileContainerFormat.Trim().ToLower();
                if (fileContainerFormat.Equals("windows media"))
                {
                    //sourceService = ServiceFactory.GetService<AviSynthFfms2AudioSourceService>();
                    sourceService = ServiceFactory.GetService<AviSynthDirectShowAudioSourceService>();
                }
                else
                {
                    sourceService = ServiceFactory.GetService<AviSynthLWLibavAudioSourceService>();
                }
            }
            else
            {
                sourceService = audioSourceService;
            }

            avsScriptBuilder.AppendLine(sourceService.GetAviSynthAudioSource(mediaInfo.Filename, -1, false));

            // Write the file
            using (StreamWriter sw = new StreamWriter(avsScriptFilename, false, Encoding.GetEncoding(1253)))
            {
                sw.Write(avsScriptBuilder.ToString());
            }

            return avsScriptFilename;
        }
    }
}
