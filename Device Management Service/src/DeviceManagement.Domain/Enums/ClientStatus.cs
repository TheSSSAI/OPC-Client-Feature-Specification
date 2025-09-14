using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace System.Services.DeviceManagement.Domain.Enums;

/// <summary>
/// Represents the possible operational states of an OPC Core Client.
/// This status is typically reported by the client via MQTT and reflects its current health and activity.
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum ClientStatus
{
    /// <summary>
    /// The client has been created in the system but has not yet completed the secure provisioning process.
    /// It cannot connect or perform any operations until it is fully registered.
    /// </summary>
    Pending = 0,

    /// <summary>
    /// The client is provisioned but is not currently connected to the MQTT broker or has missed its heartbeat interval.
    /// It is considered unavailable for commands.
    /// </summary>
    Offline = 1,

    /// <summary>
    /// The client is connected to the MQTT broker and sending regular health status updates.
    /// It is fully operational and ready to receive commands.
    /// </summary>
    Online = 2,

    /// <summary>
    /// The client has received a command and is currently in the process of a software or configuration update.
    /// During this state, it may be temporarily unavailable.
    /// </summary>
    Updating = 3,

    /// <summary>
    /// The client has encountered an unrecoverable error and is not operational.
    /// This state requires administrative intervention.
    /// </summary>
    Error = 4,

    /// <summary>
    /// The initial provisioning token has expired before the client could register.
    /// A new token must be generated.
    /// </summary>
    RegistrationExpired = 5,

    /// <summary>
    /// A command (e.g., software update, configuration push) has been queued for an offline client.
    /// The command will be delivered when the client next connects.
    /// </summary>
    UpdateQueued = 6,

    /// <summary>
    /// The client is in the process of rolling back to a previous software version due to a failed update.
    /// </summary>
    RollbackInProgress = 7,

    /// <summary>
    /// The client's attempt to roll back to a previous version failed.
    /// This state requires manual intervention.
    /// </summary>
    RollbackFailed = 8,
    
    /// <summary>
    /// The client's last attempted update failed.
    /// </summary>
    UpdateFailed = 9
}