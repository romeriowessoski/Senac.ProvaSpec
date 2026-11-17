using System.Globalization;

namespace Senac.ProvaSpec.Domain;

public readonly record struct LapTime
{
    public long TotalMilliseconds { get; }
    public bool IsValid => TotalMilliseconds is > 0 and <= 3_599_999;

    private LapTime(long totalMilliseconds)
    {
        TotalMilliseconds = totalMilliseconds;
    }

    public static bool TryParse(string? value, out LapTime lapTime)
    {
        lapTime = default;

        if (value is null || value.Length != 9 || value[2] != ':' || value[5] != '.')
        {
            return false;
        }

        if (!AreDigits(value, 0, 2) || !AreDigits(value, 3, 2) || !AreDigits(value, 6, 3))
        {
            return false;
        }

        var minutes = ParseDigits(value, 0, 2);
        var seconds = ParseDigits(value, 3, 2);
        var milliseconds = ParseDigits(value, 6, 3);

        if (minutes > 59 || seconds > 59)
        {
            return false;
        }

        var totalMilliseconds = (minutes * 60L + seconds) * 1000 + milliseconds;
        if (totalMilliseconds == 0)
        {
            return false;
        }

        lapTime = new LapTime(totalMilliseconds);
        return true;
    }

    public static LapTime FromMilliseconds(long totalMilliseconds)
    {
        if (totalMilliseconds <= 0 || totalMilliseconds > 3_599_999)
        {
            throw new ArgumentOutOfRangeException(nameof(totalMilliseconds));
        }

        return new LapTime(totalMilliseconds);
    }

    public static LapTime FromRoundedMilliseconds(long totalMilliseconds)
    {
        return FromMilliseconds(totalMilliseconds);
    }

    public override string ToString()
    {
        var minutes = TotalMilliseconds / 60_000;
        var seconds = TotalMilliseconds / 1_000 % 60;
        var milliseconds = TotalMilliseconds % 1_000;
        return string.Create(CultureInfo.InvariantCulture, $"{minutes:00}:{seconds:00}.{milliseconds:000}");
    }

    private static bool AreDigits(string value, int start, int length)
    {
        for (var index = start; index < start + length; index++)
        {
            if (value[index] is < '0' or > '9')
            {
                return false;
            }
        }

        return true;
    }

    private static int ParseDigits(string value, int start, int length)
    {
        var number = 0;
        for (var index = start; index < start + length; index++)
        {
            number = number * 10 + value[index] - '0';
        }

        return number;
    }
}
