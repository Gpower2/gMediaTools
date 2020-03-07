using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Diagnostics;

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
        #region "PInvoke"

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

        #endregion

        private IntPtr _Handle = IntPtr.Zero;

        private IntPtr _MediaInfoDLL = IntPtr.Zero;
        private string _MediaInfoDLLPath = null;

        public string Filename { get; }

        private List<GeneralTrack> _GeneralTracks;

        ///<summary> List of all the General streams available in the file, type GeneralTrack[trackindex] to access a specific track</summary>
        public IEnumerable<GeneralTrack> GeneralTracks
        {
            get
            {
                return _GeneralTracks ?? Enumerable.Empty<GeneralTrack>();
            }
        }

        private List<VideoTrack> _VideoTracks;

        ///<summary> List of all the Video streams available in the file, type VideoTrack[trackindex] to access a specific track</summary>
        public IEnumerable<VideoTrack> VideoTracks
        {
            get
            {
                return _VideoTracks ?? Enumerable.Empty<VideoTrack>();
            }
        }

        private List<AudioTrack> _AudioTracks;

        ///<summary> List of all the Audio streams available in the file, type AudioTrack[trackindex] to access a specific track</summary>
        public IEnumerable<AudioTrack> AudioTracks
        {
            get
            {
                return _AudioTracks ?? Enumerable.Empty<AudioTrack>();
            }
        }

        private List<TextTrack> _TextTracks;

        ///<summary> List of all the Text streams available in the file, type TextTrack[trackindex] to access a specific track</summary>
        public IEnumerable<TextTrack> TextTracks
        {
            get
            {
                return _TextTracks ?? Enumerable.Empty<TextTrack>();
            }
        }

        private List<ChaptersTrack> _ChaptersTracks;

        ///<summary> List of all the Chapters streams available in the file, type ChaptersTrack[trackindex] to access a specific track</summary>
        public IEnumerable<ChaptersTrack> ChaptersTracks
        {
            get
            {
                return _ChaptersTracks ?? Enumerable.Empty<ChaptersTrack>();
            }
        }

        private string _InfoCustom;

        /// <summary>
        /// Lists every available property in every track
        /// </summary>
        /// <returns></returns>
        public string InfoCustom
        {
            get
            {
                return GetCustomInfo();
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
                    throw new Exception($"Could not load {_MediaInfoDLLPath}!");
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

        private string GetSpecificMediaInfo(MediaInfoStreamKind streamKind, int trackIndex, string parameterName)
        {
            try
            {
                IntPtr p = GetFunctionDelegate<MediaInfo_Get>()(_Handle, streamKind, Convert.ToUInt32(trackIndex), parameterName, MediaInfoInfoKind.Text, MediaInfoInfoKind.Name);

                return Marshal.PtrToStringUni(p);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
            }
        }

        public gMediaInfo(string argPath)
        {
            if (!File.Exists(argPath))
            {
                throw new FileNotFoundException(string.Format("File {0} does not exist!", argPath));
            }

            // Set filename property
            Filename = argPath;

            // Set the appropriate MediaInfo DLL
            SetMediaInfoDLL();

            _Handle = GetFunctionDelegate<MediaInfo_New>()();

            int res = GetFunctionDelegate<MediaInfo_Open>()(_Handle, argPath);

            if (res == 0)
            {
                throw new Exception(string.Format("Could not open file {0}!", argPath));
            }

            try
            {
                LoadAllTracks();
            }
            finally //ensure MediaInfo_Close is called even if something goes wrong 
            {
                GetFunctionDelegate<MediaInfo_Close>()(_Handle);
            }
        }

        private void LoadAllTracks()
        {
            LoadGeneralTracks();
            LoadVideoTracks();
            LoadAudioTracks();
            LoadTextTracks();
            LoadChapterTracks();
        }

        /// <summary>
        /// Uses reflection to get every property for every track in a tracklist and get its value.
        /// </summary>
        /// <typeparam name="T">Type of tracklist, for instance List'VideoTrack'</typeparam>
        /// <param name="trackList">tracklist, for instance Video</param>
        /// <returns>A formatted string listing every property for every track</returns>
        private string ListEveryAvailablePropery<T>(IEnumerable<T> trackList) where T : IMediaInfoTrack
        {
            StringBuilder sb = new StringBuilder();
            foreach (T track in trackList)
            {
                var properties = track.GetType().GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    string propValue = property.GetValue(track, null)?.ToString();
                    if (!string.IsNullOrWhiteSpace(propValue))
                    {
                        sb.AppendLine($"{property.Name}:{propValue}");
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

        private string GetCustomInfo()
        {
            if (string.IsNullOrWhiteSpace(_InfoCustom))
            {
                StringBuilder sb = new StringBuilder();

                if (GeneralTracks != null && GeneralTracks.Any())
                {
                    sb.AppendLine("General");
                    sb.AppendLine(ListEveryAvailablePropery<GeneralTrack>(GeneralTracks));
                }

                if (VideoTracks != null && VideoTracks.Any())
                {
                    sb.AppendLine("Video");
                    sb.AppendLine(ListEveryAvailablePropery<VideoTrack>(VideoTracks));
                }

                if (AudioTracks != null && AudioTracks.Any())
                {
                    sb.AppendLine("Audio");
                    sb.AppendLine(ListEveryAvailablePropery<AudioTrack>(AudioTracks));
                }

                if (TextTracks != null && TextTracks.Any())
                {
                    sb.AppendLine("Text");
                    sb.AppendLine(ListEveryAvailablePropery<TextTrack>(TextTracks));
                }

                if (ChaptersTracks != null && ChaptersTracks.Any())
                {
                    sb.AppendLine("Chapters");
                    sb.AppendLine(ListEveryAvailablePropery<ChaptersTrack>(ChaptersTracks));
                }

                _InfoCustom = sb.ToString();
            }

            return _InfoCustom;
        }

        private void LoadGeneralTracks()
        {
            if (_GeneralTracks != null)
            {
                return;
            }
            _GeneralTracks = new List<GeneralTrack>();
            int trackCount = GetFunctionDelegate<MediaInfo_Count_Get>()(_Handle, MediaInfoStreamKind.General, -1);
            for (int i = 0; i < trackCount; i++)
            {
                GeneralTrack tmpGeneralTrack = new GeneralTrack();
                tmpGeneralTrack.Count = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Count");
                tmpGeneralTrack.StreamCount = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "StreamCount");
                tmpGeneralTrack.StreamKind = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "StreamKind");
                tmpGeneralTrack.StreamKindID = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "StreamKindID");
                tmpGeneralTrack.StreamOrder = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "StreamOrder");
                tmpGeneralTrack.Inform = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Inform");
                tmpGeneralTrack.ID = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "ID");
                tmpGeneralTrack.UniqueID = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "UniqueID");
                tmpGeneralTrack.GeneralCount = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "GeneralCount");
                tmpGeneralTrack.VideoCount = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "VideoCount");
                tmpGeneralTrack.AudioCount = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "AudioCount");
                tmpGeneralTrack.TextCount = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "TextCount");
                tmpGeneralTrack.ChaptersCount = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "ChaptersCount");
                tmpGeneralTrack.ImageCount = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "ImageCount");
                tmpGeneralTrack.CompleteName = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "CompleteName");
                tmpGeneralTrack.FolderName = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "FolderName");
                tmpGeneralTrack.FileName = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "FileName");
                tmpGeneralTrack.FileExtension = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "FileExtension");
                tmpGeneralTrack.FileSize = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "FileSize");
                tmpGeneralTrack.FileSizeString = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "FileSize/String");
                tmpGeneralTrack.FileSizeString1 = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "FileSize/String1");
                tmpGeneralTrack.FileSizeString2 = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "FileSize/String2");
                tmpGeneralTrack.FileSizeString3 = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "FileSize/String3");
                tmpGeneralTrack.FileSizeString4 = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "FileSize/String4");
                tmpGeneralTrack.Format = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Format");
                tmpGeneralTrack.FormatString = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Format/String");
                tmpGeneralTrack.FormatInfo = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Format/Info");
                tmpGeneralTrack.FormatUrl = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Format/Url");
                tmpGeneralTrack.FormatExtensions = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Format/Extensions");
                tmpGeneralTrack.OveralBitRate = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "OveralBitRate");
                tmpGeneralTrack.OveralBitRateString = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "OveralBitRate/String");
                tmpGeneralTrack.PlayTime = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "PlayTime");
                tmpGeneralTrack.PlayTimeString = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "PlayTime/String");
                tmpGeneralTrack.PlayTimeString1 = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "PlayTime/String1");
                tmpGeneralTrack.PlayTimeString2 = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "PlayTime/String2");
                tmpGeneralTrack.PlayTimeString3 = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "PlayTime/String3");
                tmpGeneralTrack.Title = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Title");
                tmpGeneralTrack.TitleMore = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Title/More");
                tmpGeneralTrack.Domain = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Domain");
                tmpGeneralTrack.Collection = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Collection");
                tmpGeneralTrack.CollectionTotalParts = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Collection/Total_Parts");
                tmpGeneralTrack.Season = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Season");
                tmpGeneralTrack.Movie = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Movie");
                tmpGeneralTrack.MovieMore = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Movie/More");
                tmpGeneralTrack.Album = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Album");
                tmpGeneralTrack.AlbumTotalParts = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Album/Total_Parts");
                tmpGeneralTrack.AlbumSort = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Album/Sort");
                tmpGeneralTrack.Comic = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Comic");
                tmpGeneralTrack.ComicTotalParts = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Comic/Total_Parts");
                tmpGeneralTrack.Part = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Part");
                tmpGeneralTrack.PartTotalParts = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Part/Total_Parts");
                tmpGeneralTrack.PartPosition = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Part/Position");
                tmpGeneralTrack.Track = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Track");
                tmpGeneralTrack.TrackPosition = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Track/Position");
                tmpGeneralTrack.TrackMore = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Track/More");
                tmpGeneralTrack.TrackSort = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Track/Sort");
                tmpGeneralTrack.Chapter = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Chapter");
                tmpGeneralTrack.SubTrack = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "SubTrack");
                tmpGeneralTrack.OriginalAlbum = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Original/Album");
                tmpGeneralTrack.OriginalMovie = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Original/Movie");
                tmpGeneralTrack.OriginalPart = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Original/Part");
                tmpGeneralTrack.OriginalTrack = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Original/Track");
                tmpGeneralTrack.Author = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Author");
                tmpGeneralTrack.Artist = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Artist");
                tmpGeneralTrack.PerformerSort = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Performer/Sort");
                tmpGeneralTrack.OriginalPerformer = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Original/Performer");
                tmpGeneralTrack.Accompaniment = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Accompaniment");
                tmpGeneralTrack.MusicianInstrument = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Musician_Instrument");
                tmpGeneralTrack.Composer = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Composer");
                tmpGeneralTrack.ComposerNationality = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Composer/Nationality");
                tmpGeneralTrack.Arranger = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Arranger");
                tmpGeneralTrack.Lyricist = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Lyricist");
                tmpGeneralTrack.OriginalLyricist = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Original/Lyricist");
                tmpGeneralTrack.Conductor = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Conductor");
                tmpGeneralTrack.Actor = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Actor");
                tmpGeneralTrack.ActorCharacter = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Actor_Character");
                tmpGeneralTrack.WrittenBy = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "WrittenBy");
                tmpGeneralTrack.ScreenplayBy = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "ScreenplayBy");
                tmpGeneralTrack.Director = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Director");
                tmpGeneralTrack.AssistantDirector = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "AssistantDirector");
                tmpGeneralTrack.DirectorOfPhotography = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "DirectorOfPhotography");
                tmpGeneralTrack.ArtDirector = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "ArtDirector");
                tmpGeneralTrack.EditedBy = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "EditedBy");
                tmpGeneralTrack.Producer = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Producer");
                tmpGeneralTrack.CoProducer = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "CoProducer");
                tmpGeneralTrack.ExecutiveProducer = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "ExecutiveProducer");
                tmpGeneralTrack.ProductionDesigner = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "ProductionDesigner");
                tmpGeneralTrack.CostumeDesigner = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "CostumeDesigner");
                tmpGeneralTrack.Choregrapher = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Choregrapher");
                tmpGeneralTrack.SoundEngineer = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "SoundEngineer");
                tmpGeneralTrack.MasteredBy = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "MasteredBy");
                tmpGeneralTrack.RemixedBy = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "RemixedBy");
                tmpGeneralTrack.ProductionStudio = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "ProductionStudio");
                tmpGeneralTrack.Publisher = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Publisher");
                tmpGeneralTrack.PublisherURL = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Publisher/URL");
                tmpGeneralTrack.DistributedBy = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "DistributedBy");
                tmpGeneralTrack.EncodedBy = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "EncodedBy");
                tmpGeneralTrack.ThanksTo = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "ThanksTo");
                tmpGeneralTrack.Technician = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Technician");
                tmpGeneralTrack.CommissionedBy = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "CommissionedBy");
                tmpGeneralTrack.EncodedOriginalDistributedBy = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Encoded_Original/DistributedBy");
                tmpGeneralTrack.RadioStation = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "RadioStation");
                tmpGeneralTrack.RadioStationOwner = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "RadioStation/Owner");
                tmpGeneralTrack.RadioStationURL = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "RadioStation/URL");
                tmpGeneralTrack.ContentType = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "ContentType");
                tmpGeneralTrack.Subject = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Subject");
                tmpGeneralTrack.Synopsys = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Synopsys");
                tmpGeneralTrack.Summary = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Summary");
                tmpGeneralTrack.Description = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Description");
                tmpGeneralTrack.Keywords = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Keywords");
                tmpGeneralTrack.Period = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Period");
                tmpGeneralTrack.LawRating = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "LawRating");
                tmpGeneralTrack.IRCA = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "IRCA");
                tmpGeneralTrack.Language = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Language");
                tmpGeneralTrack.Medium = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Medium");
                tmpGeneralTrack.Product = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Product");
                tmpGeneralTrack.Country = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Country");
                tmpGeneralTrack.WrittenDate = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Written_Date");
                tmpGeneralTrack.RecordedDate = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Recorded_Date");
                tmpGeneralTrack.ReleasedDate = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Released_Date");
                tmpGeneralTrack.MasteredDate = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Mastered_Date");
                tmpGeneralTrack.EncodedDate = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Encoded_Date");
                tmpGeneralTrack.TaggedDate = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Tagged_Date");
                tmpGeneralTrack.OriginalReleasedDate = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Original/Released_Date");
                tmpGeneralTrack.OriginalRecordedDate = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Original/Recorded_Date");
                tmpGeneralTrack.WrittenLocation = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Written_Location");
                tmpGeneralTrack.RecordedLocation = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Recorded_Location");
                tmpGeneralTrack.ArchivalLocation = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Archival_Location");
                tmpGeneralTrack.Genre = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Genre");
                tmpGeneralTrack.Mood = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Mood");
                tmpGeneralTrack.Comment = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Comment");
                tmpGeneralTrack.Rating = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Rating ");
                tmpGeneralTrack.EncodedApplication = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Encoded_Application");
                tmpGeneralTrack.EncodedLibrary = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Encoded_Library");
                tmpGeneralTrack.EncodedLibrarySettings = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Encoded_Library_Settings");
                tmpGeneralTrack.EncodedOriginal = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Encoded_Original");
                tmpGeneralTrack.EncodedOriginalUrl = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Encoded_Original/Url");
                tmpGeneralTrack.Copyright = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Copyright");
                tmpGeneralTrack.ProducerCopyright = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Producer_Copyright");
                tmpGeneralTrack.TermsOfUse = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "TermsOfUse");
                tmpGeneralTrack.CopyrightUrl = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Copyright/Url");
                tmpGeneralTrack.ISRC = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "ISRC");
                tmpGeneralTrack.MSDI = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "MSDI");
                tmpGeneralTrack.ISBN = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "ISBN");
                tmpGeneralTrack.BarCode = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "BarCode");
                tmpGeneralTrack.LCCN = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "LCCN");
                tmpGeneralTrack.CatalogNumber = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "CatalogNumber");
                tmpGeneralTrack.LabelCode = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "LabelCode");
                tmpGeneralTrack.Cover = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Cover");
                tmpGeneralTrack.CoverDatas = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Cover_Datas");
                tmpGeneralTrack.BPM = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "BPM");
                tmpGeneralTrack.VideoCodecList = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Video_Codec_List");
                tmpGeneralTrack.VideoLanguageList = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Video_Language_List");
                tmpGeneralTrack.AudioCodecList = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Audio_Codec_List");
                tmpGeneralTrack.AudioLanguageList = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Audio_Language_List");
                tmpGeneralTrack.TextCodecList = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Text_Codec_List");
                tmpGeneralTrack.TextLanguageList = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Text_Language_List");
                tmpGeneralTrack.ChaptersCodecList = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Chapters_Codec_List");
                tmpGeneralTrack.ChaptersLanguageList = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Chapters_Language_List");
                tmpGeneralTrack.ImageCodecList = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Image_Codec_List");
                tmpGeneralTrack.ImageLanguageList = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Image_Language_List");
                tmpGeneralTrack.Other = GetSpecificMediaInfo(MediaInfoStreamKind.General, i, "Other");

                _GeneralTracks.Add(tmpGeneralTrack);
            }
        }

        private void LoadVideoTracks()
        {
            if (_VideoTracks != null)
            {
                return;
            }
            _VideoTracks = new List<VideoTrack>();
            int trackCount = GetFunctionDelegate<MediaInfo_Count_Get>()(_Handle, MediaInfoStreamKind.Video, -1);
            for (int i = 0; i < trackCount; i++)
            {
                VideoTrack tmpVideoTrack = new VideoTrack();
                tmpVideoTrack.Count = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Count");
                tmpVideoTrack.StreamCount = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "StreamCount");
                tmpVideoTrack.StreamKind = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "StreamKind");
                tmpVideoTrack.StreamKindID = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "StreamKindID");
                tmpVideoTrack.StreamOrder = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "StreamOrder");
                tmpVideoTrack.Inform = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Inform");
                tmpVideoTrack.ID = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "ID");
                tmpVideoTrack.UniqueID = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "UniqueID");
                tmpVideoTrack.Title = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Title");
                tmpVideoTrack.Codec = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Codec");
                tmpVideoTrack.CodecString = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Codec/String");
                tmpVideoTrack.CodecInfo = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Codec/Info");
                tmpVideoTrack.CodecUrl = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Codec/Url");
                tmpVideoTrack.CodecID = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "CodecID");
                tmpVideoTrack.CodecIDInfo = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "CodecID/Info");
                tmpVideoTrack.BitRate = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "BitRate");
                tmpVideoTrack.BitRateString = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "BitRate/String");
                tmpVideoTrack.BitRateMode = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "BitRate_Mode");
                tmpVideoTrack.EncodedLibrary = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Encoded_Library");
                tmpVideoTrack.EncodedLibrarySettings = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Encoded_Library_Settings");
                tmpVideoTrack.Width = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Width");
                tmpVideoTrack.Height = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Height");
                tmpVideoTrack.AspectRatio = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "AspectRatio");
                tmpVideoTrack.AspectRatioString = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "AspectRatio/String");
                tmpVideoTrack.PixelAspectRatio = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "PixelAspectRatio");
                tmpVideoTrack.PixelAspectRatioString = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "PixelAspectRatio/String");
                tmpVideoTrack.FrameRate = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "FrameRate");
                tmpVideoTrack.FrameRateString = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "FrameRate/String");
                tmpVideoTrack.FrameRateOriginal = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "FrameRate_Original");
                tmpVideoTrack.FrameRateOriginalString = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "FrameRate_Original/String");
                tmpVideoTrack.FrameRateMode = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "FrameRate_Mode");
                tmpVideoTrack.FrameRateModeString = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "FrameRate_Mode/String");
                tmpVideoTrack.FrameCount = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "FrameCount");
                tmpVideoTrack.BitDepth = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "BitDepth");
                tmpVideoTrack.BitsPixelFrame = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Bits/(Pixel*Frame)");
                tmpVideoTrack.Delay = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Delay");
                tmpVideoTrack.Duration = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Duration");
                tmpVideoTrack.DurationString = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Duration/String");
                tmpVideoTrack.DurationString1 = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Duration/String1");
                tmpVideoTrack.DurationString2 = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Duration/String2");
                tmpVideoTrack.DurationString3 = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Duration/String3");
                tmpVideoTrack.Language = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Language");
                tmpVideoTrack.LanguageString = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Language/String");
                tmpVideoTrack.LanguageMore = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Language_More");
                tmpVideoTrack.Format = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Format");
                tmpVideoTrack.FormatInfo = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Format/Info");
                tmpVideoTrack.FormatProfile = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Format_Profile");
                tmpVideoTrack.FormatSettings = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Format_Settings");
                tmpVideoTrack.FormatSettingsBVOP = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Format_Settings_BVOP");
                tmpVideoTrack.FormatSettingsBVOPString = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Format_Settings_BVOP/String");
                tmpVideoTrack.FormatSettingsCABAC = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Format_Settings_CABAC");
                tmpVideoTrack.FormatSettingsCABACString = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Format_Settings_CABAC/String");
                tmpVideoTrack.FormatSettingsGMC = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Format_Settings_GMC");
                tmpVideoTrack.FormatSettingsGMCString = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Format_Settings_GMAC/String");
                tmpVideoTrack.FormatSettingsMatrix = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Format_Settings_Matrix");
                tmpVideoTrack.FormatSettingsMatrixData = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Format_Settings_Matrix_Data");
                tmpVideoTrack.FormatSettingsMatrixString = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Format_Settings_Matrix/String");
                tmpVideoTrack.FormatSettingsPulldown = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Format_Settings_Pulldown");
                tmpVideoTrack.FormatSettingsQPel = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Format_Settings_QPel");
                tmpVideoTrack.FormatSettingsQPelString = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Format_Settings_QPel/String");
                tmpVideoTrack.FormatSettingsRefFrames = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Format_Settings_RefFrames");
                tmpVideoTrack.FormatSettingsRefFramesString = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Format_Settings_RefFrames/String");
                tmpVideoTrack.ScanType = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "ScanType");
                tmpVideoTrack.ScanTypeString = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "ScanType/String");
                tmpVideoTrack.FormatUrl = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Format/Url");
                tmpVideoTrack.FormatVersion = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Format_Version");
                tmpVideoTrack.Default = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Default");
                tmpVideoTrack.DefaultString = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Default/String");
                tmpVideoTrack.Forced = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Forced");
                tmpVideoTrack.ForcedString = GetSpecificMediaInfo(MediaInfoStreamKind.Video, i, "Forced/String");

                _VideoTracks.Add(tmpVideoTrack);
            }
        }

        private void LoadAudioTracks()
        {
            if (_AudioTracks != null)
            {
                return;
            }
            _AudioTracks = new List<AudioTrack>();
            int trackCount = GetFunctionDelegate<MediaInfo_Count_Get>()(_Handle, MediaInfoStreamKind.Audio, -1);
            for (int i = 0; i < trackCount; i++)
            {
                AudioTrack tmpAudioTrack = new AudioTrack();
                tmpAudioTrack.Count = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Count");
                tmpAudioTrack.StreamCount = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "StreamCount");
                tmpAudioTrack.StreamKind = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "StreamKind");
                tmpAudioTrack.StreamKindString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "StreamKind/String");
                tmpAudioTrack.StreamKindID = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "StreamKindID");
                tmpAudioTrack.StreamKindPos = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "StreamKindPos");
                tmpAudioTrack.StreamOrder = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "StreamOrder");
                tmpAudioTrack.Inform = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Inform");
                tmpAudioTrack.ID = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "ID");
                tmpAudioTrack.IDString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "ID/String");
                tmpAudioTrack.UniqueID = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "UniqueID");
                tmpAudioTrack.MenuID = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "MenuID");
                tmpAudioTrack.MenuIDString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "MenuID/String");
                tmpAudioTrack.Format = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Format");
                tmpAudioTrack.FormatInfo = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Format/Info");
                tmpAudioTrack.FormatUrl = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Format/Url");
                tmpAudioTrack.FormatVersion = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Format_Version");
                tmpAudioTrack.FormatProfile = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Format_Profile");
                tmpAudioTrack.FormatSettings = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Format_Settings");
                tmpAudioTrack.FormatSettingsSBR = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Format_Settings_SBR");
                tmpAudioTrack.FormatSettingsSBRString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Format_Settings_SBR/String");
                tmpAudioTrack.FormatSettingsPS = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Format_Settings_PS");
                tmpAudioTrack.FormatSettingsPSString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Format_Settings_PS/String");
                tmpAudioTrack.FormatSettingsFloor = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Format_Settings_Floor");
                tmpAudioTrack.FormatSettingsFirm = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Format_Settings_Firm");
                tmpAudioTrack.FormatSettingsEndianness = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Format_Settings_Endianness");
                tmpAudioTrack.FormatSettingsSign = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Format_Settings_Sign");
                tmpAudioTrack.FormatSettingsLaw = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Format_Settings_Law");
                tmpAudioTrack.FormatSettingsITU = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Format_Settings_ITU");
                tmpAudioTrack.MuxingMode = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "MuxingMode");
                tmpAudioTrack.CodecID = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "CodecID");
                tmpAudioTrack.CodecIDInfo = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "CodecID/Info");
                tmpAudioTrack.CodecIDUrl = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "CodecID/Url");
                tmpAudioTrack.CodecIDHint = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "CodecID/Hint");
                tmpAudioTrack.CodecIDDescription = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "CodecID_Description");
                tmpAudioTrack.Duration = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Duration");
                tmpAudioTrack.DurationString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Duration/String");
                tmpAudioTrack.DurationString1 = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Duration/String1");
                tmpAudioTrack.DurationString2 = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Duration/String2");
                tmpAudioTrack.DurationString3 = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Duration/String3");
                tmpAudioTrack.BitRateMode = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "BitRate_Mode");
                tmpAudioTrack.BitRateModeString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "BitRate_Mode/String");
                tmpAudioTrack.BitRate = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "BitRate");
                tmpAudioTrack.BitRateString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "BitRate/String");
                tmpAudioTrack.BitRateMinimum = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "BitRate_Minimum");
                tmpAudioTrack.BitRateMinimumString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "BitRate_Minimum/String");
                tmpAudioTrack.BitRateNominal = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "BitRate_Nominal");
                tmpAudioTrack.BitRateNominalString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "BitRate_Nominal/String");
                tmpAudioTrack.BitRateMaximum = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "BitRate_Maximum");
                tmpAudioTrack.BitRateMaximumString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "BitRate_Maximum/String");
                tmpAudioTrack.Channels = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Channel(s)");
                tmpAudioTrack.ChannelsString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Channel(s)/String");
                tmpAudioTrack.ChannelMode = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "ChannelMode");
                tmpAudioTrack.ChannelPositions = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "ChannelPositions");
                tmpAudioTrack.ChannelPositionsString2 = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "ChannelPositions/String2");
                tmpAudioTrack.SamplingRate = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "SamplingRate");
                tmpAudioTrack.SamplingRateString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "SamplingRate/String");
                tmpAudioTrack.SamplingCount = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "SamplingCount");
                tmpAudioTrack.BitDepth = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "BitDepth");
                tmpAudioTrack.BitDepthString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "BitDepth/String");
                tmpAudioTrack.CompressionRatio = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "CompressionRatio");
                tmpAudioTrack.Delay = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Delay");
                tmpAudioTrack.DelayString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Delay/String");
                tmpAudioTrack.DelayString1 = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Delay/String1");
                tmpAudioTrack.DelayString2 = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Delay/String2");
                tmpAudioTrack.DelayString3 = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Delay/String3");
                tmpAudioTrack.VideoDelay = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Video_Delay");
                tmpAudioTrack.VideoDelayString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Video_Delay/String");
                tmpAudioTrack.VideoDelayString1 = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Video_Delay/String1");
                tmpAudioTrack.VideoDelayString2 = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Video_Delay/String2");
                tmpAudioTrack.VideoDelayString3 = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Video_Delay/String3");
                tmpAudioTrack.ReplayGainGain = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "ReplayGain_Gain");
                tmpAudioTrack.ReplayGainGainString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "ReplayGain_Gain/String");
                tmpAudioTrack.ReplayGainPeak = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "ReplayGain_Peak");
                tmpAudioTrack.StreamSize = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "StreamSize");
                tmpAudioTrack.StreamSizeString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "StreamSize/String");
                tmpAudioTrack.StreamSizeString1 = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "StreamSize/String1");
                tmpAudioTrack.StreamSizeString2 = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "StreamSize/String2");
                tmpAudioTrack.StreamSizeString3 = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "StreamSize/String3");
                tmpAudioTrack.StreamSizeString4 = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "StreamSize/String4");
                tmpAudioTrack.StreamSizeString5 = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "StreamSize/String5");
                tmpAudioTrack.StreamSizeProportion = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "StreamSize_Proportion");
                tmpAudioTrack.Alignment = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Alignment");
                tmpAudioTrack.AlignmentString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Alignment/String");
                tmpAudioTrack.InterleaveVideoFrames = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Interleave_VideoFrames");
                tmpAudioTrack.InterleaveDuration = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Interleave_Duration");
                tmpAudioTrack.InterleaveDurationString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Interleave_Duration/String");
                tmpAudioTrack.InterleavePreload = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Interleave_Preload");
                tmpAudioTrack.InterleavePreloadString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Interleave_Preload/String");
                tmpAudioTrack.Title = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Title");
                tmpAudioTrack.EncodedLibrary = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Encoded_Library");
                tmpAudioTrack.EncodedLibraryString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Encoded_Library/String");
                tmpAudioTrack.EncodedLibraryName = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Encoded_Library/Name");
                tmpAudioTrack.EncodedLibraryVersion = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Encoded_Library/Version");
                tmpAudioTrack.EncodedLibraryDate = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Encoded_Library/Date");
                tmpAudioTrack.EncodedLibrarySettings = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Encoded_Library_Settings");
                tmpAudioTrack.Language = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Language");
                tmpAudioTrack.LanguageString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Language/String");
                tmpAudioTrack.LanguageMore = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Language_More");
                tmpAudioTrack.EncodedDate = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Encoded_Date");
                tmpAudioTrack.TaggedDate = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Tagged_Date");
                tmpAudioTrack.Encryption = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Encryption");
                tmpAudioTrack.Default = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Default");
                tmpAudioTrack.DefaultString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Default/String");
                tmpAudioTrack.Forced = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Forced");
                tmpAudioTrack.ForcedString = GetSpecificMediaInfo(MediaInfoStreamKind.Audio, i, "Forced/String");

                _AudioTracks.Add(tmpAudioTrack);
            }
        }

        private void LoadTextTracks()
        {
            if (_TextTracks != null)
            {
                return;
            }
            _TextTracks = new List<TextTrack>();
            int trackCount = GetFunctionDelegate<MediaInfo_Count_Get>()(_Handle, MediaInfoStreamKind.Text, -1);
            for (int i = 0; i < trackCount; i++)
            {
                TextTrack tmpTextTrack = new TextTrack();
                tmpTextTrack.Count = GetSpecificMediaInfo(MediaInfoStreamKind.Text, i, "Count");
                tmpTextTrack.StreamCount = GetSpecificMediaInfo(MediaInfoStreamKind.Text, i, "StreamCount");
                tmpTextTrack.StreamKind = GetSpecificMediaInfo(MediaInfoStreamKind.Text, i, "StreamKind");
                tmpTextTrack.StreamKindID = GetSpecificMediaInfo(MediaInfoStreamKind.Text, i, "StreamKindID");
                tmpTextTrack.StreamOrder = GetSpecificMediaInfo(MediaInfoStreamKind.Text, i, "StreamOrder");
                tmpTextTrack.Inform = GetSpecificMediaInfo(MediaInfoStreamKind.Text, i, "Inform");
                tmpTextTrack.ID = GetSpecificMediaInfo(MediaInfoStreamKind.Text, i, "ID");
                tmpTextTrack.UniqueID = GetSpecificMediaInfo(MediaInfoStreamKind.Text, i, "UniqueID");
                tmpTextTrack.Title = GetSpecificMediaInfo(MediaInfoStreamKind.Text, i, "Title");
                tmpTextTrack.Codec = GetSpecificMediaInfo(MediaInfoStreamKind.Text, i, "Codec");
                tmpTextTrack.CodecString = GetSpecificMediaInfo(MediaInfoStreamKind.Text, i, "Codec/String");
                tmpTextTrack.CodecUrl = GetSpecificMediaInfo(MediaInfoStreamKind.Text, i, "Codec/Url");
                tmpTextTrack.Delay = GetSpecificMediaInfo(MediaInfoStreamKind.Text, i, "Delay");
                tmpTextTrack.Video0Delay = GetSpecificMediaInfo(MediaInfoStreamKind.Text, i, "Video0_Delay");
                tmpTextTrack.PlayTime = GetSpecificMediaInfo(MediaInfoStreamKind.Text, i, "PlayTime");
                tmpTextTrack.PlayTimeString = GetSpecificMediaInfo(MediaInfoStreamKind.Text, i, "PlayTime/String");
                tmpTextTrack.PlayTimeString1 = GetSpecificMediaInfo(MediaInfoStreamKind.Text, i, "PlayTime/String1");
                tmpTextTrack.PlayTimeString2 = GetSpecificMediaInfo(MediaInfoStreamKind.Text, i, "PlayTime/String2");
                tmpTextTrack.PlayTimeString3 = GetSpecificMediaInfo(MediaInfoStreamKind.Text, i, "PlayTime/String3");
                tmpTextTrack.Language = GetSpecificMediaInfo(MediaInfoStreamKind.Text, i, "Language");
                tmpTextTrack.LanguageString = GetSpecificMediaInfo(MediaInfoStreamKind.Text, i, "Language/String");
                tmpTextTrack.LanguageMore = GetSpecificMediaInfo(MediaInfoStreamKind.Text, i, "Language_More");
                tmpTextTrack.Default = GetSpecificMediaInfo(MediaInfoStreamKind.Text, i, "Default");
                tmpTextTrack.DefaultString = GetSpecificMediaInfo(MediaInfoStreamKind.Text, i, "Default/String");
                tmpTextTrack.Forced = GetSpecificMediaInfo(MediaInfoStreamKind.Text, i, "Forced");
                tmpTextTrack.ForcedString = GetSpecificMediaInfo(MediaInfoStreamKind.Text, i, "Forced/String");

                _TextTracks.Add(tmpTextTrack);
            }
        }

        private void LoadChapterTracks()
        {
            if (_ChaptersTracks != null)
            {
                return;
            }

            _ChaptersTracks = new List<ChaptersTrack>();
            int trackCount = GetFunctionDelegate<MediaInfo_Count_Get>()(_Handle, MediaInfoStreamKind.Chapters, -1);
            for (int i = 0; i < trackCount; i++)
            {
                ChaptersTrack tmpChapterTrack = new ChaptersTrack();
                tmpChapterTrack.Count = GetSpecificMediaInfo(MediaInfoStreamKind.Chapters, i, "Count");
                tmpChapterTrack.StreamCount = GetSpecificMediaInfo(MediaInfoStreamKind.Chapters, i, "StreamCount");
                tmpChapterTrack.StreamKind = GetSpecificMediaInfo(MediaInfoStreamKind.Chapters, i, "StreamKind");
                tmpChapterTrack.StreamKindID = GetSpecificMediaInfo(MediaInfoStreamKind.Chapters, i, "StreamKindID");
                tmpChapterTrack.StreamOrder = GetSpecificMediaInfo(MediaInfoStreamKind.Chapters, i, "StreamOrder");
                tmpChapterTrack.Inform = GetSpecificMediaInfo(MediaInfoStreamKind.Chapters, i, "Inform");
                tmpChapterTrack.ID = GetSpecificMediaInfo(MediaInfoStreamKind.Chapters, i, "ID");
                tmpChapterTrack.UniqueID = GetSpecificMediaInfo(MediaInfoStreamKind.Chapters, i, "UniqueID");
                tmpChapterTrack.Title = GetSpecificMediaInfo(MediaInfoStreamKind.Chapters, i, "Title");
                tmpChapterTrack.Codec = GetSpecificMediaInfo(MediaInfoStreamKind.Chapters, i, "Codec");
                tmpChapterTrack.CodecString = GetSpecificMediaInfo(MediaInfoStreamKind.Chapters, i, "Codec/String");
                tmpChapterTrack.CodecUrl = GetSpecificMediaInfo(MediaInfoStreamKind.Chapters, i, "Codec/Url");
                tmpChapterTrack.Total = GetSpecificMediaInfo(MediaInfoStreamKind.Chapters, i, "Total");
                tmpChapterTrack.Language = GetSpecificMediaInfo(MediaInfoStreamKind.Chapters, i, "Language");
                tmpChapterTrack.LanguageString = GetSpecificMediaInfo(MediaInfoStreamKind.Chapters, i, "Language/String");
                tmpChapterTrack.Default = GetSpecificMediaInfo(MediaInfoStreamKind.Chapters, i, "Default");
                tmpChapterTrack.DefaultString = GetSpecificMediaInfo(MediaInfoStreamKind.Chapters, i, "Default/String");
                tmpChapterTrack.Forced = GetSpecificMediaInfo(MediaInfoStreamKind.Chapters, i, "Forced");
                tmpChapterTrack.ForcedString = GetSpecificMediaInfo(MediaInfoStreamKind.Chapters, i, "Forced/String");

                _ChaptersTracks.Add(tmpChapterTrack);
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