
/// <summary>
/// 
/// </summary>
using System;
using System.ComponentModel;

/// <summary>
/// 
/// </summary>
public enum MediaType {    
    /// <summary>
    /// 
    /// </summary>
    UNKNOWN_CHANNEL = -1,

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
    /// 
    /// </summary>
    ADVERTISEMENT_CHANNEL = 6,
};

/// <summary>
/// 
/// </summary>
public enum EncapsulatorType {
    /// <summary>
    /// AutoRecording
    /// </summary>
    Metadata = 0,

    /// <summary>
    /// DataCasting
    /// </summary>
    Asset = 1
};

/// <summary>
/// 
/// </summary>
public enum LogMode {
    /// <summary>
    /// 
    /// </summary>
    Error,

    /// <summary>
    /// 
    /// </summary>
    Information,

    /// <summary>
    /// 
    /// </summary>
    Warning


}

/// <summary>
/// 
/// </summary>
public enum EventExtendedInfo {
    /// <summary>
    /// 
    /// </summary>
    SHORT_DESCRIPTION,

    /// <summary>
    /// 
    /// </summary>
    DESCRIPTION,

    /// <summary>
    /// 
    /// </summary>
    ACTORS,

    /// <summary>
    /// 
    /// </summary>
    DIRECTORS,

    /// <summary>
    /// 
    /// </summary>
    SCREEN_FORMAT,

    /// <summary>
    /// 
    /// </summary>
    PARENTAL_RATING,

    /// <summary>
    /// 
    /// </summary>
    MEDIA_FILE_SIZE_AFTER_REDUNDANCY,

    /// <summary>
    /// 
    /// </summary>
    MEDIA_FILE_NAME_AFTER_REDUNDANCY
};

/// <summary>
/// 
/// </summary>
public enum ScheduleStatus {
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
public enum ChannelCreateStatus {

    /// <summary>
    /// The Channel was successfully created.
    /// </summary>
    Success,

    /// <summary>
    /// The Bus is not formatted correctly.
    /// </summary>
    InvalidChannelBus,

    /// <summary>
    /// The Key is not formatted correctly.
    /// </summary>
    InvalidChannelKey,

    /// <summary>
    /// The Channel was not created, for a reason defined by the provider -Channel Already Exists-.
    /// </summary>
    ChannelRejected,

    /// <summary>
    /// The provider returned an error.
    /// </summary>
    ProviderError,

    /// <summary>
    /// The provider returned a failed operation.
    /// </summary>
    OperationFailed
};

/// <summary>
/// 
/// </summary>
public enum EpgCreateStatus {
    /// <summary>
    /// The EPG was successfully created.
    /// </summary>
    Success,

    /// <summary>
    /// The VideoItem is not formatted correctly.
    /// </summary>
    InvalidEpgVideoItem,

    /// <summary>
    /// The Channel was not created, for a reason defined by the provider -EPG Already Exists-.
    /// </summary>
    EpgRejected,

    /// <summary>
    /// The provider returned an error.
    /// </summary>
    ProviderError,

    /// <summary>
    /// The provider returned a failed operation.
    /// </summary>
    OperationFailed
};

/// <summary>
/// 
/// </summary>
public enum EpgDummyCommandStatus {
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

/// <summary>
/// 
/// </summary>
public enum CatchupTVCommandType {
    /// <summary>
    /// 
    /// </summary>
    DUMMY,

    /// <summary>
    /// 
    /// </summary>
    ACTUAL,

    /// <summary>
    /// 
    /// </summary>
    ADVERTISEMENT
}

/// <summary>
/// 
/// </summary>
public enum TriggerType {
    /// <summary>
    /// 
    /// </summary>
    Default = -1,

    /// <summary>
    /// 
    /// </summary>
    Automatic = 0,

    /// <summary>
    /// 
    /// </summary>
    Manual = 1
};

[Flags]
/// <summary>
/// 
/// </summary>
public enum MaterialType {
    [Description("Other")] 
    O = 0,

    [Description("Commercial")] 
    C = 1,

    [Description("Self Promotion")] 
    P = 2,

    [Description("Live")] 
    L = 3,

    [Description("Movie")] 
    M = 4,

    [Description("Teleshopping")] 
    T = 5,

    [Description("Sponsor")] 
    S = 6,

    [Description("Jingle")] 
    J = 7,

    [Description("Unknown")] 
    U = 8
};

/// <summary>
/// 
/// </summary>
public enum PosterTransferStatus
{
    /// <summary>
    /// 
    /// </summary>
    PREPARED = 1000,

    /// <summary>
    /// 
    /// </summary>
    FAILED = 1001,

    /// <summary>
    /// 
    /// </summary>
    ERROR = 1002,

    /// <summary>
    /// 
    /// </summary>
    SENT = 1003,

    /// <summary>
    /// 
    /// </summary>
    TOKEN_OWNED = 1008
}

/// <summary>
/// 
/// </summary>
public enum SelfCommercial {
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
};

/// <summary>
/// 
/// </summary>
public enum ScheduleDeleteStatus {
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
public enum MediaFileCreateStatus {

    /// <summary>
    /// The Channel was successfully created.
    /// </summary>
    Success,

    /// <summary>
    /// The File name is not formatted correctly.
    /// </summary>
    InvalidFileName,

    /// <summary>
    /// Unsupported file size.
    /// </summary>
    InvalidFileSize,

    /// <summary>
    /// The MediaFile was not created, for a reason defined by the provider -MediaFile Already Exists-.
    /// </summary>
    MediaFileRejected,

    /// <summary>
    /// The provider returned an error.
    /// </summary>
    ProviderError,

    /// <summary>
    /// The provider returned a failed operation.
    /// </summary>
    OperationFailed
};

/// <summary>
/// 
/// </summary>
public enum MEPG7Status {
    /// <summary>
    /// 
    /// </summary>
    PREPARED,

    /// <summary>
    /// 
    /// </summary>
    UPLOADED,

    /// <summary>
    /// 
    /// </summary>
    ACK,

    /// <summary>
    /// 
    /// </summary>
    NAK,
};

/// <summary>
/// 
/// </summary>
public enum DCCommandStatus {
    /// <summary>
    /// 
    /// </summary>
    PREPARED,

    /// <summary>
    /// 
    /// </summary>
    UPLOADED,

    /// <summary>
    /// 
    /// </summary>
    ACK,

    /// <summary>
    /// 
    /// </summary>
    NAK,
};

/// <summary>
/// 
/// </summary>
public enum CopyControl {
    /// <summary>
    /// 
    /// </summary>
    [Description("allow")]
    Allow = 0,

    /// <summary>
    /// 
    /// </summary>
    [Description("copy once")]
    CopyOnce = 1,

    /// <summary>
    /// 
    /// </summary>
    [Description("copy no more")]
    CopyNoMore = 2,

    /// <summary>
    /// 
    /// </summary>
    [Description("copy never")]
    CopyNever = 3,

    /// <summary>
    /// 
    /// </summary>
    [Description("copy never zero retention")]
    CopyNeverZeroRetention = 4
};

/// <summary>
/// 
/// </summary>
public enum ARExtendedStatus {
    /// <summary>
    /// 
    /// </summary>
    UNSUPPORTED,

    /// <summary>
    /// 
    /// </summary>
    PREPARED,

    /// <summary>
    /// 
    /// </summary>
    UPLOADED,

    /// <summary>
    /// 
    /// </summary>
    WAITING,

    /// <summary>
    /// 
    /// </summary>
    BLOCKED,

    /// <summary>
    /// 
    /// </summary>
    ACK,

    /// <summary>
    /// 
    /// </summary>
    NAK,
};

/// <summary>
/// 
/// </summary>
public enum FuturDCListStatus
{
    /// <summary>
    /// 
    /// </summary>
    ACK,

    /// <summary>
    /// 
    /// </summary>
    NAK,
};

/// <summary>
/// 
/// </summary>
public enum SendCategoryStatus
{
    /// <summary>
    /// 
    /// </summary>
    ACK,

    /// <summary>
    /// 
    /// </summary>
    NAK,
};