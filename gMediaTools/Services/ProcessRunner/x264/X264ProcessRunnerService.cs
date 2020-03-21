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
            DefaultProcessRunnerParameters all = new DefaultProcessRunnerParameters(x264FileName, " ");

            // First Group [options]
            //================================================
            DefaultProcessRunnerParameterGroup optionsGroup = new DefaultProcessRunnerParameterGroup("options", 1, " ");

            // Presets
            // =======
            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("profile", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("preset", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("tune", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("slow-firstpass", "--", " ", false)
            );

            // Frame-type options
            // ==================
            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("keyint", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("min-keyint", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new AllowsEmptyValueProcessRunnerParameter("no-scenecut", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("scenecut", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new AllowsEmptyValueProcessRunnerParameter("intra-refresh", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("bframes", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("b-adapt", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("b-bias", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("b-pyramid", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new AllowsEmptyValueProcessRunnerParameter("open-gop", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new AllowsEmptyValueProcessRunnerParameter("no-cabac", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("ref", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new AllowsEmptyValueProcessRunnerParameter("no-deblock", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("deblock", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("slices", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("slices-max", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("slice-max-size", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("slice-max-mbs", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("slice-min-mbs", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new AllowsEmptyValueProcessRunnerParameter("tff", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new AllowsEmptyValueProcessRunnerParameter("bff", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new AllowsEmptyValueProcessRunnerParameter("constrained-intra", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("pulldown", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new AllowsEmptyValueProcessRunnerParameter("fake-interlaced", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("frame-packing", "--", " ", false)
            );

            // Ratecontrol
            // ===========
            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("qp", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("bitrate", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("crf", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("rc-lookahead", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("vbv-maxrate", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("vbv-bufsize", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("vbv-init", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("crf-max", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("qpmin", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("qpmax", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("qpstep", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("ratetol", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("ipratio", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("pbratio", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("chroma-qp-offset", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("aq-mode", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("aq-strength", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("pass", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("stats", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new AllowsEmptyValueProcessRunnerParameter("no-mbtree", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("qcomp", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("cplxblur", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("qblur", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("zones", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("qpfile", "--", " ", false)
            );

            // Analysis
            // ========
            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("partitions", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("direct", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new AllowsEmptyValueProcessRunnerParameter("no-weightb", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("weightp", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("me", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("merange", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("mvrange", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("mvrange-thread", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("subme", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("psy-rd", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new AllowsEmptyValueProcessRunnerParameter("no-psy", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new AllowsEmptyValueProcessRunnerParameter("no-mixed-refs", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new AllowsEmptyValueProcessRunnerParameter("no-chroma-me", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new AllowsEmptyValueProcessRunnerParameter("no-8x8dct", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("trellis", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new AllowsEmptyValueProcessRunnerParameter("no-fast-pskip", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new AllowsEmptyValueProcessRunnerParameter("no-dct-decimate", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("nr", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("deadzone-inter", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("deadzone-intra", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("cqm", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("cqmfile", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("cqm4", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("cqm8", "--", " ", false)
            );

            // Video Usability Info (Annex E)
            // ==============================
            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("overscan", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("videoformat", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("range", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("colorprim", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("transfer", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("colormatrix", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("chromaloc", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("alternative-transfer", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("nal-hrd", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new AllowsEmptyValueProcessRunnerParameter("filler", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new AllowsEmptyValueProcessRunnerParameter("pic-struct", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("crop-rect", "--", " ", false)
            );

            // Input/Output
            // ============
            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("level", "--", " ", false)
            );
            
            optionsGroup.Parameters.Append(
                new AllowsEmptyValueProcessRunnerParameter("bluray-compat", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new AllowsEmptyValueProcessRunnerParameter("stitchable", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new AllowsEmptyValueProcessRunnerParameter("psnr", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new AllowsEmptyValueProcessRunnerParameter("ssim", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("threads", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("lookahead-threads", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new AllowsEmptyValueProcessRunnerParameter("sliced-threads", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new AllowsEmptyValueProcessRunnerParameter("thread-input", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new AllowsEmptyValueProcessRunnerParameter("non-deterministic", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new AllowsEmptyValueProcessRunnerParameter("cpu-independent", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new AllowsEmptyValueProcessRunnerParameter("opencl", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new AllowsEmptyValueProcessRunnerParameter("aud", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("tcfile-in", "--", " ", false)
            );

            optionsGroup.Parameters.Append(
                new NonEmptyValueProcessRunnerParameter("tcfile-out", "--", " ", false)
            );

            // Second group outfile
            //================================================
            DefaultProcessRunnerParameterGroup outFileGroup = new DefaultProcessRunnerParameterGroup("outfile", 2, " ");

            outFileGroup.Parameters.Append(
                new QuotedValueProcessRunnerParameter("o", "-", " ")
            );

            // Third group infile
            //================================================
            DefaultProcessRunnerParameterGroup inFileGroup = new DefaultProcessRunnerParameterGroup("infile", 3, " ");

            inFileGroup.Parameters.Append(
                new QuotedValueProcessRunnerParameter("", "", " ")
            );

            return all;
        }
    }
}
