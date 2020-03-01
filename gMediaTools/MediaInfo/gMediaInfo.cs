using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;
using System.Reflection;
using System.Threading;

namespace gMediaTools.MediaInfo
{
    
    public enum MediaInfoStreamKind
    {
        General,
        Video,
        Audio,
        Text,
        Chapters,
        Image
    }

    public enum MediaInfoInfoKind
    {
        Name,
        Text,
        Measure,
        Options,
        NameText,
        MeasureText,
        Info,
        HowTo
    }

    public enum InfoOptions
    {
        ShowInInform,
        Support,
        ShowInSupported,
        TypeOfValue
    }

    public class gMediaInfo : IDisposable
    {
        private List<VideoTrack> _Video;
        private List<GeneralTrack> _General;
        private List<AudioTrack> _Audio;
        private List<TextTrack> _Text;
        private List<ChaptersTrack> _Chapters;
        private Int32 _VideoCount;
        private Int32 _GeneralCount;
        private Int32 _AudioCount;
        private Int32 _TextCount;
        private Int32 _ChaptersCount;
        private string _InfoCustom;
        private string _FileName;

        private IntPtr _Handle = IntPtr.Zero;

        private IntPtr _MediaInfoDLL = IntPtr.Zero;
        private string _MediaInfoDLLPath = null;

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr LoadLibrary(string dllToLoad);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

        [DllImport("kernel32.dll")]
        public static extern bool FreeLibrary(IntPtr hModule);


        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        internal delegate IntPtr MediaInfo_New();

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        internal delegate int MediaInfo_Open(IntPtr Handle, string FileName);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        internal delegate int MediaInfo_Close(IntPtr Handle);


        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        internal delegate int MediaInfo_Count_Get(IntPtr Handle, [MarshalAs(UnmanagedType.U4)] MediaInfoStreamKind StreamKind, int StreamNumber);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        internal delegate void MediaInfo_Delete(IntPtr Handle);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        internal delegate IntPtr MediaInfo_Get(IntPtr Handle, [MarshalAs(UnmanagedType.U4)] MediaInfoStreamKind StreamKind, uint StreamNumber, string Parameter, [MarshalAs(UnmanagedType.U4)] MediaInfoInfoKind KindOfInfo, [MarshalAs(UnmanagedType.U4)] MediaInfoInfoKind KindOfSearch);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        internal delegate string MediaInfo_GetI(IntPtr Handle, [MarshalAs(UnmanagedType.U4)] MediaInfoStreamKind StreamKind, uint StreamNumber, uint Parameter, [MarshalAs(UnmanagedType.U4)] MediaInfoInfoKind KindOfInfo);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        internal delegate string MediaInfo_Inform(IntPtr Handle, uint Reserved);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        internal delegate string MediaInfo_Option(IntPtr Handle, string OptionString, string Value);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        internal delegate int MediaInfo_State_Get(IntPtr Handle);

        public List<VideoTrack> VideoTracks
        {
            get
            {
                return _Video;
            }
        }

        public List<GeneralTrack> GeneralTracks
        {
            get
            {
                return _General;
            }
        }

        public List<AudioTrack> AudioTracks
        {
            get
            {
                return _Audio;
            }
        }

        public List<TextTrack> TextTracks
        {
            get
            {
                return _Text;
            }
        }

        public List<ChaptersTrack> ChapterTracks
        {
            get
            {
                return _Chapters;
            }
        }

        /// <summary>
        /// Lists every available property in every track
        /// </summary>
        /// <returns></returns>
        public string InfoCustom
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_InfoCustom))
                {
                    StringBuilder sb = new StringBuilder();

                    if (General != null && General.Count > 0)
                    {
                        sb.AppendLine("General");
                        sb.AppendLine(ListEveryAvailablePropery<GeneralTrack>(General));
                    }

                    if (Video != null && Video.Count > 0)
                    {
                        sb.AppendLine("Video");
                        sb.AppendLine(ListEveryAvailablePropery<VideoTrack>(Video));
                    }

                    if (Audio != null && Audio.Count > 0)
                    {
                        sb.AppendLine("Audio");
                        sb.AppendLine(ListEveryAvailablePropery<AudioTrack>(Audio));
                    }

                    if (Text != null && Text.Count > 0)
                    {
                        sb.AppendLine("Text");
                        sb.AppendLine(ListEveryAvailablePropery<TextTrack>(Text));
                    }

                    if (Chapters != null && Chapters.Count > 0)
                    {
                        sb.AppendLine("Chapters");
                        sb.AppendLine(ListEveryAvailablePropery<ChaptersTrack>(Chapters));
                    }

                    _InfoCustom = sb.ToString();
                }
                return _InfoCustom;
            }
        }

        private void SetMediaInfoDLL()
        {
            if (_MediaInfoDLL == IntPtr.Zero)
            {
                if (_MediaInfoDLLPath == null)
                {
                    // Check if we are in 32bit or 64bit
                    if (Environment.Is64BitProcess)
                    {
                        _MediaInfoDLLPath = "MediaInfo_x64.dll";
                    }
                    else
                    {
                        _MediaInfoDLLPath = "MediaInfo_x86.dll";
                    }
                }
                _MediaInfoDLL = LoadLibrary(_MediaInfoDLLPath);
                if (_MediaInfoDLL == IntPtr.Zero)
                {
                    throw new Exception(String.Format("Could not load {0}!", _MediaInfoDLLPath));
                }
            }
        }

        private T GetFunctionDelegate<T>() where T : Delegate
        {
            IntPtr pAddressOfFunctionToCall = GetProcAddress(_MediaInfoDLL, typeof(T).Name);

            if (pAddressOfFunctionToCall == IntPtr.Zero)
            {
                throw new Exception(String.Format("Could not load function {0} from {1}!", typeof(T).Name, _MediaInfoDLLPath));
            }

            Delegate method = Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(T));

            return (T)method;
        }

        public gMediaInfo(string argPath)
        {
            if (!File.Exists(argPath))
            {
                throw new FileNotFoundException(string.Format("File {0} does not exist!", argPath));
            }
            _FileName = argPath;

            // Set the appropriate MediaInfo DLL
            SetMediaInfoDLL();

            this._Handle = GetFunctionDelegate<MediaInfo_New>()();

            int res = GetFunctionDelegate<MediaInfo_Open>()(this._Handle, argPath);

            if (res == 0)
            {
                throw new Exception(string.Format("Could not open file {0}!", argPath));
            }

            try
            {
                getStreamCount();
                getAllInfos();
            }
            finally //ensure MediaInfo_Close is called even if something goes wrong 
            {
                GetFunctionDelegate<MediaInfo_Close>()(this._Handle);
            }
        }

        private string GetSpecificMediaInfo(MediaInfoStreamKind KindOfStream, int trackindex, string NameOfParameter)
        {
            IntPtr p = GetFunctionDelegate<MediaInfo_Get>()(this._Handle, KindOfStream, Convert.ToUInt32(trackindex), NameOfParameter, MediaInfoInfoKind.Text, MediaInfoInfoKind.Name);

            string s = Marshal.PtrToStringUni(p);
            return s;
        }

        private void getStreamCount()
        {
            _AudioCount = GetFunctionDelegate<MediaInfo_Count_Get>()(this._Handle, MediaInfoStreamKind.Audio, -1);
            _VideoCount = GetFunctionDelegate<MediaInfo_Count_Get>()(this._Handle, MediaInfoStreamKind.Video, -1);
            _GeneralCount = GetFunctionDelegate<MediaInfo_Count_Get>()(this._Handle, MediaInfoStreamKind.General, -1);
            _TextCount = GetFunctionDelegate<MediaInfo_Count_Get>()(this._Handle, MediaInfoStreamKind.Text, -1);
            _ChaptersCount = GetFunctionDelegate<MediaInfo_Count_Get>()(this._Handle, MediaInfoStreamKind.Chapters, -1);
        }

        private void getAllInfos()
        {
            getVideoInfo();
            getAudioInfo();
            getChaptersInfo();
            getTextInfo();
            getGeneralInfo();
        }

        /// <summary>
        /// Uses reflection to get every property for every track in a tracklist and get its value.
        /// </summary>
        /// <typeparam name="T1">Type of tracklist, for instance List'VideoTrack'</typeparam>
        /// <param name="L">tracklist, for instance Video</param>
        /// <returns>A formatted string listing every property for every track</returns>
        private string ListEveryAvailablePropery<T1>(List<T1> L)
        {            
            StringBuilder sb = new StringBuilder();
            foreach (T1 track in L)
            {
                foreach (PropertyInfo p in track.GetType().GetProperties())
                {
                    string propValue = p.GetValue(track, null).ToString();
                    if (!String.IsNullOrWhiteSpace(propValue))
                    {
                        sb.AppendLine(String.Format("{0}:{1}", p.Name, propValue));
                    }
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Lists media info dll capacities
        /// </summary>
        /// <returns></returns>
        public string Capacities()
        {
            return GetFunctionDelegate<MediaInfo_Option>()(IntPtr.Zero, "Info_Capacities", "");
        }

        /// <summary>
        /// Lists media info parameter list for MediaInfo_Get
        /// </summary>
        /// <returns></returns>
        private string ParameterList()
        {
            return GetFunctionDelegate<MediaInfo_Option>()(IntPtr.Zero, "Info_Parameters", "");
        }

        /// <summary>
        /// Lists all supported codecs
        /// </summary>
        /// <returns></returns>
        public string KnownCodecs()
        {
            return GetFunctionDelegate<MediaInfo_Option>()(IntPtr.Zero, "Info_Codecs", "");
        }

        ///<summary> List of all the General streams available in the file, type GeneralTrack[trackindex] to access a specific track</summary>
        public List<GeneralTrack> General
        {
            get
            {
                if (this._General == null)
                {
                    getGeneralInfo();
                }
                return this._General;
            }
        }

        private void getGeneralInfo()
        {
            if (this._General == null)
            {
                this._General = new List<GeneralTrack>();
                int trackCount = GetFunctionDelegate<MediaInfo_Count_Get>()(this._Handle, MediaInfoStreamKind.General, -1);
                if (trackCount > 0)
                {
                    for (int i = 0; i < trackCount; i++)
                    {
                        GeneralTrack _tracktemp_ = new GeneralTrack();
                        _tracktemp_.Count = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Count");
                        _tracktemp_.StreamCount = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "StreamCount");
                        _tracktemp_.StreamKind = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "StreamKind");
                        _tracktemp_.StreamKindID = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "StreamKindID");
                        _tracktemp_.StreamOrder = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "StreamOrder");
                        _tracktemp_.Inform = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Inform");
                        _tracktemp_.ID = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "ID");
                        _tracktemp_.UniqueID = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "UniqueID");
                        _tracktemp_.GeneralCount = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "GeneralCount");
                        _tracktemp_.VideoCount = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "VideoCount");
                        _tracktemp_.AudioCount = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "AudioCount");
                        _tracktemp_.TextCount = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "TextCount");
                        _tracktemp_.ChaptersCount = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "ChaptersCount");
                        _tracktemp_.ImageCount = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "ImageCount");
                        _tracktemp_.CompleteName = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "CompleteName");
                        _tracktemp_.FolderName = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "FolderName");
                        _tracktemp_.FileName = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "FileName");
                        _tracktemp_.FileExtension = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "FileExtension");
                        _tracktemp_.FileSize = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "FileSize");
                        _tracktemp_.FileSizeString = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "FileSize/String");
                        _tracktemp_.FileSizeString1 = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "FileSize/String1");
                        _tracktemp_.FileSizeString2 = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "FileSize/String2");
                        _tracktemp_.FileSizeString3 = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "FileSize/String3");
                        _tracktemp_.FileSizeString4 = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "FileSize/String4");
                        _tracktemp_.Format = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Format");
                        _tracktemp_.FormatString = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Format/String");
                        _tracktemp_.FormatInfo = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Format/Info");
                        _tracktemp_.FormatUrl = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Format/Url");
                        _tracktemp_.FormatExtensions = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Format/Extensions");
                        _tracktemp_.OveralBitRate = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "OveralBitRate");
                        _tracktemp_.OveralBitRateString = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "OveralBitRate/String");
                        _tracktemp_.PlayTime = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "PlayTime");
                        _tracktemp_.PlayTimeString = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "PlayTime/String");
                        _tracktemp_.PlayTimeString1 = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "PlayTime/String1");
                        _tracktemp_.PlayTimeString2 = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "PlayTime/String2");
                        _tracktemp_.PlayTimeString3 = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "PlayTime/String3");
                        _tracktemp_.Title = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Title");
                        _tracktemp_.TitleMore = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Title/More");
                        _tracktemp_.Domain = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Domain");
                        _tracktemp_.Collection = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Collection");
                        _tracktemp_.CollectionTotalParts = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Collection/Total_Parts");
                        _tracktemp_.Season = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Season");
                        _tracktemp_.Movie = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Movie");
                        _tracktemp_.MovieMore = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Movie/More");
                        _tracktemp_.Album = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Album");
                        _tracktemp_.AlbumTotalParts = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Album/Total_Parts");
                        _tracktemp_.AlbumSort = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Album/Sort");
                        _tracktemp_.Comic = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Comic");
                        _tracktemp_.ComicTotalParts = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Comic/Total_Parts");
                        _tracktemp_.Part = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Part");
                        _tracktemp_.PartTotalParts = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Part/Total_Parts");
                        _tracktemp_.PartPosition = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Part/Position");
                        _tracktemp_.Track = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Track");
                        _tracktemp_.TrackPosition = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Track/Position");
                        _tracktemp_.TrackMore = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Track/More");
                        _tracktemp_.TrackSort = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Track/Sort");
                        _tracktemp_.Chapter = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Chapter");
                        _tracktemp_.SubTrack = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "SubTrack");
                        _tracktemp_.OriginalAlbum = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Original/Album");
                        _tracktemp_.OriginalMovie = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Original/Movie");
                        _tracktemp_.OriginalPart = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Original/Part");
                        _tracktemp_.OriginalTrack = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Original/Track");
                        _tracktemp_.Author = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Author");
                        _tracktemp_.Artist = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Artist");
                        _tracktemp_.PerformerSort = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Performer/Sort");
                        _tracktemp_.OriginalPerformer = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Original/Performer");
                        _tracktemp_.Accompaniment = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Accompaniment");
                        _tracktemp_.MusicianInstrument = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Musician_Instrument");
                        _tracktemp_.Composer = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Composer");
                        _tracktemp_.ComposerNationality = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Composer/Nationality");
                        _tracktemp_.Arranger = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Arranger");
                        _tracktemp_.Lyricist = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Lyricist");
                        _tracktemp_.OriginalLyricist = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Original/Lyricist");
                        _tracktemp_.Conductor = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Conductor");
                        _tracktemp_.Actor = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Actor");
                        _tracktemp_.ActorCharacter = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Actor_Character");
                        _tracktemp_.WrittenBy = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "WrittenBy");
                        _tracktemp_.ScreenplayBy = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "ScreenplayBy");
                        _tracktemp_.Director = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Director");
                        _tracktemp_.AssistantDirector = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "AssistantDirector");
                        _tracktemp_.DirectorOfPhotography = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "DirectorOfPhotography");
                        _tracktemp_.ArtDirector = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "ArtDirector");
                        _tracktemp_.EditedBy = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "EditedBy");
                        _tracktemp_.Producer = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Producer");
                        _tracktemp_.CoProducer = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "CoProducer");
                        _tracktemp_.ExecutiveProducer = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "ExecutiveProducer");
                        _tracktemp_.ProductionDesigner = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "ProductionDesigner");
                        _tracktemp_.CostumeDesigner = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "CostumeDesigner");
                        _tracktemp_.Choregrapher = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Choregrapher");
                        _tracktemp_.SoundEngineer = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "SoundEngineer");
                        _tracktemp_.MasteredBy = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "MasteredBy");
                        _tracktemp_.RemixedBy = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "RemixedBy");
                        _tracktemp_.ProductionStudio = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "ProductionStudio");
                        _tracktemp_.Publisher = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Publisher");
                        _tracktemp_.PublisherURL = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Publisher/URL");
                        _tracktemp_.DistributedBy = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "DistributedBy");
                        _tracktemp_.EncodedBy = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "EncodedBy");
                        _tracktemp_.ThanksTo = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "ThanksTo");
                        _tracktemp_.Technician = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Technician");
                        _tracktemp_.CommissionedBy = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "CommissionedBy");
                        _tracktemp_.EncodedOriginalDistributedBy = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Encoded_Original/DistributedBy");
                        _tracktemp_.RadioStation = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "RadioStation");
                        _tracktemp_.RadioStationOwner = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "RadioStation/Owner");
                        _tracktemp_.RadioStationURL = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "RadioStation/URL");
                        _tracktemp_.ContentType = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "ContentType");
                        _tracktemp_.Subject = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Subject");
                        _tracktemp_.Synopsys = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Synopsys");
                        _tracktemp_.Summary = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Summary");
                        _tracktemp_.Description = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Description");
                        _tracktemp_.Keywords = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Keywords");
                        _tracktemp_.Period = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Period");
                        _tracktemp_.LawRating = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "LawRating");
                        _tracktemp_.IRCA = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "IRCA");
                        _tracktemp_.Language = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Language");
                        _tracktemp_.Medium = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Medium");
                        _tracktemp_.Product = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Product");
                        _tracktemp_.Country = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Country");
                        _tracktemp_.WrittenDate = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Written_Date");
                        _tracktemp_.RecordedDate = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Recorded_Date");
                        _tracktemp_.ReleasedDate = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Released_Date");
                        _tracktemp_.MasteredDate = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Mastered_Date");
                        _tracktemp_.EncodedDate = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Encoded_Date");
                        _tracktemp_.TaggedDate = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Tagged_Date");
                        _tracktemp_.OriginalReleasedDate = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Original/Released_Date");
                        _tracktemp_.OriginalRecordedDate = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Original/Recorded_Date");
                        _tracktemp_.WrittenLocation = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Written_Location");
                        _tracktemp_.RecordedLocation = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Recorded_Location");
                        _tracktemp_.ArchivalLocation = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Archival_Location");
                        _tracktemp_.Genre = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Genre");
                        _tracktemp_.Mood = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Mood");
                        _tracktemp_.Comment = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Comment");
                        _tracktemp_.Rating = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Rating ");
                        _tracktemp_.EncodedApplication = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Encoded_Application");
                        _tracktemp_.EncodedLibrary = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Encoded_Library");
                        _tracktemp_.EncodedLibrarySettings = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Encoded_Library_Settings");
                        _tracktemp_.EncodedOriginal = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Encoded_Original");
                        _tracktemp_.EncodedOriginalUrl = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Encoded_Original/Url");
                        _tracktemp_.Copyright = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Copyright");
                        _tracktemp_.ProducerCopyright = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Producer_Copyright");
                        _tracktemp_.TermsOfUse = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "TermsOfUse");
                        _tracktemp_.CopyrightUrl = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Copyright/Url");
                        _tracktemp_.ISRC = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "ISRC");
                        _tracktemp_.MSDI = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "MSDI");
                        _tracktemp_.ISBN = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "ISBN");
                        _tracktemp_.BarCode = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "BarCode");
                        _tracktemp_.LCCN = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "LCCN");
                        _tracktemp_.CatalogNumber = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "CatalogNumber");
                        _tracktemp_.LabelCode = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "LabelCode");
                        _tracktemp_.Cover = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Cover");
                        _tracktemp_.CoverDatas = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Cover_Datas");
                        _tracktemp_.BPM = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "BPM");
                        _tracktemp_.VideoCodecList = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Video_Codec_List");
                        _tracktemp_.VideoLanguageList = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Video_Language_List");
                        _tracktemp_.AudioCodecList = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Audio_Codec_List");
                        _tracktemp_.AudioLanguageList = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Audio_Language_List");
                        _tracktemp_.TextCodecList = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Text_Codec_List");
                        _tracktemp_.TextLanguageList = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Text_Language_List");
                        _tracktemp_.ChaptersCodecList = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Chapters_Codec_List");
                        _tracktemp_.ChaptersLanguageList = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Chapters_Language_List");
                        _tracktemp_.ImageCodecList = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Image_Codec_List");
                        _tracktemp_.ImageLanguageList = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Image_Language_List");
                        _tracktemp_.Other = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Other");
                        this._General.Add(_tracktemp_);
                    }
                }
            }
        }

        ///<summary> List of all the Video streams available in the file, type VideoTrack[trackindex] to access a specific track</summary>
        public List<VideoTrack> Video
        {
            get
            {
                if (this._Video == null)
                {
                    getVideoInfo();
                }
                return this._Video;
            }
        }

        private void getVideoInfo()
        {
            if (this._Video == null)
            {
                this._Video = new List<VideoTrack>();
                int trackCount = GetFunctionDelegate<MediaInfo_Count_Get>()(this._Handle, MediaInfoStreamKind.Video, -1);
                if (trackCount > 0)
                {
                    for (int i = 0; i < trackCount; i++)
                    {
                        VideoTrack _tracktemp_ = new VideoTrack();
                        _tracktemp_.Count = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Count");
                        _tracktemp_.StreamCount = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "StreamCount");
                        _tracktemp_.StreamKind = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "StreamKind");
                        _tracktemp_.StreamKindID = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "StreamKindID");
                        _tracktemp_.StreamOrder = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "StreamOrder");
                        _tracktemp_.Inform = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Inform");
                        _tracktemp_.ID = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "ID");
                        _tracktemp_.UniqueID = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "UniqueID");
                        _tracktemp_.Title = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Title");
                        _tracktemp_.Codec = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Codec");
                        _tracktemp_.CodecString = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Codec/String");
                        _tracktemp_.CodecInfo = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Codec/Info");
                        _tracktemp_.CodecUrl = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Codec/Url");
                        _tracktemp_.CodecID = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "CodecID");
                        _tracktemp_.CodecIDInfo = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "CodecID/Info");
                        _tracktemp_.BitRate = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "BitRate");
                        _tracktemp_.BitRateString = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "BitRate/String");
                        _tracktemp_.BitRateMode = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "BitRate_Mode");
                        _tracktemp_.EncodedLibrary = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Encoded_Library");
                        _tracktemp_.EncodedLibrarySettings = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Encoded_Library_Settings");
                        _tracktemp_.Width = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Width");
                        _tracktemp_.Height = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Height");
                        _tracktemp_.AspectRatio = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "AspectRatio");
                        _tracktemp_.AspectRatioString = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "AspectRatio/String");
                        _tracktemp_.PixelAspectRatio = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "PixelAspectRatio");
                        _tracktemp_.PixelAspectRatioString = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "PixelAspectRatio/String");
                        _tracktemp_.FrameRate = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "FrameRate");
                        _tracktemp_.FrameRateString = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "FrameRate/String");
                        _tracktemp_.FrameRateOriginal = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "FrameRate_Original");
                        _tracktemp_.FrameRateOriginalString = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "FrameRate_Original/String");
                        _tracktemp_.FrameRateMode = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "FrameRate_Mode");
                        _tracktemp_.FrameRateModeString = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "FrameRate_Mode/String");
                        _tracktemp_.FrameCount = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "FrameCount");
                        _tracktemp_.BitDepth = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "BitDepth");
                        _tracktemp_.BitsPixelFrame = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Bits/(Pixel*Frame)");
                        _tracktemp_.Delay = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Delay");
                        _tracktemp_.Duration = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Duration");
                        _tracktemp_.DurationString = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Duration/String");
                        _tracktemp_.DurationString1 = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Duration/String1");
                        _tracktemp_.DurationString2 = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Duration/String2");
                        _tracktemp_.DurationString3 = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Duration/String3");
                        _tracktemp_.Language = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Language");
                        _tracktemp_.LanguageString = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Language/String");
                        _tracktemp_.LanguageMore = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Language_More");
                        _tracktemp_.Format = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Format");
                        _tracktemp_.FormatInfo = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Format/Info");
                        _tracktemp_.FormatProfile = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Format_Profile");
                        _tracktemp_.FormatSettings = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Format_Settings");
                        _tracktemp_.FormatSettingsBVOP = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Format_Settings_BVOP");
                        _tracktemp_.FormatSettingsBVOPString = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Format_Settings_BVOP/String");
                        _tracktemp_.FormatSettingsCABAC = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Format_Settings_CABAC");
                        _tracktemp_.FormatSettingsCABACString = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Format_Settings_CABAC/String");
                        _tracktemp_.FormatSettingsGMC = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Format_Settings_GMC");
                        _tracktemp_.FormatSettingsGMCString = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Format_Settings_GMAC/String");
                        _tracktemp_.FormatSettingsMatrix = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Format_Settings_Matrix");
                        _tracktemp_.FormatSettingsMatrixData = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Format_Settings_Matrix_Data");
                        _tracktemp_.FormatSettingsMatrixString = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Format_Settings_Matrix/String");
                        _tracktemp_.FormatSettingsPulldown = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Format_Settings_Pulldown");
                        _tracktemp_.FormatSettingsQPel = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Format_Settings_QPel");
                        _tracktemp_.FormatSettingsQPelString = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Format_Settings_QPel/String");
                        _tracktemp_.FormatSettingsRefFrames = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Format_Settings_RefFrames");
                        _tracktemp_.FormatSettingsRefFramesString = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Format_Settings_RefFrames/String");
                        _tracktemp_.ScanType = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "ScanType");
                        _tracktemp_.ScanTypeString = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "ScanType/String");
                        _tracktemp_.FormatUrl = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Format/Url");
                        _tracktemp_.FormatVersion = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Format_Version");
                        _tracktemp_.Default = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Default");
                        _tracktemp_.DefaultString = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Default/String");
                        _tracktemp_.Forced = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Forced");
                        _tracktemp_.ForcedString = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Forced/String");
                        this._Video.Add(_tracktemp_);
                    }
                }
            }
        }

        ///<summary> List of all the Audio streams available in the file, type AudioTrack[trackindex] to access a specific track</summary>
        public List<AudioTrack> Audio
        {
            get
            {
                if (this._Audio == null)
                {
                    getAudioInfo();
                }
                return this._Audio;
            }
        }

        private void getAudioInfo()
        {
            if (this._Audio == null)
            {
                this._Audio = new List<AudioTrack>();
                int trackCount = GetFunctionDelegate<MediaInfo_Count_Get>()(this._Handle, MediaInfoStreamKind.Audio, -1);
                if (trackCount > 0)
                {
                    for (int i = 0; i < trackCount; i++)
                    {
                        AudioTrack _tracktemp_ = new AudioTrack();
                        _tracktemp_.Count = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Count");
                        _tracktemp_.StreamCount = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "StreamCount");
                        _tracktemp_.StreamKind = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "StreamKind");
                        _tracktemp_.StreamKindString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "StreamKind/String");
                        _tracktemp_.StreamKindID = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "StreamKindID");
                        _tracktemp_.StreamKindPos = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "StreamKindPos");
                        _tracktemp_.StreamOrder = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "StreamOrder");
                        _tracktemp_.Inform = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Inform");
                        _tracktemp_.ID = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "ID");
                        _tracktemp_.IDString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "ID/String");
                        _tracktemp_.UniqueID = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "UniqueID");
                        _tracktemp_.MenuID = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "MenuID");
                        _tracktemp_.MenuIDString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "MenuID/String");
                        _tracktemp_.Format = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Format");
                        _tracktemp_.FormatInfo = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Format/Info");
                        _tracktemp_.FormatUrl = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Format/Url");
                        _tracktemp_.FormatVersion = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Format_Version");
                        _tracktemp_.FormatProfile = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Format_Profile");
                        _tracktemp_.FormatSettings = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Format_Settings");
                        _tracktemp_.FormatSettingsSBR = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Format_Settings_SBR");
                        _tracktemp_.FormatSettingsSBRString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Format_Settings_SBR/String");
                        _tracktemp_.FormatSettingsPS = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Format_Settings_PS");
                        _tracktemp_.FormatSettingsPSString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Format_Settings_PS/String");
                        _tracktemp_.FormatSettingsFloor = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Format_Settings_Floor");
                        _tracktemp_.FormatSettingsFirm = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Format_Settings_Firm");
                        _tracktemp_.FormatSettingsEndianness = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Format_Settings_Endianness");
                        _tracktemp_.FormatSettingsSign = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Format_Settings_Sign");
                        _tracktemp_.FormatSettingsLaw = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Format_Settings_Law");
                        _tracktemp_.FormatSettingsITU = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Format_Settings_ITU");
                        _tracktemp_.MuxingMode = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "MuxingMode");
                        _tracktemp_.CodecID = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "CodecID");
                        _tracktemp_.CodecIDInfo = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "CodecID/Info");
                        _tracktemp_.CodecIDUrl = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "CodecID/Url");
                        _tracktemp_.CodecIDHint = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "CodecID/Hint");
                        _tracktemp_.CodecIDDescription = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "CodecID_Description");
                        _tracktemp_.Duration = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Duration");
                        _tracktemp_.DurationString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Duration/String");
                        _tracktemp_.DurationString1 = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Duration/String1");
                        _tracktemp_.DurationString2 = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Duration/String2");
                        _tracktemp_.DurationString3 = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Duration/String3");
                        _tracktemp_.BitRateMode = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "BitRate_Mode");
                        _tracktemp_.BitRateModeString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "BitRate_Mode/String");
                        _tracktemp_.BitRate = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "BitRate");
                        _tracktemp_.BitRateString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "BitRate/String");
                        _tracktemp_.BitRateMinimum = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "BitRate_Minimum");
                        _tracktemp_.BitRateMinimumString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "BitRate_Minimum/String");
                        _tracktemp_.BitRateNominal = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "BitRate_Nominal");
                        _tracktemp_.BitRateNominalString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "BitRate_Nominal/String");
                        _tracktemp_.BitRateMaximum = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "BitRate_Maximum");
                        _tracktemp_.BitRateMaximumString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "BitRate_Maximum/String");
                        _tracktemp_.Channels = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Channel(s)");
                        _tracktemp_.ChannelsString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Channel(s)/String");
                        _tracktemp_.ChannelMode = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "ChannelMode");
                        _tracktemp_.ChannelPositions = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "ChannelPositions");
                        _tracktemp_.ChannelPositionsString2 = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "ChannelPositions/String2");
                        _tracktemp_.SamplingRate = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "SamplingRate");
                        _tracktemp_.SamplingRateString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "SamplingRate/String");
                        _tracktemp_.SamplingCount = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "SamplingCount");
                        _tracktemp_.BitDepth = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "BitDepth");
                        _tracktemp_.BitDepthString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "BitDepth/String");
                        _tracktemp_.CompressionRatio = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "CompressionRatio");
                        _tracktemp_.Delay = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Delay");
                        _tracktemp_.DelayString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Delay/String");
                        _tracktemp_.DelayString1 = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Delay/String1");
                        _tracktemp_.DelayString2 = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Delay/String2");
                        _tracktemp_.DelayString3 = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Delay/String3");
                        _tracktemp_.VideoDelay = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Video_Delay");
                        _tracktemp_.VideoDelayString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Video_Delay/String");
                        _tracktemp_.VideoDelayString1 = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Video_Delay/String1");
                        _tracktemp_.VideoDelayString2 = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Video_Delay/String2");
                        _tracktemp_.VideoDelayString3 = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Video_Delay/String3");
                        _tracktemp_.ReplayGainGain = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "ReplayGain_Gain");
                        _tracktemp_.ReplayGainGainString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "ReplayGain_Gain/String");
                        _tracktemp_.ReplayGainPeak = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "ReplayGain_Peak");
                        _tracktemp_.StreamSize = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "StreamSize");
                        _tracktemp_.StreamSizeString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "StreamSize/String");
                        _tracktemp_.StreamSizeString1 = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "StreamSize/String1");
                        _tracktemp_.StreamSizeString2 = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "StreamSize/String2");
                        _tracktemp_.StreamSizeString3 = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "StreamSize/String3");
                        _tracktemp_.StreamSizeString4 = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "StreamSize/String4");
                        _tracktemp_.StreamSizeString5 = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "StreamSize/String5");
                        _tracktemp_.StreamSizeProportion = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "StreamSize_Proportion");
                        _tracktemp_.Alignment = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Alignment");
                        _tracktemp_.AlignmentString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Alignment/String");
                        _tracktemp_.InterleaveVideoFrames = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Interleave_VideoFrames");
                        _tracktemp_.InterleaveDuration = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Interleave_Duration");
                        _tracktemp_.InterleaveDurationString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Interleave_Duration/String");
                        _tracktemp_.InterleavePreload = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Interleave_Preload");
                        _tracktemp_.InterleavePreloadString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Interleave_Preload/String");
                        _tracktemp_.Title = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Title");
                        _tracktemp_.EncodedLibrary = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Encoded_Library");
                        _tracktemp_.EncodedLibraryString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Encoded_Library/String");
                        _tracktemp_.EncodedLibraryName = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Encoded_Library/Name");
                        _tracktemp_.EncodedLibraryVersion = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Encoded_Library/Version");
                        _tracktemp_.EncodedLibraryDate = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Encoded_Library/Date");
                        _tracktemp_.EncodedLibrarySettings = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Encoded_Library_Settings");
                        _tracktemp_.Language = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Language");
                        _tracktemp_.LanguageString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Language/String");
                        _tracktemp_.LanguageMore = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Language_More");
                        _tracktemp_.EncodedDate = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Encoded_Date");
                        _tracktemp_.TaggedDate = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Tagged_Date");
                        _tracktemp_.Encryption = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Encryption");
                        _tracktemp_.Default = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Default");
                        _tracktemp_.DefaultString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Default/String");
                        _tracktemp_.Forced = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Forced");
                        _tracktemp_.ForcedString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Forced/String");
                        this._Audio.Add(_tracktemp_);
                    }
                }
            }
        }

        ///<summary> List of all the Text streams available in the file, type TextTrack[trackindex] to access a specific track</summary>
        public List<TextTrack> Text
        {
            get
            {
                if (this._Text == null)
                {
                    getTextInfo();
                }
                return this._Text;
            }
        }

        private void getTextInfo()
        {
            if (this._Text == null)
            {
                this._Text = new List<TextTrack>();
                int trackCount = GetFunctionDelegate<MediaInfo_Count_Get>()(this._Handle, MediaInfoStreamKind.Text, -1);
                if (trackCount > 0)
                {
                    for (int i = 0; i < trackCount; i++)
                    {
                        TextTrack _tracktemp_ = new TextTrack();
                        _tracktemp_.Count = GetSpecificMediaInfo(MediaInfoStreamKind.Text, i, "Count");
                        _tracktemp_.StreamCount = GetSpecificMediaInfo(MediaInfoStreamKind.Text, i, "StreamCount");
                        _tracktemp_.StreamKind = GetSpecificMediaInfo(MediaInfoStreamKind.Text, i, "StreamKind");
                        _tracktemp_.StreamKindID = GetSpecificMediaInfo(MediaInfoStreamKind.Text, i, "StreamKindID");
                        _tracktemp_.StreamOrder = GetSpecificMediaInfo(MediaInfoStreamKind.Text, i, "StreamOrder");
                        _tracktemp_.Inform = GetSpecificMediaInfo(MediaInfoStreamKind.Text, i, "Inform");
                        _tracktemp_.ID = GetSpecificMediaInfo(MediaInfoStreamKind.Text, i, "ID");
                        _tracktemp_.UniqueID = GetSpecificMediaInfo(MediaInfoStreamKind.Text, i, "UniqueID");
                        _tracktemp_.Title = GetSpecificMediaInfo(MediaInfoStreamKind.Text, i, "Title");
                        _tracktemp_.Codec = GetSpecificMediaInfo(MediaInfoStreamKind.Text, i, "Codec");
                        _tracktemp_.CodecString = GetSpecificMediaInfo(MediaInfoStreamKind.Text, i, "Codec/String");
                        _tracktemp_.CodecUrl = GetSpecificMediaInfo(MediaInfoStreamKind.Text, i, "Codec/Url");
                        _tracktemp_.Delay = GetSpecificMediaInfo(MediaInfoStreamKind.Text, i, "Delay");
                        _tracktemp_.Video0Delay = GetSpecificMediaInfo(MediaInfoStreamKind.Text, i, "Video0_Delay");
                        _tracktemp_.PlayTime = GetSpecificMediaInfo(MediaInfoStreamKind.Text, i, "PlayTime");
                        _tracktemp_.PlayTimeString = GetSpecificMediaInfo(MediaInfoStreamKind.Text, i, "PlayTime/String");
                        _tracktemp_.PlayTimeString1 = GetSpecificMediaInfo(MediaInfoStreamKind.Text, i, "PlayTime/String1");
                        _tracktemp_.PlayTimeString2 = GetSpecificMediaInfo(MediaInfoStreamKind.Text, i, "PlayTime/String2");
                        _tracktemp_.PlayTimeString3 = GetSpecificMediaInfo(MediaInfoStreamKind.Text, i, "PlayTime/String3");
                        _tracktemp_.Language = GetSpecificMediaInfo(MediaInfoStreamKind.Text, i, "Language");
                        _tracktemp_.LanguageString = GetSpecificMediaInfo(MediaInfoStreamKind.Text, i, "Language/String");
                        _tracktemp_.LanguageMore = GetSpecificMediaInfo(MediaInfoStreamKind.Text, i, "Language_More");
                        _tracktemp_.Default = GetSpecificMediaInfo(MediaInfoStreamKind.Text, i, "Default");
                        _tracktemp_.DefaultString = GetSpecificMediaInfo(MediaInfoStreamKind.Text, i, "Default/String");
                        _tracktemp_.Forced = GetSpecificMediaInfo(MediaInfoStreamKind.Text, i, "Forced");
                        _tracktemp_.ForcedString = GetSpecificMediaInfo(MediaInfoStreamKind.Text, i, "Forced/String");
                        this._Text.Add(_tracktemp_);
                    }
                }
            }
        }

        ///<summary> List of all the Chapters streams available in the file, type ChaptersTrack[trackindex] to access a specific track</summary>
        public List<ChaptersTrack> Chapters
        {
            get
            {
                if (this._Chapters == null)
                {
                    getChaptersInfo();
                }
                return this._Chapters;
            }
        }

        private void getChaptersInfo()
        {
            if (this._Chapters == null)
            {
                this._Chapters = new List<ChaptersTrack>();
                int trackCount = GetFunctionDelegate<MediaInfo_Count_Get>()(this._Handle, MediaInfoStreamKind.Chapters, -1);
                if (trackCount > 0)
                {
                    for (int i = 0; i < trackCount; i++)
                    {
                        ChaptersTrack _tracktemp_ = new ChaptersTrack();
                        _tracktemp_.Count = GetSpecificMediaInfo(MediaInfoStreamKind.Chapters, i, "Count");
                        _tracktemp_.StreamCount = GetSpecificMediaInfo(MediaInfoStreamKind.Chapters, i, "StreamCount");
                        _tracktemp_.StreamKind = GetSpecificMediaInfo(MediaInfoStreamKind.Chapters, i, "StreamKind");
                        _tracktemp_.StreamKindID = GetSpecificMediaInfo(MediaInfoStreamKind.Chapters, i, "StreamKindID");
                        _tracktemp_.StreamOrder = GetSpecificMediaInfo(MediaInfoStreamKind.Chapters, i, "StreamOrder");
                        _tracktemp_.Inform = GetSpecificMediaInfo(MediaInfoStreamKind.Chapters, i, "Inform");
                        _tracktemp_.ID = GetSpecificMediaInfo(MediaInfoStreamKind.Chapters, i, "ID");
                        _tracktemp_.UniqueID = GetSpecificMediaInfo(MediaInfoStreamKind.Chapters, i, "UniqueID");
                        _tracktemp_.Title = GetSpecificMediaInfo(MediaInfoStreamKind.Chapters, i, "Title");
                        _tracktemp_.Codec = GetSpecificMediaInfo(MediaInfoStreamKind.Chapters, i, "Codec");
                        _tracktemp_.CodecString = GetSpecificMediaInfo(MediaInfoStreamKind.Chapters, i, "Codec/String");
                        _tracktemp_.CodecUrl = GetSpecificMediaInfo(MediaInfoStreamKind.Chapters, i, "Codec/Url");
                        _tracktemp_.Total = GetSpecificMediaInfo(MediaInfoStreamKind.Chapters, i, "Total");
                        _tracktemp_.Language = GetSpecificMediaInfo(MediaInfoStreamKind.Chapters, i, "Language");
                        _tracktemp_.LanguageString = GetSpecificMediaInfo(MediaInfoStreamKind.Chapters, i, "Language/String");
                        _tracktemp_.Default = GetSpecificMediaInfo(MediaInfoStreamKind.Chapters, i, "Default");
                        _tracktemp_.DefaultString = GetSpecificMediaInfo(MediaInfoStreamKind.Chapters, i, "Default/String");
                        _tracktemp_.Forced = GetSpecificMediaInfo(MediaInfoStreamKind.Chapters, i, "Forced");
                        _tracktemp_.ForcedString = GetSpecificMediaInfo(MediaInfoStreamKind.Chapters, i, "Forced/String");
                        this._Chapters.Add(_tracktemp_);
                    }
                }
            }
        }


        #region Disposable Pattern

        private bool disposed;

        ~gMediaInfo()
        {
            Dispose(false);
        }

        protected bool IsDisposed
        {
            get { return disposed; }
        }

        protected void CheckDisposed()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(this.ToString());
            }
        }

        /// <summary>Call this one to kill the wrapper, and close his handle to the MediaInfo.dll, you should never need it anyway </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    DisposeManagedResources();
                }
                DisposeUnmanagedResources();
            }
            this.disposed = true;
        }

        protected virtual void DisposeManagedResources()
        {
        }

        protected virtual void DisposeUnmanagedResources()
        {
            GetFunctionDelegate<MediaInfo_Close>()(this._Handle);
            GetFunctionDelegate<MediaInfo_Delete>()(this._Handle);
        }

        #endregion

    }
}
