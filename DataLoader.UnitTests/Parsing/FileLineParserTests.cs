using System.Globalization;
using FluentAssertions;
using Sundroid.Homework.DataLoader.Parsing;
using Sundroid.Homework.Persistence.Entities;
using Xunit;

namespace Sundroid.Homework.DataLoader.UnitTests.Parsing;

public class FileLineParserTests
{
    [Theory]
    [InlineData("#INV1 ESN:21010730236TLC900294", "INV1")]
    [InlineData("#INV123456789 ESN:21010730236TLC900294", "INV123456789")]
    public void ExtractInverterName_ValidInput_ShouldReturnCorrectResult(string line, string expectedResult)
    {
        FileLineParser.ExtractInverterName(line).Should().Be(expectedResult);
    }

    [Theory]
    [InlineData("#SmartLogger ESN:102110045721")]
    [InlineData("#INV1")]
    [InlineData("# INV1 ESN:21010730236TLC900294")]
    [InlineData("")]
    public void ExtractInverterName_InvalidInput_ShouldThrowFileParsingException(string line)
    {
        var action = () => FileLineParser.ExtractInverterName(line);
        action.Should().Throw<FileParsingException>().Where(i => i.Message.Contains(line));
    }

    [Theory]
    [InlineData("#SmartLogger ESN:102110045721", "102110045721")]
    [InlineData("#SmartLogger    ESN:102110045721    ", "102110045721")]
    [InlineData("#INV1 ESN:21010730236TLC900294", "21010730236TLC900294")]
    [InlineData("#INV1 ESN:  21010730236TLC900294", "21010730236TLC900294")]
    public void ExtractSerialNumber_ValidInput_ShouldReturnCorrectResult(string line, string expectedResult)
    {
        FileLineParser.ExtractSerialNumber(line).Should().Be(expectedResult);
    }

    [Theory]
    [InlineData("#SmartLogger ES:102110045721")]
    [InlineData("#INV1 ESN 21010730236TLC900294")]
    [InlineData("")]
    public void ExtractSerialNumber_InvalidInput_ShouldThrowFileParsingException(string line)
    {
        var action = () => FileLineParser.ExtractSerialNumber(line);
        action.Should().Throw<FileParsingException>().Where(i => i.Message.Contains(line));
    }

    [Fact]
    public void ParseDataLoggerLine_ValidInput_ShouldReturnCorrectResult()
    {
        FileLineParser.ParseDataLoggerLine("#SmartLogger ESN:102110045721")
            .Should().BeEquivalentTo(new ParsedDataLoggerLine { SerialNumber = "102110045721" });
    }

    [Fact]
    public void ParseInverterLine_ValidInput_ShouldReturnCorrectResult()
    {
        FileLineParser.ParseInverterLine("#INV1 ESN:21010730236TLC900294")
            .Should().BeEquivalentTo(new ParsedInverterLine { Name = "INV1", SerialNumber = "21010730236TLC900294" });
    }

    [Fact]
    public void ParseLogItemLine_ValidInput_ShouldParseAllColumns()
    {
        FileLineParser.ParseLogItemLine(
                "2025-05-15 13:00:00;1;2;3;4;5;6;7;8;9;10;11;12;13;14;15;16;17;18;19;20;21;22;23;24;25;26;27;28;29;30;31;32;33;"
            )
            .Should()
            .BeEquivalentTo(new ParsedLogItemLine
                {
                    Time = DateTimeOffset.Parse("2025-05-15 13:00:00", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal),
                    Values = new ValueSet
                    {
                        Upv1 = 1,
                        Upv2 = 2,
                        Upv3 = 3,
                        Upv4 = 4,
                        Upv5 = 5,
                        Upv6 = 6,
                        Upv7 = 7,
                        Upv8 = 8,
                        Ipv1 = 9,
                        Ipv2 = 10,
                        Ipv3 = 11,
                        Ipv4 = 12,
                        Ipv5 = 13,
                        Ipv6 = 14,
                        Ipv7 = 15,
                        Ipv8 = 16,
                        Uac1 = 17,
                        Uac2 = 18,
                        Uac3 = 19,
                        Iac1 = 20,
                        Iac2 = 21,
                        Iac3 = 22,
                        Status = 23,
                        Error = 24,
                        Temp = 25,
                        Cos = 26,
                        Fac = 27,
                        Pac = 28,
                        Qac = 29,
                        Eac = 30,
                        EDay = 31,
                        ETotal = 32
                    },
                    CycleTime = 33
                }
            );
    }

    [Fact]
    public void ParseLogItemLine_ValidInput_ShouldParseDecimalValuesCorrectly()
    {
        FileLineParser.ParseLogItemLine(
                "2025-05-15 13:00:00;1234.5678;2;3;4;5;6;7;8;9;10;11;12;13;14;15;16;17;18;19;20;21;22;23;24;25;26;27;28;29;30;31;32;33;"
            )
            .Values.Upv1.Should().Be(1234.5678m);
    }

    [Fact]
    public void ParseLogItemLine_ValidInput_ShouldParseTimestampAsUtc()
    {
        FileLineParser.ParseLogItemLine(
                "2025-05-15 13:00:00;1;2;3;4;5;6;7;8;9;10;11;12;13;14;15;16;17;18;19;20;21;22;23;24;25;26;27;28;29;30;31;32;33;"
            )
            .Time.ToUniversalTime()
            .Should().Be(new DateTimeOffset(2025, 5, 15, 13, 0, 0, TimeSpan.Zero));
    }
}