using Senac.ProvaSpec.Domain;

namespace Senac.ProvaSpec.Application;

public sealed record RaceSummary(
    IReadOnlyList<LapTime> Laps,
    RaceStatistics? Statistics,
    int LapsNeededForStatistics);

public sealed class InvalidLapTimeException : Exception
{
    public InvalidLapTimeException()
        : base("Informe o tempo no formato MM:SS.mmm, maior que zero, com minutos e segundos entre 00 e 59.")
    {
    }
}

public sealed class LapLimitExceededException : Exception
{
    public LapLimitExceededException()
        : base($"O limite de {Race.MaximumLaps} voltas foi atingido.")
    {
    }
}
