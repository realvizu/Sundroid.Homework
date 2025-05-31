namespace Sundroid.Homework.Persistence.Entities;

/// <summary>
/// Defines the numeric values of a log entry.
/// Separated from the DB entity to be reusable in other parts of the application.
/// </summary>
public sealed class ValueSet
{
    public decimal Upv1 { get; set; }
    public decimal Upv2 { get; set; }
    public decimal Upv3 { get; set; }
    public decimal Upv4 { get; set; }
    public decimal Upv5 { get; set; }
    public decimal Upv6 { get; set; }
    public decimal Upv7 { get; set; }
    public decimal Upv8 { get; set; }
    public decimal Ipv1 { get; set; }
    public decimal Ipv2 { get; set; }
    public decimal Ipv3 { get; set; }
    public decimal Ipv4 { get; set; }
    public decimal Ipv5 { get; set; }
    public decimal Ipv6 { get; set; }
    public decimal Ipv7 { get; set; }
    public decimal Ipv8 { get; set; }
    public decimal Uac1 { get; set; }
    public decimal Uac2 { get; set; }
    public decimal Uac3 { get; set; }
    public decimal Iac1 { get; set; }
    public decimal Iac2 { get; set; }
    public decimal Iac3 { get; set; }
    public int Status { get; set; }
    public int Error { get; set; }
    public decimal Temp { get; set; }
    public decimal Cos { get; set; }
    public decimal Fac { get; set; }
    public decimal Pac { get; set; }
    public decimal Qac { get; set; }
    public decimal Eac { get; set; }
    public decimal EDay { get; set; }
    public decimal ETotal { get; set; }
}