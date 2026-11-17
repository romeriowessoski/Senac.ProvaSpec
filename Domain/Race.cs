namespace Senac.ProvaSpec.Domain;

public sealed class Race
{
    public const int MinimumLapsForStatistics = 3;
    public const int MaximumLaps = 100;

    private readonly List<LapTime> _laps;

    public IReadOnlyList<LapTime> Laps => _laps;

    public Race(IEnumerable<LapTime>? laps = null)
    {
        _laps = laps?.ToList() ?? [];

        if (_laps.Count > MaximumLaps)
        {
            throw new ArgumentException($"A prova não pode ter mais de {MaximumLaps} voltas.", nameof(laps));
        }

        if (_laps.Any(lap => !lap.IsValid))
        {
            throw new ArgumentException("A prova contém um tempo de volta inválido.", nameof(laps));
        }
    }

    public void AddLap(LapTime lapTime)
    {
        if (!lapTime.IsValid)
        {
            throw new ArgumentException("O tempo de volta é inválido.", nameof(lapTime));
        }

        if (_laps.Count >= MaximumLaps)
        {
            throw new InvalidOperationException($"O limite de {MaximumLaps} voltas foi atingido.");
        }

        _laps.Add(lapTime);
    }
}
