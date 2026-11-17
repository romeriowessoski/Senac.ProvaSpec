using Senac.ProvaSpec.Domain;

namespace Senac.ProvaSpec.Application;

public sealed class LapTimeApplicationService(ILapRepository repository)
{
    public async Task<RaceSummary> GetSummaryAsync(CancellationToken cancellationToken = default)
    {
        var laps = await repository.GetLapsAsync(cancellationToken);
        var race = new Race(laps);
        var statistics = race.Laps.Count >= Race.MinimumLapsForStatistics
            ? RaceStatistics.Calculate(race.Laps)
            : null;
        var lapsNeeded = Math.Max(0, Race.MinimumLapsForStatistics - race.Laps.Count);

        return new RaceSummary(race.Laps.ToArray(), statistics, lapsNeeded);
    }

    public async Task<RaceSummary> AddLapAsync(string? input, CancellationToken cancellationToken = default)
    {
        if (!LapTime.TryParse(input, out var lapTime))
        {
            throw new InvalidLapTimeException();
        }

        var race = new Race(await repository.GetLapsAsync(cancellationToken));
        try
        {
            race.AddLap(lapTime);
        }
        catch (InvalidOperationException)
        {
            throw new LapLimitExceededException();
        }

        await repository.AddLapAsync(lapTime, cancellationToken);
        return await GetSummaryAsync(cancellationToken);
    }
}
