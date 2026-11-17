using Senac.ProvaSpec.Domain;

namespace Senac.ProvaSpec.Application;

public interface ILapRepository
{
    Task<IReadOnlyList<LapTime>> GetLapsAsync(CancellationToken cancellationToken = default);
    Task AddLapAsync(LapTime lapTime, CancellationToken cancellationToken = default);
}
