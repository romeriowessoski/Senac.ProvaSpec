using Senac.ProvaSpec.Domain;
using Xunit;

namespace Senac.ProvaSpec.Tests;

public sealed class RaceRulesTests
{
    [Theory]
    [InlineData("01:02.003", 62_003)]
    [InlineData("59:59.999", 3_599_999)]
    [InlineData("00:00.001", 1)]
    public void LapTime_parses_valid_values(string input, long expectedMilliseconds)
    {
        Assert.True(LapTime.TryParse(input, out var time));
        Assert.Equal(expectedMilliseconds, time.TotalMilliseconds);
        Assert.Equal(input, time.ToString());
    }

    [Theory]
    [InlineData("00:00.000")]
    [InlineData("60:00.001")]
    [InlineData("00:60.001")]
    [InlineData("01:02.1000")]
    [InlineData("1:02.003")]
    [InlineData("-1:02.003")]
    [InlineData(" 01:02.003")]
    [InlineData("01:02.003 ")]
    [InlineData("01:02,003")]
    public void LapTime_rejects_invalid_values(string input)
    {
        Assert.False(LapTime.TryParse(input, out _));
    }

    [Fact]
    public void Statistics_calculate_mean_median_mode_total_and_fastest()
    {
        var statistics = RaceStatistics.Calculate([Time("00:01.000"), Time("00:01.000"), Time("00:02.000"), Time("00:02.000"), Time("00:03.000")]);

        Assert.Equal("00:01.800", statistics.Average.ToString());
        Assert.Equal("00:02.000", statistics.Median.ToString());
        Assert.Equal(new[] { "00:01.000", "00:02.000" }, statistics.Modes.Select(mode => mode.ToString()));
        Assert.Equal(9_000, statistics.TotalMilliseconds);
        Assert.Equal("00:01.000", statistics.Fastest.ToString());
    }

    [Fact]
    public void Statistics_round_half_milliseconds_up()
    {
        var statistics = RaceStatistics.Calculate([Time("00:00.001"), Time("00:00.002"), Time("00:00.003"), Time("00:00.004")]);

        Assert.Equal("00:00.003", statistics.Average.ToString());
        Assert.Equal("00:00.003", statistics.Median.ToString());
        Assert.Empty(statistics.Modes);
    }

    [Fact]
    public void Statistics_require_at_least_three_laps()
    {
        Assert.Throws<InvalidOperationException>(() => RaceStatistics.Calculate([Time("00:01.000"), Time("00:02.000")]));
    }

    [Fact]
    public void Race_accepts_one_hundred_laps_and_rejects_the_next()
    {
        var race = new Race();
        var lap = Time("00:01.000");

        for (var index = 0; index < Race.MaximumLaps; index++)
        {
            race.AddLap(lap);
        }

        Assert.Throws<InvalidOperationException>(() => race.AddLap(lap));
        Assert.Equal(Race.MaximumLaps, race.Laps.Count);
    }

    private static LapTime Time(string value)
    {
        Assert.True(LapTime.TryParse(value, out var time));
        return time;
    }
}
