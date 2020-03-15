using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gMediaTools.Extensions;
using gMediaTools.Models;

namespace gMediaTools.Services.AviSynth
{
    public class AviSynthVfrToCfrConversionService
    {
        public string GetConvertVfrToCfrScript(List<VideoFrameInfo> videoFrameList, List<VideoFrameSection> videoFrameSections, decimal targetVideoFrameRate)
        {
            // Calculate target duration
            decimal targetDuration = 1000.0m / targetVideoFrameRate;

            // Define maximum frame range 
            int maxFrameRound = 5;

            // Check for sections
            if (!videoFrameSections.Any())
            {
                // If no sections, create a dummy section for the whole video
                videoFrameSections.Add(
                    new VideoFrameSection()
                    {
                        StartFrameNumber = 0,
                        EndFrameNumber = videoFrameList.Count - 1,
                        Name = "wholeVideo"
                    }
                );
            }

            decimal frameGlitch = 0.0m;
            decimal frameSectionGlitch = 0.0m;

            //Loop for every section
            foreach (VideoFrameSection section in videoFrameSections)
            {
                // Reset data just to be sure
                section.FramesToDelete.Clear();
                section.FramesToDuplicate.Clear();

                // Declare needed variables
                int currentSectionFrame = 0;
                decimal sectionStartTime = videoFrameList.FirstOrDefault(f => f.Number == section.StartFrameNumber).StartTime;
                int gatherStartFrame = section.StartFrameNumber;
                int gatherEndFrame = section.EndFrameNumber;

                int sectionCounter = 0;
                decimal currentCheckTime = 0.0m;
                decimal currentShouldBeTime = 0.0m;
                decimal currentTimeDiff = 0.0m;

                // Check Section Name
                if (string.IsNullOrWhiteSpace(section.Name))
                {
                    //If no name, set a default
                    section.Name = $"Section{sectionCounter + 1}";
                }

                // Loop for all the frames in the section
                for (int currentFrameNumber = section.StartFrameNumber; currentFrameNumber <= section.EndFrameNumber; currentFrameNumber++)
                {
                    // Increase the current frame counter
                    currentSectionFrame++;

                    // Calculate the current time to check by removing the section start time
                    currentCheckTime = videoFrameList.FirstOrDefault(f => f.Number == currentFrameNumber).EndTime - sectionStartTime;

                    // Calculate the expected time based on the CFR duration
                    currentShouldBeTime = (currentSectionFrame - section.FramesToDelete.Count + section.FramesToDuplicate.Count) * targetDuration;

                    // Calculate the time difference
                    currentTimeDiff = currentCheckTime - currentShouldBeTime;

                    // Calculate how many frames off we are based on time diff and target duration
                    frameGlitch = currentTimeDiff / targetDuration;

                    // Add the cumulative frame glitch 
                    frameGlitch += frameSectionGlitch;

                    // Check if we have more than 1 frame glitch
                    if (frameGlitch > -1.0m && frameGlitch < 1.0m)
                    {
                        continue;
                    }

                    // Check if we need to delete or duplicate frames
                    if (frameGlitch <= -1.0m)
                    {
                        // Negative frame glitch, we need to delete

                        // Decide how many frames we need to delete
                        // We use the lowest possible number of frames
                        // e.g -1.9 => 1 frame to delete
                        int numberOfFramesToDelete = -Convert.ToInt32(Math.Ceiling(frameGlitch));

                        // Set the current frame as the end range frame
                        gatherEndFrame = currentFrameNumber;

                        // Check frame range based on the previously set start frame
                        if (gatherEndFrame - gatherStartFrame > maxFrameRound)
                        {
                            // We exceeded the max range of frames per round
                            // Reevaluate the start frame
                            gatherStartFrame = gatherEndFrame - maxFrameRound;
                        }

                        // Delete frames
                        // Update frame section glitch
                        frameSectionGlitch = frameGlitch + DeleteFrames(videoFrameList, section, gatherStartFrame, gatherEndFrame, numberOfFramesToDelete);
                    }
                    else if (frameGlitch >= 1.0m)
                    {
                        // Positive frame glitch, we need to duplicate

                        // Decide how many frames we need to duplicate
                        // We use the lowest possible number of frames
                        // e.g 1.9 => 1 frame to duplicate
                        int numberOfFramesToDuplicate = Convert.ToInt32(Math.Floor(frameGlitch));

                        // Set the current frame as the end range frame
                        gatherEndFrame = currentFrameNumber;

                        // Check frame range based on the previously set start frame
                        if (gatherEndFrame - gatherStartFrame > maxFrameRound)
                        {
                            // We exceeded the max range of frames per round
                            // Reevaluate the start frame
                            gatherStartFrame = gatherEndFrame - maxFrameRound;
                        }

                        // Duplicate frames
                        // Update frame section glitch
                        frameSectionGlitch = frameGlitch - DuplicateFrames(videoFrameList, section, gatherStartFrame, gatherEndFrame, numberOfFramesToDuplicate);
                    }

                    // Set the gather start frame
                    if (currentFrameNumber + 1 > videoFrameList.Last().Number)
                    {
                        gatherStartFrame = videoFrameList.Last().Number;
                    }
                    else
                    {
                        gatherStartFrame = currentFrameNumber + 1;
                    }
                }

                // Increase the section counter
                sectionCounter++;

                // Set the frame Section Glitch
                frameSectionGlitch = frameGlitch;
            }

            StringBuilder sb = new StringBuilder();

            // Write the delete and duplicate frame for each section
            foreach (VideoFrameSection section in videoFrameSections)
            {
                sb.AppendLine(KienzanString(section));
            }
            
            return sb.ToString();
        }

        private int DuplicateFrames(List<VideoFrameInfo> allFrames, VideoFrameSection videoFrameSection, int startFrame, int endFrame, int numberOfFramesToDuplicate)
        {
            // Create a temporary list with only the frame range
            List<VideoFrameInfo> list = allFrames.Where(f => f.Number >= startFrame && f.Number <= endFrame).ToList();

            IOrderedEnumerable<VideoFrameInfo> orderedList;

            // if the list is CFR then sort by frame difference, else by frame numer
            if (list.IsCFR())
            {
                // Sort all frames by difference
                orderedList = list.OrderBy(f => f.DifferencePercentageFromPreviousFrame);
                //list.Sort(VideoFrameList.SortType.ByFrameDifference, VideoFrameList.SortOrder.Ascending);
            }
            else
            {
                // Sort the first addFrames by frame number
                orderedList = list.OrderBy(f => f.Number);
                //list.Sort(VideoFrameList.SortType.ByFrameNumber, VideoFrameList.SortOrder.Ascending);
            }

            // Finally sort all the frames by duration
            orderedList = orderedList.ThenByDescending(f => f.Duration);
            //list.Sort(VideoFrameList.SortType.ByDuration, VideoFrameList.SortOrder.Descending);

            // Check if the muber of frames to duplicate is more than the range of frames
            if (numberOfFramesToDuplicate > endFrame - startFrame + 1)
            {
                // We have to duplicate more frames than we have
                // That means we are going to duplicate some frames more than once
                int framesDuppedCounter = 0;
                while (framesDuppedCounter < numberOfFramesToDuplicate)
                {
                    foreach (var vf in list)
                    {
                        videoFrameSection.FramesToDuplicate.Add(vf);
                        framesDuppedCounter++;
                    }
                }
            }
            else
            {
                // We have to duplicate less frames than the ones we have
                // Select the first numberOfFramesToDuplicate frames from the ordered list
                for (int i = 0; i < numberOfFramesToDuplicate; i++)
                {
                    videoFrameSection.FramesToDuplicate.Add(list[i]);
                }
            }

            return numberOfFramesToDuplicate;
        }

        private int DeleteFrames(List<VideoFrameInfo> allFrames, VideoFrameSection videoFrameSection, int startFrame, int endFrame, int numberOfFrameToDelete)
        {
            // Create a temporary list with only the frame range
            List<VideoFrameInfo> list = allFrames.Where(f => f.Number >= startFrame && f.Number <= endFrame).ToList();

            IOrderedEnumerable<VideoFrameInfo> orderedList;

            // if the list is CFR then sort by frame numer, else by frame difference
            if (list.IsCFR())
            {
                // Sort the first cutFrames by frame number
                orderedList = list.OrderBy(f => f.Number);
                //list.Sort(VideoFrameList.SortType.ByFrameNumber, VideoFrameList.SortOrder.Ascending);
            }
            else
            {
                // Sort all frames by difference
                orderedList = list.OrderBy(f => f.DifferencePercentageFromPreviousFrame);
                //list.Sort(VideoFrameList.SortType.ByFrameDifference, VideoFrameList.SortOrder.Ascending);
            }

            // Finally sort all the frames by duration
            orderedList = orderedList.ThenByDescending(f => f.Duration);
            //list.Sort(VideoFrameList.SortType.ByDuration, VideoFrameList.SortOrder.Descending);

            // Apply the order to the list
            list = orderedList.ToList();

            // Check if the muber of frames to delete is more than the range of frames
            if (numberOfFrameToDelete > endFrame - startFrame + 1)
            {
                // We have to delete more frames than we have
                // That means we are going to delete all the frames from the list
                // which are going to be less than the numberOfFrameToDelete
                foreach (var vf in list)
                {
                    videoFrameSection.FramesToDelete.Add(vf);
                }

                return list.Count;
            }
            else
            {
                // We have to delete less frames than the ones we have
                // Select the first numberOfFrameToDelete frames from the ordered list
                for (int i = 0; i < numberOfFrameToDelete; i++)
                {
                    videoFrameSection.FramesToDelete.Add(list[i]);
                }

                return numberOfFrameToDelete;
            }
        }

        private string KienzanString(VideoFrameSection videoFrameSection)
        {
            // Create the string builder
            StringBuilder sb = new StringBuilder();

            // Write the comment line for the section
            sb.AppendFormat("#{0} FramesDeleted : {1} FramesDuplicated : {2}", videoFrameSection.Name,
                videoFrameSection.FramesToDelete.Count, videoFrameSection.FramesToDuplicate.Count);
            sb.AppendLine();

            // Write the first trim 
            sb.AppendFormat("{0} = trim(0, {1})", videoFrameSection.Name, videoFrameSection.EndFrameNumber);
            sb.AppendLine();

            // Write the delete frames (if any)
            if (videoFrameSection.FramesToDelete.Any())
            {
                // Ensure sorted list by frame number 
                // Descending sorting order to avoid clumsy remapping later
                var framesToDeleteOrdered = videoFrameSection.FramesToDelete.OrderByDescending(f => f.Number);

                // Create counter
                int numberOfFramesDeleted = 0;

                // Write filter first
                sb.AppendFormat("{0} = DeleteFrame({0}", videoFrameSection.Name);

                foreach (var vf in framesToDeleteOrdered)
                {
                    // Check if we already appended 900 frames (AviSynth limitation)
                    if (numberOfFramesDeleted == 900)
                    {
                        // Close the previous filter
                        sb.AppendLine(")");

                        // Begin a new one
                        sb.AppendFormat("{0} = DeleteFrame({0}", videoFrameSection.Name);

                        // Write the frame number
                        sb.AppendFormat(", {0}", vf.Number);

                        // Reset the counter
                        numberOfFramesDeleted = 1;
                    }
                    else
                    {
                        // Write the frame number
                        sb.AppendFormat(", {0}", vf.Number);

                        // Increase the counter
                        numberOfFramesDeleted++;
                    }
                }

                // Close the last filter
                sb.AppendLine(")");
            }

            // Write the remapped duplicate frames (if any)
            if (videoFrameSection.FramesToDuplicate.Any())
            {
                // Create new remapped list of duplicate frames
                // First Delete the frames
                // So remap the frames to duplicate
                var remappedFramesToDuplicate = GetRemappedFramesToDuplicate(videoFrameSection);

                // Ensure sorted list by frame number
                // Descending sorting order to avoid clumsy remapping later
                remappedFramesToDuplicate = remappedFramesToDuplicate.OrderByDescending(f => f.Number).ToList();

                // Create counter
                int numberOfFramesDuplicated = 0;

                // Write filter first
                sb.AppendFormat("{0} = DuplicateFrame({0}", videoFrameSection.Name);

                foreach (var vf in remappedFramesToDuplicate)
                {
                    // Check if we already appended 900 frames (AviSynth limitation)
                    if (numberOfFramesDuplicated == 900)
                    {
                        // Close the previous filter
                        sb.AppendLine(")");

                        // Begin a new one
                        sb.AppendFormat("{0} = DuplicateFrame({0}", videoFrameSection.Name);

                        // Write the frame number
                        sb.AppendFormat(", {0}", vf.Number);

                        // Reset the counter
                        numberOfFramesDuplicated = 1;
                    }
                    else
                    {
                        // Write the frame number
                        sb.AppendFormat(", {0}", vf.Number);

                        // Increase the counter
                        numberOfFramesDuplicated++;
                    }
                }

                // Close the last filter
                sb.AppendLine(")");
            }

            // If section starts from the first video frame (framestart = 0) then there is no reason for the final trim
            if (videoFrameSection.StartFrameNumber > 0)
            {
                // Write the final trim
                sb.AppendFormat("{0} = trim({0}, {1}, 0)", videoFrameSection.Name, videoFrameSection.StartFrameNumber);
                sb.AppendLine();
            }

            return sb.ToString();
        }

        private enum VideoFrameProcessType
        {
            Delete,
            Duplicate
        }

        private class VideoFrameInfoWithProcessType
        {
            public VideoFrameInfo VideoFrameInfo { get; set; }

            public VideoFrameProcessType VideoFrameProcessType { get; set; }
        }

        public List<VideoFrameInfo> GetRemappedFramesToDuplicate(VideoFrameSection videoFrameSection)
        {
            // Merge the frame lists
            List<VideoFrameInfoWithProcessType> mergeList =
                videoFrameSection.FramesToDuplicate.Select(
                    f => new VideoFrameInfoWithProcessType
                    { 
                         VideoFrameInfo=f,
                          VideoFrameProcessType = VideoFrameProcessType.Duplicate
                    }
                ).ToList()
                .Concat(
                   videoFrameSection.FramesToDelete.Select(
                       f => new VideoFrameInfoWithProcessType
                       {
                           VideoFrameInfo = f,
                           VideoFrameProcessType = VideoFrameProcessType.Delete
                       }
                    ).ToList()
                ).ToList();

            // Sort the merge list by frame number
            mergeList = mergeList.OrderBy(f => f.VideoFrameInfo.Number).ToList();

            // Create new remapped list of duplicate frames
            List<VideoFrameInfo> remappedFramesToDuplicate = new List<VideoFrameInfo>();

            // Frames 
            int framesDeletedSoFar = 0;

            foreach (var vf in mergeList)
            {
                if (vf.VideoFrameProcessType == VideoFrameProcessType.Delete)
                {
                    // Increase the counter
                    framesDeletedSoFar++;
                    continue;
                }
                else if (vf.VideoFrameProcessType == VideoFrameProcessType.Duplicate)
                {
                    // Remap the frame number
                    // Add it to the remapped list
                    remappedFramesToDuplicate.Add(
                        new VideoFrameInfo
                        {
                            Number = vf.VideoFrameInfo.Number - framesDeletedSoFar,
                            StartTime = vf.VideoFrameInfo.StartTime,
                            Duration = vf.VideoFrameInfo.Duration
                        }
                    );
                }
            }

            return remappedFramesToDuplicate;
        }
    }
}
