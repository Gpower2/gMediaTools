using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace gMediaTools.AviSynth
{
    public enum AvsAudioSampleType
    {
        FLOAT = 0x10,
        INT16 = 2,
        INT24 = 4,
        INT32 = 8,
        INT8 = 1,
        Unknown = 0
    }

    public enum AvsVideoColorspace
    {
        I420 = -1610612720,
        IYUV = -1610612720,
        RGB24 = 0x50000001,
        RGB32 = 0x50000002,
        Unknown = 0,
        YUY2 = -1610612740,
        YV12 = -1610612728
    }

    public sealed class AviSynthClip : IDisposable
    {
        #region "PInvoke"

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr LoadLibrary(string dllToLoad);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

        [DllImport("kernel32.dll")]
        public static extern bool FreeLibrary(IntPtr hModule);


        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        private delegate int g_avs_init(ref IntPtr avs, string func, string arg, ref AvsWrapperVideoInfo vi, ref AvsVideoColorspace originalColorspace, ref AvsAudioSampleType originalSampleType, string cs);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        private delegate int g_avs_destroy(ref IntPtr avs);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        private delegate int g_avs_get_last_error(IntPtr avs, [MarshalAs(UnmanagedType.LPStr)] StringBuilder sb, int len);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        private delegate int g_avs_get_video_frame(IntPtr avs, IntPtr buf, int stride, int frm);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        private delegate int g_avs_get_audio_frame(IntPtr avs, IntPtr buf, long sampleNo, long sampleCount);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        private delegate int g_avs_get_int_variable(IntPtr avs, string name, ref int val);


        [StructLayout(LayoutKind.Sequential)]
        private struct AvsWrapperVideoInfo
        {
            public int width;
            public int height;
            public int raten;
            public int rated;
            public int aspectn;
            public int aspectd;
            public int interlaced_frame;
            public int top_field_first;
            public int num_frames;
            public AvsVideoColorspace pixel_type;
            public int audio_samples_per_second;
            public AvsAudioSampleType sample_type;
            public int nchannels;
            public int num_audio_frames;
            public long num_audio_samples;
        }

        private IntPtr _gAviSynthWrapperDLL = IntPtr.Zero;
        private string _gAviSynthWrapperDLLPath = null;


        private void SetAviSynthDLL()
        {
            if (_gAviSynthWrapperDLL == IntPtr.Zero)
            {
                if (_gAviSynthWrapperDLLPath == null)
                {
                    // Check if we are in 32bit or 64bit
                    if (Environment.Is64BitProcess)
                    {
                        _gAviSynthWrapperDLLPath = "gAviSynthWrapper_x64.dll";
                    }
                    else
                    {
                        _gAviSynthWrapperDLLPath = "gAviSynthWrapper_x86.dll";
                    }
                }
                _gAviSynthWrapperDLL = LoadLibrary(_gAviSynthWrapperDLLPath);
                if (_gAviSynthWrapperDLL == IntPtr.Zero)
                {
                    throw new Exception($"Could not load {_gAviSynthWrapperDLLPath}!");
                }
            }
        }

        private T GetFunctionDelegate<T>() where T : Delegate
        {
            IntPtr pAddressOfFunctionToCall = GetProcAddress(_gAviSynthWrapperDLL, typeof(T).Name);

            if (pAddressOfFunctionToCall == IntPtr.Zero)
            {
                throw new Exception(String.Format("Could not load function {0} from {1}!", typeof(T).Name, _gAviSynthWrapperDLLPath));
            }

            Delegate method = Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(T));

            return (T)method;
        }

        private int AvsInit(ref IntPtr avs, string func, string arg, ref AvsWrapperVideoInfo vi, ref AvsVideoColorspace originalColorspace, ref AvsAudioSampleType originalSampleType, string cs)
        {
            return GetFunctionDelegate<g_avs_init>()(ref avs, func, arg, ref vi, ref originalColorspace, ref originalSampleType, cs);
        }

        private int AvsDestroy(ref IntPtr avs)
        {
            return GetFunctionDelegate<g_avs_destroy>()(ref avs);
        }

        private int AvsGetLastError(IntPtr avs, [MarshalAs(UnmanagedType.LPStr)] StringBuilder sb, int len)
        {
            return GetFunctionDelegate<g_avs_get_last_error>()(avs, sb, len);
        }

        private int AvsGetVideoFrame(IntPtr avs, IntPtr buf, int stride, int frm)
        {
            return GetFunctionDelegate<g_avs_get_video_frame>()(avs, buf, stride, frm);
        }

        private int AvsGetAudioSamples(IntPtr avs, IntPtr buf, long sampleNo, long sampleCount)
        {
            return GetFunctionDelegate<g_avs_get_audio_frame>()(avs, buf, sampleNo, sampleCount);
        }

        private int AvsGetIntVariable(IntPtr avs, string name, ref int val)
        {
            return GetFunctionDelegate<g_avs_get_int_variable>()(avs, name, ref val);
        }

        #endregion

        #region "Properties"

        public bool HasVideo
        {
            get
            {
                return ((VideoWidth > 0) && (VideoHeight > 0));
            }
        }

        public int VideoWidth
        {
            get
            {
                return _videoInfoStruct.width;
            }
        }

        public int VideoHeight
        {
            get
            {
                return _videoInfoStruct.height;
            }
        }

        public int VideoFramesCount
        {
            get
            {
                return _videoInfoStruct.num_frames;
            }
        }

        public Double VideoFrameRate
        {
            get
            {
                return Convert.ToDouble(_videoInfoStruct.raten) / Convert.ToDouble(_videoInfoStruct.rated);
            }
        }

        public int VideoFrameRateDenominator
        {
            get
            {
                return _videoInfoStruct.rated;
            }
        }

        public int VideoFrameRateNumerator
        {
            get
            {
                return _videoInfoStruct.raten;
            }
        }

        public int VideoAspectDenominator
        {
            get
            {
                return _videoInfoStruct.aspectd;
            }
        }

        public int VideoAspectNumerator
        {
            get
            {
                return _videoInfoStruct.aspectn;
            }
        }

        public int VideoHasInterlacedFrames
        {
            get
            {
                return _videoInfoStruct.interlaced_frame;
            }
        }

        public int VideoHasTopFieldFirst
        {
            get
            {
                return _videoInfoStruct.top_field_first;
            }
        }

        public AvsVideoColorspace OriginalVideoColorspace
        {
            get
            {
                return _videoColorSpace;
            }
        }

        public AvsVideoColorspace VideoPixelType
        {
            get
            {
                return _videoInfoStruct.pixel_type;
            }
        }

        public short AudioChannelsCount
        {
            get
            {
                return (short)_videoInfoStruct.nchannels;
            }
        }

        public int AudioSampleRate
        {
            get
            {
                return _videoInfoStruct.audio_samples_per_second;
            }
        }

        public long AudioSizeInBytes
        {
            get
            {
                return ((AudioSamplesCount * AudioChannelsCount) * AudioBytesPerSample);
            }
        }

        public int AudioAvgBytesPerSec
        {
            get
            {
                return ((AudioSampleRate * AudioChannelsCount) * AudioBytesPerSample);
            }
        }

        public short AudioBitsPerSample
        {
            get
            {
                return (short)(AudioBytesPerSample * 8);
            }
        }

        public short AudioBytesPerSample
        {
            get
            {
                switch (AudioSampleType)
                {
                    case AvsAudioSampleType.INT8:
                        return 1;

                    case AvsAudioSampleType.INT16:
                        return 2;

                    case AvsAudioSampleType.INT24:
                        return 3;

                    case AvsAudioSampleType.INT32:
                        return 4;

                    case AvsAudioSampleType.FLOAT:
                        return 4;
                }
                throw new ArgumentException(AudioSampleType.ToString());
            }
        }

        public AvsAudioSampleType OriginalAudioSampleType
        {
            get
            {
                return _audioSampleType;
            }
        }

        public long AudioSamplesCount
        {
            get
            {
                return _videoInfoStruct.num_audio_samples;
            }
        }

        public AvsAudioSampleType AudioSampleType
        {
            get
            {
                return _videoInfoStruct.sample_type;
            }
        }

        #endregion

        // Fields
        private IntPtr _avsWrapper = IntPtr.Zero;

        private readonly AvsVideoColorspace _videoColorSpace = AvsVideoColorspace.Unknown;
        private readonly AvsAudioSampleType _audioSampleType = AvsAudioSampleType.Unknown;
        private readonly AvsWrapperVideoInfo _videoInfoStruct = new AvsWrapperVideoInfo();

        public AviSynthClip(string func, string arg, AvsVideoColorspace forceColorspace)
        {
            SetAviSynthDLL();

            string colorSpace = forceColorspace.ToString();
            if (forceColorspace == AvsVideoColorspace.Unknown)
            {
                colorSpace = "";
            }
            if (AvsInit(ref _avsWrapper, func, arg, ref _videoInfoStruct, ref _videoColorSpace, ref _audioSampleType, colorSpace) != 0)
            {
                // Get Last AviSynth Error BEFORE disposing the avs
                string errorMessage = GetLastAviSynthError();

                Dispose(false);

                throw new AviSynthException(errorMessage);
            }
        }

        public int GetAviSynthIntVariable(string variableName, int defaultValue)
        {
            int intValue = 0;
            int intResult = AvsGetIntVariable(_avsWrapper, variableName, ref intValue);
            if (intResult < 0)
            {
                throw new AviSynthException(GetLastAviSynthError());
            }
            if (intResult != 0)
            {
                return defaultValue;
            }
            return intValue;
        }

        private string GetLastAviSynthError()
        {
            StringBuilder sb = new StringBuilder(0x400);
            sb.Length = AvsGetLastError(_avsWrapper, sb, 0x400);
            return sb.ToString();
        }

        public void ReadAudioSamples(byte[] buffer, long offset, long count)
        {
            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            try
            {
                ReadAudioSamples(handle.AddrOfPinnedObject(), offset, count);
            }
            finally
            {
                handle.Free();
            }
        }

        public void ReadAudioSamples(IntPtr addr, long offset, long count)
        {
            if (AvsGetAudioSamples(_avsWrapper, addr, offset, count) != 0)
            {
                throw new AviSynthException(GetLastAviSynthError());
            }
        }

        public void ReadVideoFrame(IntPtr addr, int stride, int frame)
        {
            if (AvsGetVideoFrame(_avsWrapper, addr, stride, frame) != 0)
            {
                throw new AviSynthException(GetLastAviSynthError());
            }
        }

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    GC.SuppressFinalize(this);
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.
                AvsDestroy(ref _avsWrapper);

                _avsWrapper = IntPtr.Zero;


                disposedValue = true;
            }
        }

        ~AviSynthClip()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        #endregion
    }
}
