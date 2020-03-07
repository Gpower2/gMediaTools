using System;
using System.Reflection;
using System.Text;

namespace gMediaTools.MediaInfo
{
    ///<summary>Contains properties for a VideoTrack </summary>
    public class VideoTrack: IMediaInfoTrack
    {
        private string _Count;
        private string _StreamCount;
        private string _StreamKind;
        private string _StreamKindID;
        private string _StreamOrder;
        private string _StreamSize;
        private string _Inform;
        private string _ID;
        private string _UniqueID;
        private string _Title;
        private string _Codec;
        private string _CodecString;
        private string _CodecInfo;
        private string _CodecUrl;
        private string _CodecID;
        private string _CodecIDInfo;
        private string _BitRate;
        private string _BitRateString;
        private string _BitRateMode;
        private string _EncodedLibrary;
        private string _EncodedLibrarySettings;
        private string _Width;
        private string _Height;
        private string _AspectRatio;
        private string _AspectRatioString;
        private string _PixelAspectRatio;
        private string _PixelAspectRatioString;
        private string _FrameRate;
        private string _FrameRateString;
        private string _FrameRateOriginal;
        private string _FrameRateOriginalString;
        private string _FrameRateMode;
        private string _FrameRateModeString;
        private string _FrameCount;
        private string _BitDepth;
        private string _BitsPixelFrame;
        private string _Delay;
        private string _Duration;
        private string _DurationString;
        private string _DurationString1;
        private string _DurationString2;
        private string _DurationString3;
        private string _Language;
        private string _LanguageString;
        private string _LanguageMore;
        private string _Format;
        private string _FormatInfo;
        private string _FormatUrl;
        private string _FormatVersion;
        private string _FormatProfile;
        private string _FormatSettings;
        private string _FormatSettingsBVOP;
        private string _FormatSettingsBVOPString;
        private string _FormatSettingsQPel;
        private string _FormatSettingsQPelString;
        private string _FormatSettingsGMC;
        private string _FormatSettingsGMCString;
        private string _FormatSettingsMatrix;
        private string _FormatSettingsMatrixString;
        private string _FormatSettingsMatrixData;
        private string _FormatSettingsCABAC;
        private string _FormatSettingsCABACString;
        private string _FormatSettingsRefFrames;
        private string _FormatSettingsRefFramesString;
        private string _FormatSettingsPulldown;
        private string _ScanType;
        private string _ScanTypeString;
        private string _Default;
        private string _DefaultString;
        private string _Forced;
        private string _ForcedString;
        private string _ColorSpace;
        private string _ChromaSubsampling;

        ///<summary> Count of objects available in this stream </summary>
        public string Count
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_Count))
                    _Count = "";
                return _Count;
            }
            set
            {
                _Count = value;
            }
        }

        ///<summary> Count of streams of that kind available </summary>
        public string StreamCount
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_StreamCount))
                    _StreamCount = "";
                return _StreamCount;
            }
            set
            {
                _StreamCount = value;
            }
        }

        ///<summary> Stream name </summary>
        public string StreamKind
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_StreamKind))
                    _StreamKind = "";
                return _StreamKind;
            }
            set
            {
                _StreamKind = value;
            }
        }

        ///<summary> When multiple streams, number of the stream </summary>
        public string StreamKindID
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_StreamKindID))
                    _StreamKindID = "";
                return _StreamKindID;
            }
            set
            {
                _StreamKindID = value;
            }
        }

        ///<summary>Stream order in the file, whatever is the kind of stream (base=0)</summary>
        public string StreamOrder
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_StreamOrder))
                    _StreamOrder = "";
                return _StreamOrder;
            }
            set
            {
                _StreamOrder = value;
            }
        }

        /// <summary>
        /// Streamsize in bytes
        /// </summary>
        public string StreamSize
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_StreamSize))
                    _StreamSize = "";
                return _StreamSize;
            }
            set
            {
                _StreamSize = value;
            }
        }

        ///<summary> Last   Inform   call </summary>
        public string Inform
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_Inform))
                    _Inform = "";
                return _Inform;
            }
            set
            {
                _Inform = value;
            }
        }

        ///<summary> A ID for this stream in this file </summary>
        public string ID
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_ID))
                    _ID = "";
                return _ID;
            }
            set
            {
                _ID = value;
            }
        }

        ///<summary> A unique ID for this stream, should be copied with stream copy </summary>
        public string UniqueID
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_UniqueID))
                    _UniqueID = "";
                return _UniqueID;
            }
            set
            {
                _UniqueID = value;
            }
        }

        ///<summary> Name of the track </summary>
        public string Title
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_Title))
                    _Title = "";
                return _Title;
            }
            set
            {
                _Title = value;
            }
        }

        ///<summary> Codec used </summary>
        public string Codec
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_Codec))
                    _Codec = "";
                return _Codec;
            }
            set
            {
                _Codec = value;
            }
        }
        
        ///<summary> Codec used (text) </summary>
        public string CodecString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_CodecString))
                    _CodecString = "";
                return _CodecString;
            }
            set
            {
                _CodecString = value;
            }
        }

        ///<summary> Info about codec </summary>
        public string CodecInfo
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_CodecInfo))
                    _CodecInfo = "";
                return _CodecInfo;
            }
            set
            {
                _CodecInfo = value;
            }
        }

        ///<summary> Link </summary>
        public string CodecUrl
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_CodecUrl))
                    _CodecUrl = "";
                return _CodecUrl;
            }
            set
            {
                _CodecUrl = value;
            }
        }

        ///<summary> Codec ID used </summary>
        public string CodecID
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_CodecID))
                    _CodecID = "";
                return _CodecID;
            }
            set
            {
                _CodecID = value;
            }
        }

        ///<summary> Info about codec ID </summary>
        public string CodecIDInfo
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_CodecIDInfo))
                    _CodecIDInfo = "";
                return _CodecIDInfo;
            }
            set
            {
                _CodecIDInfo = value;
            }
        }

        ///<summary> Bit rate in bps </summary>
        public string BitRate
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_BitRate))
                    _BitRate = "";
                return _BitRate;
            }
            set
            {
                _BitRate = value;
            }
        }

        ///<summary> Bit rate (with measurement) </summary>
        public string BitRateString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_BitRateString))
                    _BitRateString = "";
                return _BitRateString;
            }
            set
            {
                _BitRateString = value;
            }
        }

        ///<summary> Bit rate mode (VBR, CBR) </summary>
        public string BitRateMode
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_BitRateMode))
                    _BitRateMode = "";
                return _BitRateMode;
            }
            set
            {
                _BitRateMode = value;
            }
        }

        ///<summary> Software used to create the file </summary>
        public string EncodedLibrary
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_EncodedLibrary))
                    _EncodedLibrary = "";
                return _EncodedLibrary;
            }
            set
            {
                _EncodedLibrary = value;
            }
        }

        ///<summary> Parameters used by the software </summary>
        public string EncodedLibrarySettings
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_EncodedLibrarySettings))
                    _EncodedLibrarySettings = "";
                return _EncodedLibrarySettings;
            }
            set
            {
                _EncodedLibrarySettings = value;
            }
        }

        ///<summary> Width </summary>
        public string Width
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_Width))
                    _Width = "";
                return _Width;
            }
            set
            {
                _Width = value;
            }
        }

        ///<summary> Height </summary>
        public string Height
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_Height))
                    _Height = "";
                return _Height;
            }
            set
            {
                _Height = value;
            }
        }

        ///<summary> Aspect ratio </summary>
        public string AspectRatio
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_AspectRatio))
                    _AspectRatio = "";
                return _AspectRatio;
            }
            set
            {
                _AspectRatio = value;
            }
        }

        ///<summary> Aspect ratio </summary>
        public string AspectRatioString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_AspectRatioString))
                    _AspectRatioString = "";
                return _AspectRatioString;
            }
            set
            {
                _AspectRatioString = value;
            }
        }

        ///<summary> Pixel Aspect Ratio </summary>
        public string PixelAspectRatio
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_PixelAspectRatio))
                    _PixelAspectRatio = "";
                return _PixelAspectRatio;
            }
            set
            {
                _PixelAspectRatio = value;
            }
        }

        ///<summary> Pixel Aspect Ratio </summary>
        public string PixelAspectRatioString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_PixelAspectRatioString))
                    _PixelAspectRatioString = "";
                return _PixelAspectRatioString;
            }
            set
            {
                _PixelAspectRatioString = value;
            }
        }

        ///<summary> Frame rate </summary>
        public string FrameRate
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_FrameRate))
                    _FrameRate = "";
                return _FrameRate;
            }
            set
            {
                _FrameRate = value;
            }
        }

        ///<summary> Frame rate </summary>
        public string FrameRateString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_FrameRateString))
                    _FrameRateString = "";
                return _FrameRateString;
            }
            set
            {
                _FrameRateString = value;
            }
        }

        ///<summary> Frame rate original </summary>
        public string FrameRateOriginal
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_FrameRateOriginal))
                    _FrameRateOriginal = "";
                return _FrameRateOriginal;
            }
            set
            {
                _FrameRateOriginal = value;
            }
        }

        ///<summary> Frame rate original</summary>
        public string FrameRateOriginalString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_FrameRateOriginalString))
                    _FrameRateOriginalString = "";
                return _FrameRateOriginalString;
            }
            set
            {
                _FrameRateOriginalString = value;
            }
        }

        ///<summary> Frame rate mode</summary>
        public string FrameRateMode
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_FrameRateMode))
                    _FrameRateMode = "";
                return _FrameRateMode;
            }
            set
            {
                _FrameRateMode = value;
            }
        }

        ///<summary> Frame rate mode</summary>
        public string FrameRateModeString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_FrameRateModeString))
                    _FrameRateModeString = "";
                return _FrameRateModeString;
            }
            set
            {
                _FrameRateModeString = value;
            }
        }

        ///<summary> Frame count </summary>
        public string FrameCount
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_FrameCount))
                    _FrameCount = "";
                return _FrameCount;
            }
            set
            {
                _FrameCount = value;
            }
        }

        ///<summary> example for MP3 : 16 bits </summary>
        public string BitDepth
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_BitDepth))
                    _BitDepth = "";
                return _BitDepth;
            }
            set
            {
                _BitDepth = value;
            }
        }

        ///<summary> bits_(Pixel Frame) (like Gordian Knot) </summary>
        public string BitsPixelFrame
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_BitsPixelFrame))
                    _BitsPixelFrame = "";
                return _BitsPixelFrame;
            }
            set
            {
                _BitsPixelFrame = value;
            }
        }

        ///<summary> Delay fixed in the stream (relative) IN MS </summary>
        public string Delay
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_Delay))
                    _Delay = "";
                return _Delay;
            }
            set
            {
                _Delay = value;
            }
        }

        ///<summary> Duration of the stream </summary>
        public string Duration
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_Duration))
                    _Duration = "";
                return _Duration;
            }
            set
            {
                _Duration = value;
            }
        }

        ///<summary> Duration (formated) </summary>
        public string DurationString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_DurationString))
                    _DurationString = "";
                return _DurationString;
            }
            set
            {
                _DurationString = value;
            }
        }

        ///<summary> Duration in format : HHh MMmn SSs MMMms, XX omited if zero </summary>
        public string DurationString1
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_DurationString1))
                    _DurationString1 = "";
                return _DurationString1;
            }
            set
            {
                _DurationString1 = value;
            }
        }

        ///<summary> Duration in format : XXx YYy only, YYy omited if zero </summary>
        public string DurationString2
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_DurationString2))
                    _DurationString2 = "";
                return _DurationString2;
            }
            set
            {
                _DurationString2 = value;
            }
        }

        ///<summary> Duration in format : HH:MM:SS.MMM </summary>
        public string DurationString3
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_DurationString3))
                    _DurationString3 = "";
                return _DurationString3;
            }
            set
            {
                _DurationString3 = value;
            }
        }

        ///<summary> Language (2 letters) </summary>
        public string Language
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_Language))
                    _Language = "";
                return _Language;
            }
            set
            {
                _Language = value;
            }
        }

        ///<summary> Language (full) </summary>
        public string LanguageString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_LanguageString))
                    _LanguageString = "";
                return _LanguageString;
            }
            set
            {
                _LanguageString = value;
            }
        }

        ///<summary> More info about Language (director s comment...) </summary>
        public string LanguageMore
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_LanguageMore))
                    _LanguageMore = "";
                return _LanguageMore;
            }
            set
            {
                _LanguageMore = value;
            }
        }

        ///<summary> Format </summary>
        public string Format
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_Format))
                    _Format = "";
                return _Format;
            }
            set
            {
                _Format = value;
            }
        }

        ///<summary> Info about Format </summary>
        public string FormatInfo
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_FormatInfo))
                    _FormatInfo = "";
                return _FormatInfo;
            }
            set
            {
                _FormatInfo = value;
            }
        }

        ///<summary> Url about Format </summary>
        public string FormatUrl
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_FormatUrl))
                    _FormatUrl = "";
                return _FormatUrl;
            }
            set
            {
                _FormatUrl = value;
            }
        }

        ///<summary> Version of the Format </summary>
        public string FormatVersion
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_FormatVersion))
                    _FormatVersion = "";
                return _FormatVersion;
            }
            set
            {
                _FormatVersion = value;
            }
        }

        ///<summary> Profile of the Format </summary>
        public string FormatProfile
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_FormatProfile))
                    _FormatProfile = "";
                return _FormatProfile;
            }
            set
            {
                _FormatProfile = value;
            }
        }

        ///<summary> Settings of the Format </summary>
        public string FormatSettings
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_FormatSettings))
                    _FormatSettings = "";
                return _FormatSettings;
            }
            set
            {
                _FormatSettings = value;
            }
        }

        ///<summary> BVOP Info </summary>
        public string FormatSettingsBVOP
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_FormatSettingsBVOP))
                    _FormatSettingsBVOP = "";
                return _FormatSettingsBVOP;
            }
            set
            {
                _FormatSettingsBVOP = value;
            }
        }

        ///<summary> BVOP Info (string format)</summary>
        public string FormatSettingsBVOPString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_FormatSettingsBVOPString))
                    _FormatSettingsBVOPString = "";
                return _FormatSettingsBVOPString;
            }
            set
            {
                _FormatSettingsBVOPString = value;
            }
        }

        ///<summary> Qpel Info </summary>
        public string FormatSettingsQPel
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_FormatSettingsQPel))
                    _FormatSettingsQPel = "";
                return _FormatSettingsQPel;
            }
            set
            {
                _FormatSettingsQPel = value;
            }
        }

        ///<summary> Qpel Info (string format)</summary>
        public string FormatSettingsQPelString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_FormatSettingsQPelString))
                    _FormatSettingsQPelString = "";
                return _FormatSettingsQPelString;
            }
            set
            {
                _FormatSettingsQPelString = value;
            }
        }

        ///<summary> GMC Info </summary>
        public string FormatSettingsGMC
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_FormatSettingsGMC))
                    _FormatSettingsGMC = "";
                return _FormatSettingsGMC;
            }
            set
            {
                _FormatSettingsGMC = value;
            }
        }

        ///<summary> GMC Info (string format)</summary>
        public string FormatSettingsGMCString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_FormatSettingsGMCString))
                    _FormatSettingsGMCString = "";
                return _FormatSettingsGMCString;
            }
            set
            {
                _FormatSettingsGMCString = value;
            }
        }

        ///<summary> Matrix Info </summary>
        public string FormatSettingsMatrix
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_FormatSettingsMatrix))
                    _FormatSettingsMatrix = "";
                return _FormatSettingsMatrix;
            }
            set
            {
                _FormatSettingsMatrix = value;
            }
        }

        ///<summary> Matrix Info (string format)</summary>
        public string FormatSettingsMatrixString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_FormatSettingsMatrixString))
                    _FormatSettingsMatrixString = "";
                return _FormatSettingsMatrixString;
            }
            set
            {
                _FormatSettingsMatrixString = value;
            }
        }

        ///<summary> Matrix Info (data format)</summary>
        public string FormatSettingsMatrixData
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_FormatSettingsMatrixData))
                    _FormatSettingsMatrixData = "";
                return _FormatSettingsMatrixData;
            }
            set
            {
                _FormatSettingsMatrixData = value;
            }
        }

        ///<summary> CABAC Info </summary>
        public string FormatSettingsCABAC
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_FormatSettingsCABAC))
                    _FormatSettingsCABAC = "";
                return _FormatSettingsCABAC;
            }
            set
            {
                _FormatSettingsCABAC = value;
            }
        }

        ///<summary> CABAC Info (string format)</summary>
        public string FormatSettingsCABACString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_FormatSettingsCABACString))
                    _FormatSettingsCABACString = "";
                return _FormatSettingsCABACString;
            }
            set
            {
                _FormatSettingsCABACString = value;
            }
        }

        ///<summary> RefFrames Info </summary>
        public string FormatSettingsRefFrames
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_FormatSettingsRefFrames))
                    _FormatSettingsRefFrames = "";
                return _FormatSettingsRefFrames;
            }
            set
            {
                _FormatSettingsRefFrames = value;
            }
        }

        ///<summary> RefFrames Info (string format)</summary>
        public string FormatSettingsRefFramesString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_FormatSettingsRefFramesString))
                    _FormatSettingsRefFramesString = "";
                return _FormatSettingsRefFramesString;
            }
            set
            {
                _FormatSettingsRefFramesString = value;
            }
        }

        ///<summary> Pulldown Info </summary>
        public string FormatSettingsPulldown
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_FormatSettingsPulldown))
                    _FormatSettingsPulldown = "";
                return _FormatSettingsPulldown;
            }
            set
            {
                _FormatSettingsPulldown = value;
            }
        }

        ///<summary> ScanType Info </summary>
        public string ScanType
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_ScanType))
                    _ScanType = "";
                return _ScanType;
            }
            set
            {
                _ScanType = value;
            }
        }

        ///<summary> ScanType Info (string format)</summary>
        public string ScanTypeString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_ScanTypeString))
                    _ScanTypeString = "";
                return _ScanTypeString;
            }
            set
            {
                _ScanTypeString = value;
            }
        }

        ///<summary> Default Info </summary>
        public string Default
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_Default))
                    _Default = "";
                return _Default;
            }
            set
            {
                _Default = value;
            }
        }

        ///<summary> Default Info (string format)</summary>
        public string DefaultString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_DefaultString))
                    _DefaultString = "";
                return _DefaultString;
            }
            set
            {
                _DefaultString = value;
            }
        }

        ///<summary> Forced Info </summary>
        public string Forced
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_Forced))
                    _Forced = "";
                return _Forced;
            }
            set
            {
                _Forced = value;
            }
        }

        ///<summary> Forced Info (string format)</summary>
        public string ForcedString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_ForcedString))
                    _ForcedString = "";
                return _ForcedString;
            }
            set
            {
                _ForcedString = value;
            }
        }

        public string ColorSpace
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_ColorSpace))
                    _ColorSpace = "";
                return _ColorSpace;
            }
            set
            {
                _ColorSpace = value;
            }
        }

        public string ChromaSubsampling
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_ChromaSubsampling))
                    _ChromaSubsampling = "";
                return _ChromaSubsampling;
            }
            set
            {
                _ChromaSubsampling = value;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(GetType().Name);
            var properties = GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                string propValue = property.GetValue(this, null)?.ToString();
                if (!string.IsNullOrWhiteSpace(propValue))
                {
                    sb.AppendLine($"{property.Name?.Trim()}:{propValue?.Trim()}");
                }
            }

            return sb.ToString();
        }
    }
}
