using CsvHelper.Configuration;

namespace Sundroid.Homework.DataLoader.Parsing;

public sealed class ParsedLogItemLineClassMap : ClassMap<ParsedLogItemLine>
{
    public ParsedLogItemLineClassMap()
    {
        Map(i => i.Time).Index(0).TypeConverter<UtcDateTimeOffsetConverter>();
        Map(i => i.Values.Upv1).Index(1);
        Map(i => i.Values.Upv2).Index(2);
        Map(i => i.Values.Upv3).Index(3);
        Map(i => i.Values.Upv4).Index(4);
        Map(i => i.Values.Upv5).Index(5);
        Map(i => i.Values.Upv6).Index(6);
        Map(i => i.Values.Upv7).Index(7);
        Map(i => i.Values.Upv8).Index(8);
        Map(i => i.Values.Ipv1).Index(9);
        Map(i => i.Values.Ipv2).Index(10);
        Map(i => i.Values.Ipv3).Index(11);
        Map(i => i.Values.Ipv4).Index(12);
        Map(i => i.Values.Ipv5).Index(13);
        Map(i => i.Values.Ipv6).Index(14);
        Map(i => i.Values.Ipv7).Index(15);
        Map(i => i.Values.Ipv8).Index(16);
        Map(i => i.Values.Uac1).Index(17);
        Map(i => i.Values.Uac2).Index(18);
        Map(i => i.Values.Uac3).Index(19);
        Map(i => i.Values.Iac1).Index(20);
        Map(i => i.Values.Iac2).Index(21);
        Map(i => i.Values.Iac3).Index(22);
        Map(i => i.Values.Status).Index(23);
        Map(i => i.Values.Error).Index(24);
        Map(i => i.Values.Temp).Index(25);
        Map(i => i.Values.Cos).Index(26);
        Map(i => i.Values.Fac).Index(27);
        Map(i => i.Values.Pac).Index(28);
        Map(i => i.Values.Qac).Index(29);
        Map(i => i.Values.Eac).Index(30);
        Map(i => i.Values.EDay).Index(31);
        Map(i => i.Values.ETotal).Index(32);
        Map(i => i.CycleTime).Index(33);
    }
}