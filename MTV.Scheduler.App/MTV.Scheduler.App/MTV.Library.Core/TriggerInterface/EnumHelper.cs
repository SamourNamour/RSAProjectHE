using System;

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
[System.SerializableAttribute()]
//[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.harris.com", IncludeInSchema = false)]
public enum ItemsChoiceType
{
    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute(":ACK")]
    ACK,

    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute(":NAK")]
    NAK,

    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute(":event")]
    @event,
}

/// <summary>
/// All event.field xmlelement possible attributs.
/// </summary>
public enum UDPmsgTagAttributs
{
    TITLE,
    VIDEO_ITEM,
    VIDEO_INTIME,
    DURATION,
    BUS,
    TIME,
    TYPE_MATERIAL,
    Unknown
};

/// <summary>
/// 
/// </summary>
public enum EventState : byte
{
    NeedToPrepare = 0,
    Preparing,
    WaitingForReconnect,
    Prepared,
    Working,
    Deleting,
    Deleted,
    Ended,
    succes,
    Failed,
    Error,
    Wrong
};

/// <summary>
/// 
/// </summary>
public enum LogState : byte
{    
    Preparing,
    WaitingForReconnect,    
    Ended,
    EndedWithError
};

/// <summary>
/// 
/// </summary>
public enum Acknowledgement
{
    ACK,
    NAK
};

[Flags]
/// <summary>
/// 
/// </summary>
public enum SegmentState
{
    Unstarted = 0,
    Waiting = 1,
    Running = 2,
    Suspended = 3,
    Stopped = 5,
    Aborted = 6,
    Unknowned = 7,
    Prepared = 8,
    Duplicate = 9
};

[Flags]
/// <summary>
/// 
/// </summary>
public enum AdvertisementState {
    Unstarted = 0,
    Waiting = 1,
    Running = 2,
    Suspended = 3,
    Stopped = 5,
    Aborted = 6,
    Unknowned = 7
};

[Flags]
/// <summary>
/// 
/// </summary>
public enum EpgStatus
{
    Unstarted = 0,
    /// <summary>
    /// Create the XMLs (Command , MPEG7) in Ingesta Folder for all Instance. (Try to get the Schedule) , and change status to Prepared (else aborted)
    /// </summary>
    Preparing = 1, 
    /// <summary>
    /// Create the Schedule in the Encapsulator , Is time to send cmd to the Enc (EPGEntry) ==> change status to Waiting 
    /// </summary>
    Prepared = 2,
    /// <summary>
    /// Check is the time is reached -is yes ; change status to IsRunning).
    /// </summary>
    Waiting = 3,
    /// <summary>
    /// 
    /// </summary>
    Running = 4,
    Suspended = 5,
    Stopped = 6,
    Aborted = 7,
    Unknowned = 8,
    Failed = 9,
    WaitingForStop = 10
};


