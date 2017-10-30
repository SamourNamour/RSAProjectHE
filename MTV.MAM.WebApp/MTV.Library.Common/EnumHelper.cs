using System.Text;

namespace MTV.Library.Common
{
    /// <summary>
    /// 
    /// </summary>
    public enum MediaType
    {
        /// <summary>
        /// AutoRecording
        /// </summary>
        TV_CHANNEL = 0,

        /// <summary>
        /// DataCasting
        /// </summary>
        DC_CHANNEL = 1,

        /// <summary>
        /// Trailer
        /// </summary>
        TRAILER_CHANNEL = 2,

        /// <summary>
        /// Bonus
        /// </summary>
        BONUS_CHANNEL = 3,

        /// <summary>
        /// Advertisement Channel
        /// </summary>
        ADV_CHANNEL = 6,

        /// <summary>
        /// Dummy Schedule
        /// </summary>
        DUMMY_SCHEDULE = 20,
    };

    /// <summary>
    /// 
    /// </summary>
    public enum ScheduleStatus
    {
        /// <summary>
        /// Event Published.
        /// </summary>
        PREPARED = 1000,

        /// <summary>
        /// Event Started Successfully.
        /// </summary>
        STARTED = 1001,

        /// <summary>
        /// Event Stoped Successfully.
        /// </summary>
        STOPPED = 1002,

        /// <summary>
        /// Trigger Start not received.
        /// </summary>
        MISSING_START = 1003,

        /// <summary>
        /// Trigger Stop not received.
        /// </summary>
        MISSING_STOP = 1004,

        /// <summary>
        /// Error occured during sending Start command to encapsulator.
        /// </summary>
        FAILED_START = 1005,

        /// <summary>
        /// Error occured during sending Stop command to encapsulator.
        /// </summary>
        FAILED_STOP = 1006,

        /// <summary>
        /// Event Locked X (Configurable) minutes before the Estimated start.
        /// </summary>
        LOCKED = 1007,

        /// <summary>
        /// 
        /// </summary>
        TOKEN_OWNED = 1008,

        /// <summary>
        /// 
        /// </summary>
        TOKEN_UNOWNED = 1009,

        /// <summary>
        /// 
        /// </summary>
        TOKEN_ATTEMPT = 1010,
    };

    /// <summary>
    /// 
    /// </summary>
    public enum PosterStatus
    {
        PREPARED = 1000,

        SENT = 1001
    };

    /// <summary>
    /// Les valeur possible pour "Tigger Type"
    /// </summary>
    public enum TriggerType
    {
        /// <summary>
        /// Valeur par défaut.
        /// </summary>
        Default = -1,

        /// <summary>
        /// Le Trigger reçu est Automaric.
        /// </summary>
        Automatic = 0,

        /// <summary>
        /// Le Trigger Reçu est manuel.
        /// </summary>
        Manual = 1
    };

    /// <summary>
    /// 
    /// </summary>
    public enum EncapsulatorType
    {
        /// <summary>
        /// Metadata
        /// </summary>
        Metadata,

        /// <summary>
        /// Asset
        /// </summary>
        Asset,

        /// <summary>
        /// Both Metadata and Asset
        /// </summary>
        Metadata_Asset
    };

    /// <summary>
    /// 
    /// </summary>
    public enum EncapsulatorStatus
    {
        /// <summary>
        /// Main server
        /// </summary>
        Master = 1,

        /// <summary>
        /// Backup
        /// </summary>
        Slave = 0
    };

    /// <summary>
    /// 
    /// </summary>
    public enum ScheduleDeleteStatus
    {
        /// <summary>
        /// 
        /// </summary>
        Default = -1,

        /// <summary>
        /// Mark the Event as deleted.
        /// </summary>
        Deleting = 0,

        /// <summary>
        /// The Command delete is sent
        /// </summary>
        Deleted = 1
    };

    /// <summary>
    /// 
    /// </summary>
    public enum UsersRoles
    {
        MEBSAdmin = 0,
        MEBSMAM = 1,
        MEBSCategorization = 2
    }

    /// <summary>
    /// 
    /// </summary>
    public enum SelfCommercial
    {
        /// <summary>
        /// Content Is Not Commercial
        /// </summary>
        NotLinked = -1,

        /// <summary>
        /// Content Is Commercial but Not Linked To ADV
        /// </summary>
        Prepared = 0,

        /// <summary>
        /// Content Is Linked to ADV
        /// </summary>
        Linked = 1
    }

    /// <summary>
    /// La List des Actions valable pour une PlayList des publicité
    /// </summary>
    public enum AdvertisementActions
    {
        OnPlay,
        OnPause, 
        OnBackward, 
        OnForward
    };

    /// <summary>
    /// 
    /// </summary>
    public enum EpgDummyCommandStatus
    {
        /// <summary>
        /// 
        /// </summary>
        PREPARED = 1000,

        /// <summary>
        /// 
        /// </summary>
        ACK = 1001,

        /// <summary>
        /// 
        /// </summary>
        NAK = 1005,

        /// <summary>
        /// 
        /// </summary>
        TOKEN_OWNED = 1008
    };
}
