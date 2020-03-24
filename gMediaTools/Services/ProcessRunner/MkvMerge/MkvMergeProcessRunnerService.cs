using gMediaTools.Models.ProcessRunner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gMediaTools.Services.ProcessRunner.MkvMerge
{
    public class MkvMergeProcessRunnerService
    {
        public IProcessRunnerParameters GetAllParameters(string mkvMergeFileName, int numberOfInputFiles)
        {
            // Syntax: mkvmerge [global options] {-o out} [options1] {file1} [[options2] {file2}] [@options-file.json] 
            DefaultProcessRunnerParameters all = new DefaultProcessRunnerParameters(mkvMergeFileName, " ");

            // First Group [global options]
            //================================================
            DefaultProcessRunnerParameterGroup globalOptionsGroup = new DefaultProcessRunnerParameterGroup("global options", 1, " ");
            all.ParameterGroups.Add(globalOptionsGroup);

            globalOptionsGroup.Parameters.Add(
                new AllowsEmptyValueProcessRunnerParameter("verbose", "--", " ", false)
            );

            globalOptionsGroup.Parameters.Add(
                new AllowsEmptyValueProcessRunnerParameter("quiet", "--", " ", false)
            );

            globalOptionsGroup.Parameters.Add(
                new AllowsEmptyValueProcessRunnerParameter("webm", "--", " ", false)
            );

            globalOptionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("title", "--", " ", true)
            );

            globalOptionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("default-language", "--", " ", false)
            );

            // Second Group [out]
            //================================================
            DefaultProcessRunnerParameterGroup outGroup = new DefaultProcessRunnerParameterGroup("out", 2, " ");
            all.ParameterGroups.Add(outGroup);

            outGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("output", "--", " ", true)
            );

            // For each input file, add two groups
            for(int i = 0; i < numberOfInputFiles; i++)
            {
                // Third Group [options]
                //================================================
                DefaultProcessRunnerParameterGroup optionsGroup = new DefaultProcessRunnerParameterGroup($"options{i}", 3 + i, " ");
                all.ParameterGroups.Add(optionsGroup);

                optionsGroup.Parameters.Add(
                    new NonEmptyValueProcessRunnerParameter("audio-tracks", "--", " ", false)
                );

                optionsGroup.Parameters.Add(
                    new NonEmptyValueProcessRunnerParameter("video-tracks", "--", " ", false)
                );

                optionsGroup.Parameters.Add(
                    new NonEmptyValueProcessRunnerParameter("subtitle-tracks", "--", " ", false)
                );

                optionsGroup.Parameters.Add(
                    new NonEmptyValueProcessRunnerParameter("button-tracks", "--", " ", false)
                );

                optionsGroup.Parameters.Add(
                    new NonEmptyValueProcessRunnerParameter("track-tags", "--", " ", false)
                );

                optionsGroup.Parameters.Add(
                    new NonEmptyValueProcessRunnerParameter("attachments", "--", " ", false)
                );

                optionsGroup.Parameters.Add(
                    new AllowsEmptyValueProcessRunnerParameter("no-audio", "--", " ", false)
                );

                optionsGroup.Parameters.Add(
                    new AllowsEmptyValueProcessRunnerParameter("no-video", "--", " ", false)
                );

                optionsGroup.Parameters.Add(
                    new AllowsEmptyValueProcessRunnerParameter("no-subtitles", "--", " ", false)
                );

                optionsGroup.Parameters.Add(
                    new AllowsEmptyValueProcessRunnerParameter("no-buttons", "--", " ", false)
                );

                optionsGroup.Parameters.Add(
                    new AllowsEmptyValueProcessRunnerParameter("no-track-tags", "--", " ", false)
                );

                optionsGroup.Parameters.Add(
                    new AllowsEmptyValueProcessRunnerParameter("no-chapters", "--", " ", false)
                );

                optionsGroup.Parameters.Add(
                    new AllowsEmptyValueProcessRunnerParameter("no-attachments", "--", " ", false)
                );

                optionsGroup.Parameters.Add(
                    new AllowsEmptyValueProcessRunnerParameter("no-global-tags", "--", " ", false)
                );

                // Fourth Group [file]
                //================================================
                DefaultProcessRunnerParameterGroup fileGroup = new DefaultProcessRunnerParameterGroup($"file{i}", 4 + i, " ");
                all.ParameterGroups.Add(fileGroup);

                fileGroup.Parameters.Add(
                    new NoNameProcessRunnerParameter("file", true)
                );
            }

            return all;
        }
    }
}
