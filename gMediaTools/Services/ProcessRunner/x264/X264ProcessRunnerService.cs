using gMediaTools.Models.ProcessRunner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gMediaTools.Services.ProcessRunner.x264
{
    public class X264ProcessRunnerService
    {         
        public IProcessRunnerParameters GetAllParameters(string x264FileName)
        {
            // Syntax: x264 [options] -o outfile infile
            DefaultProcessRunnerParameters all = new DefaultProcessRunnerParameters(x264FileName, " ", false);

            // First Group [options]
            //================================================
            DefaultProcessRunnerParameterGroup optionsGroup = new DefaultProcessRunnerParameterGroup("options", 1, " ");
            all.ParameterGroups.Add(optionsGroup);

            // Presets
            // =======
            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("profile", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("preset", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("tune", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("slow-firstpass", "--", " ", false)
            );

            // Frame-type options
            // ==================
            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("keyint", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("min-keyint", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new AllowsEmptyValueProcessRunnerParameter("no-scenecut", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("scenecut", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new AllowsEmptyValueProcessRunnerParameter("intra-refresh", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("bframes", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("b-adapt", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("b-bias", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("b-pyramid", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new AllowsEmptyValueProcessRunnerParameter("open-gop", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new AllowsEmptyValueProcessRunnerParameter("no-cabac", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("ref", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new AllowsEmptyValueProcessRunnerParameter("no-deblock", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("deblock", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("slices", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("slices-max", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("slice-max-size", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("slice-max-mbs", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("slice-min-mbs", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new AllowsEmptyValueProcessRunnerParameter("tff", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new AllowsEmptyValueProcessRunnerParameter("bff", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new AllowsEmptyValueProcessRunnerParameter("constrained-intra", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("pulldown", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new AllowsEmptyValueProcessRunnerParameter("fake-interlaced", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("frame-packing", "--", " ", false)
            );

            // Ratecontrol
            // ===========
            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("qp", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("bitrate", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("crf", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("rc-lookahead", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("vbv-maxrate", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("vbv-bufsize", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("vbv-init", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("crf-max", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("qpmin", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("qpmax", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("qpstep", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("ratetol", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("ipratio", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("pbratio", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("chroma-qp-offset", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("aq-mode", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("aq-strength", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("pass", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("stats", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new AllowsEmptyValueProcessRunnerParameter("no-mbtree", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("qcomp", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("cplxblur", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("qblur", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("zones", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("qpfile", "--", " ", false)
            );

            // Analysis
            // ========
            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("partitions", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("direct", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new AllowsEmptyValueProcessRunnerParameter("no-weightb", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("weightp", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("me", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("merange", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("mvrange", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("mvrange-thread", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("subme", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("psy-rd", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new AllowsEmptyValueProcessRunnerParameter("no-psy", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new AllowsEmptyValueProcessRunnerParameter("no-mixed-refs", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new AllowsEmptyValueProcessRunnerParameter("no-chroma-me", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new AllowsEmptyValueProcessRunnerParameter("no-8x8dct", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("trellis", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new AllowsEmptyValueProcessRunnerParameter("no-fast-pskip", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new AllowsEmptyValueProcessRunnerParameter("no-dct-decimate", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("nr", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("deadzone-inter", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("deadzone-intra", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("cqm", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("cqmfile", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("cqm4", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("cqm8", "--", " ", false)
            );

            // Video Usability Info (Annex E)
            // ==============================
            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("overscan", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("videoformat", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("range", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("colorprim", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("transfer", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("colormatrix", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("chromaloc", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("alternative-transfer", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("nal-hrd", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new AllowsEmptyValueProcessRunnerParameter("filler", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new AllowsEmptyValueProcessRunnerParameter("pic-struct", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("crop-rect", "--", " ", false)
            );

            // Input/Output
            // ============
            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("muxer", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("level", "--", " ", false)
            );
            
            optionsGroup.Parameters.Add(
                new AllowsEmptyValueProcessRunnerParameter("bluray-compat", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new AllowsEmptyValueProcessRunnerParameter("stitchable", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new AllowsEmptyValueProcessRunnerParameter("psnr", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new AllowsEmptyValueProcessRunnerParameter("ssim", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("threads", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("lookahead-threads", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new AllowsEmptyValueProcessRunnerParameter("sliced-threads", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new AllowsEmptyValueProcessRunnerParameter("thread-input", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new AllowsEmptyValueProcessRunnerParameter("non-deterministic", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new AllowsEmptyValueProcessRunnerParameter("cpu-independent", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new AllowsEmptyValueProcessRunnerParameter("opencl", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new AllowsEmptyValueProcessRunnerParameter("aud", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("tcfile-in", "--", " ", false)
            );

            optionsGroup.Parameters.Add(
                new NonEmptyValueProcessRunnerParameter("tcfile-out", "--", " ", false)
            );

            // Second group outfile
            //================================================
            DefaultProcessRunnerParameterGroup outFileGroup = new DefaultProcessRunnerParameterGroup("outfile", 2, " ");
            all.ParameterGroups.Add(outFileGroup);

            outFileGroup.Parameters.Add(
                new QuotedValueProcessRunnerParameter("output", "--", " ")
            );

            // Third group infile
            //================================================
            DefaultProcessRunnerParameterGroup inFileGroup = new DefaultProcessRunnerParameterGroup("infile", 3, " ");
            all.ParameterGroups.Add(inFileGroup);

            inFileGroup.Parameters.Add(
                new NoNameProcessRunnerParameter("infile", true)
            );

            return all;
        }
    }
}
