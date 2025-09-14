namespace System.Shared.Domain.Enums;

/// <summary>
/// Defines the predefined, configurable user roles within the system, as per REQ-1-011.
/// Each role corresponds to a distinct set of permissions.
/// </summary>
public enum UserRole
{
    /// <summary>
    /// Has unrestricted, system-wide privileges, including tenant and user management.
    /// </summary>
    Administrator,

    /// <summary>
    /// Has permissions tailored for AI/ML model development and access to sandboxed data.
    /// </summary>
    DataScientist,

    /// <summary>
    /// Has permissions to configure and manage the system's operational aspects like data sources and dashboards.
    /// </summary>
    Engineer,

    /// <summary>
    /// Has permissions for day-to-day plant monitoring and operation, including alarm acknowledgement.
    /// </summary>
    Operator,

    /// <summary>
    /// A strictly read-only role with access to view assigned dashboards and reports.
    /// </summary>
    Viewer
}