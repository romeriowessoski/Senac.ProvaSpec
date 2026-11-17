namespace Senac.ProvaSpec.Domain;

public sealed record RaceStatistics(
    LapTime Average,
    LapTime Median,
    IReadOnlyList<LapTime> Modes,
    long TotalMilliseconds,
    LapTime Fastest)
{
    public static RaceStatistics Calculate(IReadOnlyCollection<LapTime> laps)
    {
        ArgumentNullException.ThrowIfNull(laps);

        if (laps.Count < Race.MinimumLapsForStatistics)
        {
            throw new InvalidOperationException($"São necessárias pelo menos {Race.MinimumLapsForStatistics} voltas para calcular as estatísticas.");
        }

        if (laps.Any(lap => !lap.IsValid))
        {
            throw new ArgumentException("A lista contém um tempo de volta inválido.", nameof(laps));
        }

        var ordered = laps.Select(lap => lap.TotalMilliseconds).Order().ToArray();
        var total = ordered.Sum();
        var average = (total + ordered.Length / 2) / ordered.Length;
        var middle = ordered.Length / 2;
        var median = ordered.Length % 2 == 1
            ? ordered[middle]
            : (ordered[middle - 1] + ordered[middle] + 1) / 2;

        var groups = ordered.GroupBy(value => value)
            .Select(group => new { Value = group.Key, Count = group.Count() })
            .ToArray();
        var maximumFrequency = groups.Max(group => group.Count);
        var modes = maximumFrequency == 1
            ? Array.Empty<LapTime>()
            : groups.Where(group => group.Count == maximumFrequency)
                .Select(group => LapTime.FromMilliseconds(group.Value))
                .ToArray();

        return new RaceStatistics(
            LapTime.FromRoundedMilliseconds(average),
            LapTime.FromRoundedMilliseconds(median),
            modes,
            total,
            LapTime.FromMilliseconds(ordered[0]));
    }
}
