using System;

namespace gMediaTools.Models.MediaInfo
{
    ///<summary>Contains properties for a AudioTrack </summary>
    public class AudioTrack: IMediaInfoTrack
    {
        private string _Count;
        private string _StreamCount;
        private string _StreamKind;
        private string _StreamKindString;
        private string _StreamKindID;
        private string _StreamKindPos;
        private string _StreamOrder;
        private string _Inform;
        private string _ID;
        private string _IDString;
        private string _UniqueID;
        private string _MenuID;
        private string _MenuIDString;
        private string _Format;
        private string _FormatString;
        private string _FormatInfo;
        private string _FormatUrl;
        private string _FormatVersion;
        private string _FormatProfile;
        private string _FormatSettings;
        private string _FormatSettingsSBR;
        private string _FormatSettingsSBRString;
        private string _FormatSettingsPS;
        private string _FormatSettingsPSString;
        private string _FormatSettingsFloor;
        private string _FormatSettingsFirm;
        private string _FormatSettingsEndianness;
        private string _FormatSettingsSign;
        private string _FormatSettingsLaw;
        private string _FormatSettingsITU;
        private string _MuxingMode;
        private string _CodecID;
        private string _CodecIDInfo;
        private string _CodecIDHint;
        private string _CodecIDUrl;
        private string _CodecIDDescription;
        private string _Duration;
        private string _DurationString;
        private string _DurationString1;
        private string _DurationString2;
        private string _DurationString3;
        private string _BitRate;
        private string _BitRateString;
        private string _BitRateMode;
        private string _BitRateModeString;
        private string _BitRateMinimum;
        private string _BitRateMinimumString;
        private string _BitRateNominal;
        private string _BitRateNominalString;
        private string _BitRateMaximum;
        private string _BitRateMaximumString;
        private string _Channels;
        private string _ChannelsString;
        private string _ChannelPositions;
        private string _ChannelPositionsString2;
        private string _ChannelMode;
        private string _SamplingRate;
        private string _SamplingRateString;
        private string _SamplingCount;
        private string _BitDepth;
        private string _BitDepthString;
        private string _CompressionRatio;
        private string _Delay;
        private string _DelayString;
        private string _DelayString1;
        private string _DelayString2;
        private string _DelayString3;
        private string _VideoDelay;
        private string _VideoDelayString;
        private string _VideoDelayString1;
        private string _VideoDelayString2;
        private string _VideoDelayString3;
        private string _ReplayGainGain;
        private string _ReplayGainGainString;
        private string _ReplayGainPeak;
        private string _StreamSize;
        private string _StreamSizeString;
        private string _StreamSizeString1;
        private string _StreamSizeString2;
        private string _StreamSizeString3;
        private string _StreamSizeString4;
        private string _StreamSizeString5;
        private string _StreamSizeProportion;
        private string _Alignment;
        private string _AlignmentString;
        private string _InterleaveVideoFrames;
        private string _InterleaveDuration;
        private string _InterleaveDurationString;
        private string _InterleavePreload;
        private string _InterleavePreloadString;
        private string _Title;
        private string _EncodedLibrary;
        private string _EncodedLibraryString;
        private string _EncodedLibraryName;
        private string _EncodedLibraryVersion;
        private string _EncodedLibraryDate;
        private string _EncodedLibrarySettings;
        private string _Language;
        private string _LanguageString;
        private string _LanguageMore;
        private string _EncodedDate;
        private string _TaggedDate;
        private string _Encryption;
        private string _Default;
        private string _DefaultString;
        private string _Forced;
        private string _ForcedString;

        ///<summary> Count of objects available in this stream </summary>
        public string Count
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_Count))
                    _Count="";
                return _Count;
            }
            set
            {
                _Count=value;
            }
        }

        ///<summary> Count of streams of that kind available </summary>
        public string StreamCount
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_StreamCount))
                    _StreamCount="";
                return _StreamCount;
            }
            set
            {
                _StreamCount=value;
            }
        }

        ///<summary> Stream name </summary>
        public string StreamKind
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_StreamKind))
                    _StreamKind="";
                return _StreamKind;
            }
            set
            {
                _StreamKind=value;
            }
        }

        ///<summary> Stream name string formated</summary>
        public string StreamKindString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_StreamKindString))
                    _StreamKindString = "";
                return _StreamKindString;
            }
            set
            {
                _StreamKindString = value;
            }
        }

        ///<summary> id of the stream </summary>
        public string StreamKindID
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_StreamKindID))
                    _StreamKindID="";
                return _StreamKindID;
            }
            set
            {
                _StreamKindID=value;
            }
        }

        ///<summary> When multiple streams, number of the stream </summary>
        public string StreamKindPos
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_StreamKindPos))
                    _StreamKindPos = "";
                return _StreamKindPos;
            }
            set
            {
                _StreamKindPos = value;
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

        ///<summary> Last   Inform   call </summary>
        public string Inform
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_Inform))
                    _Inform="";
                return _Inform;
            }
            set
            {
                _Inform=value;
            }
        }

        ///<summary> A ID for the stream </summary>
        public string ID
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_ID))
                    _ID="";
                return _ID;
            }
            set
            {
                _ID=value;
            }
        }

        ///<summary> A ID for the stream  string formated</summary>
        public string IDString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_IDString))
                    _IDString = "";
                return _IDString;
            }
            set
            {
                _IDString = value;
            }
        }

        ///<summary> A unique ID for this stream, should be copied with stream copy </summary>
        public string UniqueID
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_UniqueID))
                    _UniqueID="";
                return _UniqueID;
            }
            set
            {
                _UniqueID=value;
            }
        }

        ///<summary> the Menu ID for this stream</summary>
        public string MenuID
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_MenuID))
                    _MenuID = "";
                return _MenuID;
            }
            set
            {
                _MenuID = value;
            }
        }

        ///<summary> the Menu ID for this stream string formated</summary>
        public string MenuIDString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_MenuIDString))
                    _MenuIDString = "";
                return _MenuIDString;
            }
            set
            {
                _MenuIDString = value;
            }
        }

        ///<summary> the Format used</summary>
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

        ///<summary> the Format used + additional features</summary>
        public string FormatString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_FormatString))
                    _FormatString = "";
                return _FormatString;
            }
            set
            {
                _FormatString = value;
            }
        }

        ///<summary> Info about the Format used</summary>
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

        ///<summary> Webpage of the Format</summary>
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

        ///<summary> the Version of the Format used</summary>
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

        ///<summary> the Profile of the Format used</summary>
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

        ///<summary> the Settings of the Format used</summary>
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

        ///<summary> the SBR flag</summary>
        public string FormatSettingsSBR
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_FormatSettingsSBR))
                    _FormatSettingsSBR = "";
                return _FormatSettingsSBR;
            }
            set
            {
                _FormatSettingsSBR = value;
            }
        }

        ///<summary> the SBR flag set as string</summary>
        public string FormatSettingsSBRString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_FormatSettingsSBRString))
                    _FormatSettingsSBRString = "";
                return _FormatSettingsSBRString;
            }
            set
            {
                _FormatSettingsSBRString = value;
            }
        }

        ///<summary> the PS flag</summary>
        public string FormatSettingsPS
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_FormatSettingsPS))
                    _FormatSettingsPS = "";
                return _FormatSettingsPS;
            }
            set
            {
                _FormatSettingsPS = value;
            }
        }

        ///<summary> the PS flag set as string</summary>
        public string FormatSettingsPSString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_FormatSettingsPSString))
                    _FormatSettingsPSString = "";
                return _FormatSettingsPSString;
            }
            set
            {
                _FormatSettingsPSString = value;
            }
        }

        ///<summary> the Floor used in the stream</summary>
        public string FormatSettingsFloor
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_FormatSettingsFloor))
                    _FormatSettingsFloor = "";
                return _FormatSettingsFloor;
            }
            set
            {
                _FormatSettingsFloor = value;
            }
        }

        ///<summary> the Firm used in the settings</summary>
        public string FormatSettingsFirm
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_FormatSettingsFirm))
                    _FormatSettingsFirm = "";
                return _FormatSettingsFirm;
            }
            set
            {
                _FormatSettingsFirm = value;
            }
        }

        ///<summary> the Endianness used in the stream</summary>
        public string FormatSettingsEndianness
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_FormatSettingsEndianness))
                    _FormatSettingsEndianness = "";
                return _FormatSettingsEndianness;
            }
            set
            {
                _FormatSettingsEndianness = value;
            }
        }

        ///<summary> the Sign used in the stream</summary>
        public string FormatSettingsSign
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_FormatSettingsSign))
                    _FormatSettingsSign = "";
                return _FormatSettingsSign;
            }
            set
            {
                _FormatSettingsSign = value;
            }
        }

        ///<summary> the Law used in the stream</summary>
        public string FormatSettingsLaw
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_FormatSettingsLaw))
                    _FormatSettingsLaw = "";
                return _FormatSettingsLaw;
            }
            set
            {
                _FormatSettingsLaw = value;
            }
        }

        ///<summary> the ITU Format used in the stream</summary>
        public string FormatSettingsITU
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_FormatSettingsITU))
                    _FormatSettingsITU = "";
                return _FormatSettingsITU;
            }
            set
            {
                _FormatSettingsITU = value;
            }
        }

        ///<summary> how the stream has been muxed </summary>
        public string MuxingMode
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_MuxingMode))
                    _MuxingMode = "";
                return _MuxingMode;
            }
            set
            {
                _MuxingMode = value;
            }
        }

        ///<summary> the ID of the Codec, found in the container </summary>
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

        ///<summary> Info about the CodecID </summary>
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

        ///<summary> the Hint/popular name for the CodecID  </summary>
        public string CodecIDHint
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_CodecIDHint))
                    _CodecIDHint = "";
                return _CodecIDHint;
            }
            set
            {
                _CodecIDHint = value;
            }
        }

        ///<summary> homepage for more details about the CodecID </summary>
        public string CodecIDUrl
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_CodecIDUrl))
                    _CodecIDUrl = "";
                return _CodecIDUrl;
            }
            set
            {
                _CodecIDUrl = value;
            }
        }

        ///<summary> the description of the Codec ID </summary>
        public string CodecIDDescription
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_CodecIDDescription))
                    _CodecIDDescription = "";
                return _CodecIDDescription;
            }
            set
            {
                _CodecIDDescription = value;
            }
        }

        ///<summary> Name of the track </summary>
        public string Title
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_Title))
                    _Title="";
                return _Title;
            }
            set
            {
                _Title=value;
            }
        }

        ///<summary> Bit rate in bps </summary>
        public string BitRate
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_BitRate))
                    _BitRate="";
                return _BitRate;
            }
            set
            {
                _BitRate=value;
            }
        }

        ///<summary> Bit rate (with measurement) </summary>
        public string BitRateString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_BitRateString))
                    _BitRateString="";
                return _BitRateString;
            }
            set
            {
                _BitRateString=value;
            }
        }

        ///<summary> Bit rate mode (VBR, CBR) </summary>
        public string BitRateMode
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_BitRateMode))
                    _BitRateMode="";
                return _BitRateMode;
            }
            set
            {
                _BitRateMode=value;
            }
        }

        ///<summary> Bit rate mode (VBR, CBR) formated as string </summary>
        public string BitRateModeString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_BitRateModeString))
                    _BitRateModeString = "";
                return _BitRateModeString;
            }
            set
            {
                _BitRateModeString = value;
            }
        }

        ///<summary> Minimum Bit rate mode </summary>
        public string BitRateMinimum
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_BitRateMinimum))
                    _BitRateMinimum = "";
                return _BitRateMinimum;
            }
            set
            {
                _BitRateMinimum = value;
            }
        }

        ///<summary> Minimum Bit rate mode formated as string </summary>
        public string BitRateMinimumString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_BitRateMinimumString))
                    _BitRateMinimumString = "";
                return _BitRateMinimumString;
            }
            set
            {
                _BitRateMinimumString = value;
            }
        }

        ///<summary> Nominal Bit rate </summary>
        public string BitRateNominal
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_BitRateNominal))
                    _BitRateNominal = "";
                return _BitRateNominal;
            }
            set
            {
                _BitRateNominal = value;
            }
        }

        ///<summary> Nominal Bit rate formated as string </summary>
        public string BitRateNominalString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_BitRateNominalString))
                    _BitRateNominalString = "";
                return _BitRateNominalString;
            }
            set
            {
                _BitRateNominalString = value;
            }
        }

        ///<summary> Max Bit rate </summary>
        public string BitRateMaximum
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_BitRateMaximum))
                    _BitRateMaximum = "";
                return _BitRateMaximum;
            }
            set
            {
                _BitRateMaximum = value;
            }
        }

        ///<summary> Max Bit rate formated as string </summary>
        public string BitRateMaximumString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_BitRateMaximumString))
                    _BitRateMaximumString = "";
                return _BitRateMaximumString;
            }
            set
            {
                _BitRateMaximumString = value;
            }
        }

        ///<summary> Number of channels </summary>
        public string Channels
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_Channels))
                    _Channels="";
                return _Channels;
            }
            set
            {
                _Channels=value;
            }
        }

        ///<summary> Number of channels </summary>
        public string ChannelsString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_ChannelsString))
                    _ChannelsString="";
                return _ChannelsString;
            }
            set
            {
                _ChannelsString=value;
            }
        }

        ///<summary> Positions of channels </summary>
        public string ChannelPositions
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_ChannelPositions))
                    _ChannelPositions = "";
                return _ChannelPositions;
            }
            set
            {
                _ChannelPositions = value;
            }
        }

        ///<summary> Positions of channels (x/y.z format) </summary>
        public string ChannelPositionsString2
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_ChannelPositionsString2))
                    _ChannelPositionsString2 = "";
                return _ChannelPositionsString2;
            }
            set
            {
                _ChannelPositionsString2 = value;
            }
        }

        ///<summary> Channel mode </summary>
        public string ChannelMode
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_ChannelMode))
                    _ChannelMode="";
                return _ChannelMode;
            }
            set
            {
                _ChannelMode=value;
            }
        }

        ///<summary> Sampling rate </summary>
        public string SamplingRate
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_SamplingRate))
                    _SamplingRate="";
                return _SamplingRate;
            }
            set
            {
                _SamplingRate=value;
            }
        }

        ///<summary> in KHz </summary>
        public string SamplingRateString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_SamplingRateString))
                    _SamplingRateString="";
                return _SamplingRateString;
            }
            set
            {
                _SamplingRateString=value;
            }
        }

        ///<summary> Frame count </summary>
        public string SamplingCount
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_SamplingCount))
                    _SamplingCount="";
                return _SamplingCount;
            }
            set
            {
                _SamplingCount=value;
            }
        }

        ///<summary> BitDepth in bits (8, 16, 20, 24) </summary>
        public string BitDepth
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_BitDepth))
                    _BitDepth="";
                return _BitDepth;
            }
            set
            {
                _BitDepth=value;
            }
        }

        ///<summary> BitDepth in bits (8, 16, 20, 24) formated as string </summary>
        public string BitDepthString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_BitDepthString))
                    _BitDepthString = "";
                return _BitDepthString;
            }
            set
            {
                _BitDepthString = value;
            }
        }

        ///<summary> Current Stream Size divided by uncompressed Stream Size </summary>
        public string CompressionRatio
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_CompressionRatio))
                    _CompressionRatio = "";
                return _CompressionRatio;
            }
            set
            {
                _CompressionRatio = value;
            }
        }

        ///<summary> Delay fixed in the stream (relative) </summary>
        public string Delay
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_Delay))
                    _Delay="";
                return _Delay;
            }
            set
            {
                _Delay=value;
            }
        }

        ///<summary> Delay in format : XXx YYy only, YYy omitted if zero </summary>
        public string DelayString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_DelayString))
                    _DelayString = "";
                return _DelayString;
            }
            set
            {
                _DelayString = value;
            }
        }

        ///<summary> Delay in format : HHh MMmn SSs MMMms, XX omited if zero </summary>
        public string DelayString1
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_DelayString1))
                    _DelayString1 = "";
                return _DelayString1;
            }
            set
            {
                _DelayString1 = value;
            }
        }

        ///<summary> Delay in format : XXx YYy only, YYy omitted if zero </summary>
        public string DelayString2
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_DelayString2))
                    _DelayString2 = "";
                return _DelayString2;
            }
            set
            {
                _DelayString2 = value;
            }
        }

        ///<summary> Delay in format : HH:MM:SS.MMM </summary>
        public string DelayString3
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_DelayString3))
                    _DelayString3 = "";
                return _DelayString3;
            }
            set
            {
                _DelayString3 = value;
            }
        }

        ///<summary> Delay (ms) fixed in the stream (absolute / video) </summary>
        public string VideoDelay
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_VideoDelay))
                    _VideoDelay="";
                return _VideoDelay;
            }
            set
            {
                _VideoDelay=value;
            }
        }

        ///<summary> Delay (ms) fixed in the stream (absolute / video) formated as string </summary>
        public string VideoDelayString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_VideoDelayString))
                    _VideoDelayString = "";
                return _VideoDelayString;
            }
            set
            {
                _VideoDelayString = value;
            }
        }

        ///<summary> Delay (ms) fixed in the stream (absolute / video) formated as string </summary>
        public string VideoDelayString1
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_VideoDelayString1))
                    _VideoDelayString1 = "";
                return _VideoDelayString1;
            }
            set
            {
                _VideoDelayString1 = value;
            }
        }

        ///<summary> Delay (ms) fixed in the stream (absolute / video) formated as string </summary>
        public string VideoDelayString2
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_VideoDelayString2))
                    _VideoDelayString2 = "";
                return _VideoDelayString2;
            }
            set
            {
                _VideoDelayString2 = value;
            }
        }

        ///<summary> Delay (ms) fixed in the stream (absolute / video) formated as string </summary>
        public string VideoDelayString3
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_VideoDelayString3))
                    _VideoDelayString3 = "";
                return _VideoDelayString3;
            }
            set
            {
                _VideoDelayString3 = value;
            }
        }

        ///<summary> Play time of the stream </summary>
        public string Duration
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_Duration))
                    _Duration="";
                return _Duration;
            }
            set
            {
                _Duration = value;
            }
        }

        ///<summary> Play time (formated) </summary>
        public string DurationString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_DurationString))
                    _DurationString="";
                return _DurationString;
            }
            set
            {
                _DurationString=value;
            }
        }

        ///<summary> Play time in format : HHh MMmn SSs MMMms, XX omited if zero </summary>
        public string DurationString1
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_DurationString1))
                    _DurationString1="";
                return _DurationString1;
            }
            set
            {
                _DurationString1 = value;
            }
        }

        ///<summary> Play time in format : XXx YYy only, YYy omited if zero </summary>
        public string DurationString2
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_DurationString2))
                    _DurationString2="";
                return _DurationString2;
            }
            set
            {
                _DurationString2=value;
            }
        }

        ///<summary> Play time in format : HH:MM:SS.MMM </summary>
        public string DurationString3
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_DurationString3))
                    _DurationString3="";
                return _DurationString3;
            }
            set
            {
                _DurationString3=value;
            }
        }

        ///<summary> The gain to apply to reach 89dB SPL on playback </summary>
        public string ReplayGainGain
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_ReplayGainGain))
                    _ReplayGainGain = "";
                return _ReplayGainGain;
            }
            set
            {
                _ReplayGainGain = value;
            }
        }

        ///<summary> The gain to apply to reach 89dB SPL on playback formated as string </summary>
        public string ReplayGainGainString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_ReplayGainGainString))
                    _ReplayGainGainString = "";
                return _ReplayGainGainString;
            }
            set
            {
                _ReplayGainGainString = value;
            }
        }

        ///<summary> The maximum absolute peak value of the item </summary>
        public string ReplayGainPeak
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_ReplayGainPeak))
                    _ReplayGainPeak = "";
                return _ReplayGainPeak;
            }
            set
            {
                _ReplayGainPeak = value;
            }
        }

        ///<summary> Streamsize in bytes </summary>
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

        ///<summary> Streamsize with percentage value </summary>
        public string StreamSizeString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_StreamSizeString))
                    _StreamSizeString = "";
                return _StreamSizeString;
            }
            set
            {
                _StreamSizeString = value;
            }
        }

        ///<summary> Streamsize with percentage value </summary>
        public string StreamSizeString1
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_StreamSizeString1))
                    _StreamSizeString1 = "";
                return _StreamSizeString1;
            }
            set
            {
                _StreamSizeString1 = value;
            }
        }

        ///<summary> Streamsize with percentage value </summary>
        public string StreamSizeString2
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_StreamSizeString2))
                    _StreamSizeString2 = "";
                return _StreamSizeString2;
            }
            set
            {
                _StreamSizeString2 = value;
            }
        }

        ///<summary> Streamsize with percentage value </summary>
        public string StreamSizeString3
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_StreamSizeString3))
                    _StreamSizeString3 = "";
                return _StreamSizeString3;
            }
            set
            {
                _StreamSizeString3 = value;
            }
        }

        ///<summary> Streamsize with percentage value </summary>
        public string StreamSizeString4
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_StreamSizeString4))
                    _StreamSizeString4 = "";
                return _StreamSizeString4;
            }
            set
            {
                _StreamSizeString4 = value;
            }
        }

        ///<summary> Streamsize with percentage value </summary>
        public string StreamSizeString5
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_StreamSizeString5))
                    _StreamSizeString5 = "";
                return _StreamSizeString5;
            }
            set
            {
                _StreamSizeString5 = value;
            }
        }

        ///<summary> Stream size divided by file size </summary>
        public string StreamSizeProportion
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_StreamSizeProportion))
                    _StreamSizeProportion = "";
                return _StreamSizeProportion;
            }
            set
            {
                _StreamSizeProportion = value;
            }
        }

        ///<summary> How this stream file is aligned in the container </summary>
        public string Alignment
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_Alignment))
                    _Alignment = "";
                return _Alignment;
            }
            set
            {
                _Alignment = value;
            }
        }

        ///<summary> Where this stream file is aligned in the container </summary>
        public string AlignmentString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_AlignmentString))
                    _AlignmentString = "";
                return _AlignmentString;
            }
            set
            {
                _AlignmentString = value;
            }
        }

        ///<summary> Between how many video frames the stream is inserted </summary>
        public string InterleaveVideoFrames
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_InterleaveVideoFrames))
                    _InterleaveVideoFrames = "";
                return _InterleaveVideoFrames;
            }
            set
            {
                _InterleaveVideoFrames = value;
            }
        }

        ///<summary> Between how much time (ms) the stream is inserted </summary>
        public string InterleaveDuration
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_InterleaveDuration))
                    _InterleaveDuration = "";
                return _InterleaveDuration;
            }
            set
            {
                _InterleaveDuration = value;
            }
        }

        ///<summary> Between how much time and video frames the stream is inserted (with measurement) </summary>
        public string InterleaveDurationString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_InterleaveDurationString))
                    _InterleaveDurationString = "";
                return _InterleaveDurationString;
            }
            set
            {
                _InterleaveDurationString = value;
            }
        }

        ///<summary> How much time is buffered before the first video frame </summary>
        public string InterleavePreload
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_InterleavePreload))
                    _InterleavePreload = "";
                return _InterleavePreload;
            }
            set
            {
                _InterleavePreload = value;
            }
        }

        ///<summary> How much time is buffered before the first video frame (with measurement) </summary>
        public string InterleavePreloadString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_InterleavePreloadString))
                    _InterleavePreloadString = "";
                return _InterleavePreloadString;
            }
            set
            {
                _InterleavePreloadString = value;
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

        ///<summary> Software used to create the file formated as string </summary>
        public string EncodedLibraryString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_EncodedLibraryString))
                    _EncodedLibraryString = "";
                return _EncodedLibraryString;
            }
            set
            {
                _EncodedLibraryString = value;
            }
        }

        ///<summary> Name of the Software used to create the file </summary>
        public string EncodedLibraryName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_EncodedLibraryName))
                    _EncodedLibraryName = "";
                return _EncodedLibraryName;
            }
            set
            {
                _EncodedLibraryName = value;
            }
        }


        ///<summary> Version of the Software used to create the file </summary>
        public string EncodedLibraryVersion
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_EncodedLibraryVersion))
                    _EncodedLibraryVersion = "";
                return _EncodedLibraryVersion;
            }
            set
            {
                _EncodedLibraryVersion = value;
            }
        }

        ///<summary> Date of the Software used to create the file </summary>
        public string EncodedLibraryDate
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_EncodedLibraryDate))
                    _EncodedLibraryDate = "";
                return _EncodedLibraryDate;
            }
            set
            {
                _EncodedLibraryDate = value;
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

        ///<summary> Language (2 letters) </summary>
        public string Language
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_Language))
                    _Language="";
                return _Language;
            }
            set
            {
                _Language=value;
            }
        }

        ///<summary> Language (full) </summary>
        public string LanguageString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_LanguageString))
                    _LanguageString="";
                return _LanguageString;
            }
            set
            {
                _LanguageString=value;
            }
        }

        ///<summary> More info about Language (director s comment...) </summary>
        public string LanguageMore
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_LanguageMore))
                    _LanguageMore="";
                return _LanguageMore;
            }
            set
            {
                _LanguageMore=value;
            }
        }

        ///<summary> UTC time that the encoding of this item was completed began. </summary>
        public string EncodedDate
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_EncodedDate))
                    _EncodedDate = "";
                return _EncodedDate;
            }
            set
            {
                _EncodedDate = value;
            }
        }

        ///<summary> UTC time that the tags were done for this item. </summary>
        public string TaggedDate
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_TaggedDate))
                    _TaggedDate = "";
                return _TaggedDate;
            }
            set
            {
                _TaggedDate = value;
            }
        }

        ///<summary> encryption string. </summary>
        public string Encryption
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_Encryption))
                    _Encryption = "";
                return _Encryption;
            }
            set
            {
                _Encryption = value;
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
    }
}
