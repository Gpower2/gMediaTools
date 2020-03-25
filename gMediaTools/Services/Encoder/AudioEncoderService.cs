using gMediaTools.Extensions;
using gMediaTools.Models.AviSynth;
using gMediaTools.Models.Encoder;
using gMediaTools.Models.MediaAnalyze;
using gMediaTools.Services.AviSynth;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace gMediaTools.Services.Encoder
{
    public class AudioEncoderService
    {
        private const int MAX_SAMPLES_PER_ONCE = 4096;

        public int Encode(MediaAnalyzeInfo mediaAnalyzeInfo, IAudioEncoder audioEncoder, IAudioEncoderSettings settings, Action<string> logAction, Action<string> progressAction, out string outputFileName)
        {
            // Get AviSynth script
            AviSynthScriptService aviSynthScriptService = ServiceFactory.GetService<AviSynthScriptService>();

            string avsScript = aviSynthScriptService.CreateAviSynthAudioScript(mediaAnalyzeInfo);

            // Open the AviSynth script
            AviSynthFileService aviSynthFileService = ServiceFactory.GetService<AviSynthFileService>();

            // Determine the output filename
            outputFileName = $"{mediaAnalyzeInfo.Filename}.reencode.{settings.FileExtension}".GetNewFileName();

            // Open the AviSynth Script to generate the timecodes
            using (var avsFile = aviSynthFileService.OpenAviSynthScriptFile(avsScript))
            {
                // Check for audio existence
                if (avsFile.Clip.AudioSamplesCount == 0)
                {
                    throw new ApplicationException("Can't find audio stream!");
                }

                // Calculate Size in Bytes
                long totalSizeInBytes = avsFile.Clip.AudioSamplesCount * avsFile.Clip.AudioBytesPerSample * avsFile.Clip.AudioChannelsCount;

                // Define format type tag
                // 1 for int, 3 for float
                int formatTypeTag = 1;            
                if (avsFile.Clip.AudioSampleType == AvsAudioSampleType.FLOAT)
                {
                    formatTypeTag = 3;
                }

                using (var process = new Process())
                {
                    // Create the ProcessStartInfo object
                    ProcessStartInfo info = new ProcessStartInfo
                    {
                        // Command line arguments, to be passed to encoder
                        // {0} means output file name
                        // {1} means samplerate in Hz
                        // {2} means bits per sample
                        // {3} means channel count
                        // {4} means samplecount
                        // {5} means size in bytes
                        // {6} means format (1 int, 3 float)
                        // {7} means target bitrate
                        Arguments = string.Format(
                            audioEncoder.ExecutableArguments,
                            outputFileName,
                            avsFile.Clip.AudioSampleRate,
                            avsFile.Clip.AudioBitsPerSample,
                            avsFile.Clip.AudioChannelsCount,
                            avsFile.Clip.AudioSamplesCount,
                            totalSizeInBytes,
                            formatTypeTag,
                            mediaAnalyzeInfo.TargetAudioBitrate
                        ),

                        FileName = audioEncoder.EncoderFileName,

                        UseShellExecute = false,
                        RedirectStandardInput = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    };

                    process.StartInfo = info;

                    Debug.WriteLine(info.Arguments);

                    // Start the process
                    process.Start();

                    // TODO: Revisit that
                    //process.PriorityClass = m_processPriority;

                    // Read the Standard output character by character
                    Task.Run(() => process.ReadStreamPerCharacter(true, new Action<Process, string>((p, str) => Debug.WriteLine(str))));

                    // Read the Standard error character by character
                    Task.Run(() => process.ReadStreamPerCharacter(false, new Action<Process, string>((p, str) => Debug.WriteLine(str))));

                    try
                    {
                        using (Stream processInputStream = process.StandardInput.BaseStream)
                        {
                            // Check if we need to write WAV Header
                            if (audioEncoder.WriteHeader)
                            {
                                logAction?.Invoke($"Audio encoding: {mediaAnalyzeInfo.Filename} Writing header data to encoder's StdIn...");
                                WriteHeader(audioEncoder.HeaderType, processInputStream, avsFile, totalSizeInBytes, settings.ChannelMask, formatTypeTag);
                            }

                            logAction?.Invoke($"Audio encoding: {mediaAnalyzeInfo.Filename} Writing PCM data to encoder's StdIn...");

                            // Calculate the frame buffer total size
                            int frameBufferTotalSize = MAX_SAMPLES_PER_ONCE * avsFile.Clip.AudioChannelsCount * avsFile.Clip.AudioBitsPerSample / 8;

                            // Allocate the frame buffer
                            byte[] frameBuffer = new byte[frameBufferTotalSize];

                            // Get the handle for the frame buffer
                            GCHandle bufferHandle = GCHandle.Alloc(frameBuffer, GCHandleType.Pinned);

                            try
                            {
                                // Set a current frame sample indicator
                                int currentFrameSample = 0;

                                // Start passing the audio frames to the encoder's standard input stream
                                while (currentFrameSample < avsFile.Clip.AudioSamplesCount)
                                {
                                    // Check for unexpected process exit
                                    if (process != null && process.HasExited)
                                    {
                                        throw new ApplicationException($"Unexpected encoder termination with exit code: {process.ExitCode}");
                                    }

                                    // Calculate how many frame samples to read
                                    int framesSamplesToRead = Math.Min((int)(avsFile.Clip.AudioSamplesCount - currentFrameSample), MAX_SAMPLES_PER_ONCE);

                                    int bytesRead = framesSamplesToRead * avsFile.Clip.AudioBytesPerSample * avsFile.Clip.AudioChannelsCount;

                                    // Read the audio frame samples and copy them to the frame buffer
                                    avsFile.ReadAudioSamples(bufferHandle.AddrOfPinnedObject(), currentFrameSample, framesSamplesToRead);

                                    // Calculate the current progress
                                    double progress = ((double)currentFrameSample / (double)avsFile.Clip.AudioSamplesCount) * 100.0;
                                    progressAction?.Invoke($"Progress {progress:#0.00}%");

                                    // Write the frame samples to the encoder's standard input stream
                                    processInputStream.Write(frameBuffer, 0, bytesRead);
                                    processInputStream.Flush();

                                    // Advance the current frame sample indicator
                                    currentFrameSample += framesSamplesToRead;

                                    // Signal the OS to run other threads in our time slice
                                    Thread.Yield();
                                }
                            }
                            finally
                            {
                                // Free the frame buffer handle
                                bufferHandle.Free();
                            }
                        }

                        if (process != null)
                        {
                            logAction?.Invoke($"Audio encoding: {mediaAnalyzeInfo.Filename} Finalizing encoder");

                            // Wait for the process to exit
                            process.WaitForExit();

                            // Debug write the exit code
                            Debug.WriteLine($"Exit code: {process.ExitCode}");
                        }
                    }
                    finally
                    {
                        // Sanity check for non exited process
                        if (process != null && !process.HasExited)
                        {
                            // Kill the process
                            process.Kill();

                            // Wait for the process to exit
                            process.WaitForExit();

                            // Debug write the exit code
                            Debug.WriteLine($"Exit code: {process.ExitCode}");
                        }
                    }

                    // Return the process exit code
                    return process?.ExitCode ?? 0;
                }
            }
        }

        private void WriteHeader(AudioHeaderType headerType, Stream target, AviSynthFile avsFile, long totalSizeInBytes, int channelMask, int formatTypeTag)
        {
            const uint FAAD_MAGIC_VALUE = 0xFFFFFF00;

            bool Greater4GB = totalSizeInBytes >= (uint.MaxValue - 68);
            bool WExtHeader = channelMask >= 0;
            uint HeaderSize = (uint)(WExtHeader ? 60 : 36);

            int[] defmask = { 0, 4, 3, 7, 51, 55, 63, 319, 1599, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            if (headerType ==  AudioHeaderType.W64)
            {
                HeaderSize = (uint)(WExtHeader ? 128 : 112);
                target.Write(Encoding.ASCII.GetBytes("riff"), 0, 4);
                target.Write(BitConverter.GetBytes((uint)0x11CF912E), 0, 4);  // GUID
                target.Write(BitConverter.GetBytes((uint)0xDB28D6A5), 0, 4);
                target.Write(BitConverter.GetBytes((uint)0x0000C104), 0, 4);
                target.Write(BitConverter.GetBytes((totalSizeInBytes + HeaderSize)), 0, 8);
                target.Write(Encoding.ASCII.GetBytes("wave"), 0, 4);
                target.Write(BitConverter.GetBytes((uint)0x11D3ACF3), 0, 4);  // GUID
                target.Write(BitConverter.GetBytes((uint)0xC000D18C), 0, 4);
                target.Write(BitConverter.GetBytes((uint)0x8ADB8E4F), 0, 4);
                target.Write(Encoding.ASCII.GetBytes("fmt "), 0, 4);
                target.Write(BitConverter.GetBytes((uint)0x11D3ACF3), 0, 4);  // GUID
                target.Write(BitConverter.GetBytes((uint)0xC000D18C), 0, 4);
                target.Write(BitConverter.GetBytes((uint)0x8ADB8E4F), 0, 4);
                target.Write(BitConverter.GetBytes(WExtHeader ? (ulong)64 : (ulong)48), 0, 8);
            }
            else if (headerType ==  AudioHeaderType.RF64)
            {
                HeaderSize += 36;
                target.Write(Encoding.ASCII.GetBytes("RF64"), 0, 4);
                target.Write(BitConverter.GetBytes((uint)0xFFFFFFFF), 0, 4);
                target.Write(Encoding.ASCII.GetBytes("WAVE"), 0, 4);
                target.Write(Encoding.ASCII.GetBytes("ds64"), 0, 4);  // new ds64 chunk 36 bytes
                target.Write(BitConverter.GetBytes((uint)28), 0, 4);
                target.Write(BitConverter.GetBytes((totalSizeInBytes + HeaderSize)), 0, 8);
                target.Write(BitConverter.GetBytes((totalSizeInBytes)), 0, 8);
                target.Write(BitConverter.GetBytes((avsFile.Clip.AudioSamplesCount)), 0, 8);
                target.Write(BitConverter.GetBytes((uint)0x0000), 0, 4);
                target.Write(Encoding.ASCII.GetBytes("fmt "), 0, 4);
                target.Write(BitConverter.GetBytes(WExtHeader ? (uint)40 : (uint)16), 0, 4);
            }
            else
            {
                // Assume RIFF (WAV)
                target.Write(Encoding.ASCII.GetBytes("RIFF"), 0, 4);
                target.Write(BitConverter.GetBytes(Greater4GB ? (FAAD_MAGIC_VALUE + HeaderSize) : (uint)(totalSizeInBytes + HeaderSize)), 0, 4);
                target.Write(Encoding.ASCII.GetBytes("WAVE"), 0, 4);
                target.Write(Encoding.ASCII.GetBytes("fmt "), 0, 4);
                target.Write(BitConverter.GetBytes(WExtHeader ? (uint)40 : (uint)16), 0, 4);
            }

            // fmt chunk common
            target.Write(BitConverter.GetBytes(WExtHeader ? (uint)0xFFFE : (uint)formatTypeTag), 0, 2);
            target.Write(BitConverter.GetBytes(avsFile.Clip.AudioChannelsCount), 0, 2);
            target.Write(BitConverter.GetBytes(avsFile.Clip.AudioSampleRate), 0, 4);
            target.Write(BitConverter.GetBytes(avsFile.Clip.AudioBitsPerSample * avsFile.Clip.AudioSampleRate * avsFile.Clip.AudioChannelsCount / 8), 0, 4);
            target.Write(BitConverter.GetBytes(avsFile.Clip.AudioChannelsCount * avsFile.Clip.AudioBitsPerSample / 8), 0, 2);
            target.Write(BitConverter.GetBytes(avsFile.Clip.AudioBitsPerSample), 0, 2);

            // if WAVE_FORMAT_EXTENSIBLE continue fmt chunk
            if (WExtHeader)
            {
                int channelMaskForHeader = channelMask;
                if (channelMask == 0)
                {
                    channelMaskForHeader = defmask[avsFile.Clip.AudioChannelsCount];
                }

                target.Write(BitConverter.GetBytes((uint)0x16), 0, 2);
                target.Write(BitConverter.GetBytes(avsFile.Clip.AudioBitsPerSample), 0, 2);
                target.Write(BitConverter.GetBytes(channelMaskForHeader), 0, 4);
                target.Write(BitConverter.GetBytes(formatTypeTag), 0, 4);
                target.Write(BitConverter.GetBytes((uint)0x00100000), 0, 4); // GUID
                target.Write(BitConverter.GetBytes((uint)0xAA000080), 0, 4);
                target.Write(BitConverter.GetBytes((uint)0x719B3800), 0, 4);
            }

            // data chunk
            if (headerType == AudioHeaderType.W64)
            {
                if (!WExtHeader)
                {
                    target.Write(BitConverter.GetBytes((uint)0x0000D000), 0, 4); // pad
                    target.Write(BitConverter.GetBytes((uint)0x0000D000), 0, 4);
                }

                target.Write(Encoding.ASCII.GetBytes("data"), 0, 4);
                target.Write(BitConverter.GetBytes((uint)0x11D3ACF3), 0, 4);  // GUID
                target.Write(BitConverter.GetBytes((uint)0xC000D18C), 0, 4);
                target.Write(BitConverter.GetBytes((uint)0x8ADB8E4F), 0, 4);
                target.Write(BitConverter.GetBytes(totalSizeInBytes + 24), 0, 8);
            }
            else if (headerType == AudioHeaderType.RF64)
            {
                target.Write(Encoding.ASCII.GetBytes("data"), 0, 4);
                target.Write(BitConverter.GetBytes((uint)0xFFFFFFFF), 0, 4);
            }
            else
            {
                // Assume RIFF (WAV)
                target.Write(Encoding.ASCII.GetBytes("data"), 0, 4);
                target.Write(BitConverter.GetBytes(Greater4GB ? FAAD_MAGIC_VALUE : (uint)totalSizeInBytes), 0, 4);
            }
        }

    }
}
