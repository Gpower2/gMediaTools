using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gMediaTools.Extensions;
using gMediaTools.Models;

namespace gMediaTools.Services.TimeCodes
{
    public enum TimeCodesFileVersion
    {
        Unknown,
        Version1,
        Version2
    }

    public class TimeCodesParserService
    {
        public List<VideoFrameInfo> ParseTimecodes(string timeCodesFilename, bool writeDump = false)
        {
            // Create the Video Frame List to return
            List<VideoFrameInfo> videoFrameList = new List<VideoFrameInfo>();

            try
            {
                // Open timecodes file
                using (StreamReader reader = new StreamReader(timeCodesFilename, Encoding.UTF8))
                {
                    string currentLine;
                    string previousLine = "";
                    TimeCodesFileVersion timeCodesVersion = TimeCodesFileVersion.Unknown;

                    // Read timecodes file
                    while ((currentLine = reader.ReadLine()) != null)
                    {
                        // Get currentLine to lowercase 
                        string currentLineLower = currentLine.ToLower();

                        // Check for comment lines
                        if (currentLine.StartsWith("#"))
                        {
                            if (currentLineLower.Contains("v1"))
                            {
                                timeCodesVersion = TimeCodesFileVersion.Version1;
                            }
                            else if (currentLineLower.Contains("v2"))
                            {
                                timeCodesVersion = TimeCodesFileVersion.Version2;
                            }
                        }
                        // Check for assume line
                        else if (currentLineLower.Contains("assume"))
                        {
                            currentLine = currentLineLower.Replace("assume", "").RemoveSpaces();
                        }
                        else
                        {
                            // Check for empty line
                            if (string.IsNullOrWhiteSpace(currentLine))
                            {
                                // Do nothing
                                continue;
                            }

                            switch (timeCodesVersion)
                            {
                                case TimeCodesFileVersion.Unknown:
                                    throw new Exception("Could not recognize TimeCodes file version!");
                                case TimeCodesFileVersion.Version1:
                                    AnalyseV1(currentLine, videoFrameList);
                                    break;
                                case TimeCodesFileVersion.Version2:
                                    // First Frame
                                    if (string.IsNullOrWhiteSpace(previousLine))
                                    {
                                        // Set current line as previous
                                        previousLine = currentLine;
                                        // Read the next line and set it as current
                                        currentLine = reader.ReadLine();
                                        videoFrameList.Add(AnalyseV2(previousLine, currentLine, 0));
                                        // Set the next line as previous for the next iteration
                                        previousLine = currentLine;
                                    }
                                    // Other Frames
                                    else
                                    {
                                        videoFrameList.Add(AnalyseV2(previousLine, currentLine, videoFrameList.Count));
                                        previousLine = currentLine;
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                    }

                    // Calculate last frame's FrameInfo data if v2 timecodes
                    if (timeCodesVersion == TimeCodesFileVersion.Version2)
                    {
                        videoFrameList.Add(
                            new VideoFrameInfo
                            {
                                Number = videoFrameList.Count,
                                StartTime = previousLine.ParseDecimal(),
                                Duration = videoFrameList[videoFrameList.Count - 1].Duration
                            }
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in reading timecodes file!", ex);
            }

            // Check timecode dump
            if (writeDump)
            {
                try
                {
                    StringBuilder dumpBuilder = new StringBuilder();
                    for (int i = 0; i < videoFrameList.Count; i++)
                    {
                        dumpBuilder.AppendLine($"Frame_Number:{videoFrameList[i].Number} Frame_Fps:{videoFrameList[i].FrameRate.ToString(CultureInfo.InvariantCulture)} Frame_Duration:{videoFrameList[i].Duration.ToString(CultureInfo.InvariantCulture)}");
                    }

                    // Write Timecode Dump file
                    using (StreamWriter writer = new StreamWriter($"{timeCodesFilename}.dmp".GetNewFileName(), false, Encoding.UTF8))
                    {
                        writer.Write(dumpBuilder.ToString());
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error writing timecode file dump!", ex);
                }
            }

            return videoFrameList;
        }

        private void AnalyseV1(string frameLine, List<VideoFrameInfo> videoFrameList)
        {
            // Get elements
            // elements[0] first frame
            // elememts[1] last frame
            // elements[2] framerate
            string[] elements = frameLine.Split(new string[] { "," }, StringSplitOptions.None);

            // Check for valid elements
            if (elements.Length == 3)
            {
                // Calculate FrameInfo data
                if (!int.TryParse(elements[0], out int start))
                {
                    throw new Exception("Could not parse V1 timecodes!");
                }
                if (!int.TryParse(elements[1], out int end))
                {
                    throw new Exception("Could not parse V1 timecodes!");
                }

                for (int i = start; i <= end; i++)
                {
                    VideoFrameInfo tmp = new VideoFrameInfo
                    {
                        Number = i,
                        FrameRate = elements[2].ParseDecimal()
                    };

                    if (i == 0)
                    {
                        tmp.StartTime = 0.0m;
                    }
                    else
                    {
                        tmp.StartTime = videoFrameList[i - 1].Duration + videoFrameList[i - 1].StartTime;
                    }

                    // Add FrameInfo to FrameList
                    videoFrameList.Add(tmp);
                }
            }
            else
            {
                throw new Exception("Invalid format v1 timecodes!");
            }
        }

        private VideoFrameInfo AnalyseV2(string frameLine, string nextFrameLine, int frameNumber)
        {
            // Calculate VideoFrame data
            return new VideoFrameInfo()
            {
                Number = frameNumber,
                StartTime = frameLine.ParseDecimal(),
                Duration = nextFrameLine.ParseDecimal() - frameLine.ParseDecimal()
            };
        }
    }
}
