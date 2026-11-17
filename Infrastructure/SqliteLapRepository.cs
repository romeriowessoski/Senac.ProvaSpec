using Microsoft.EntityFrameworkCore;
using Senac.ProvaSpec.Application;
using Senac.ProvaSpec.Domain;

namespace Senac.ProvaSpec.Infrastructure;

public sealed class SqliteLapRepository(LapTimeDbContext dbContext) : ILapRepository
{
    public async Task<IReadOnlyList<LapTime>> GetLapsAsync(CancellationToken cancellationToken = default)
    {
        var entries = await dbContext.Laps
            .AsNoTracking()
            .OrderBy(lap => lap.Id)
            .ToListAsync(cancellationToken);

        return entries.Select(lap => LapTime.FromMilliseconds(lap.TotalMilliseconds)).ToArray();
    }

    public async Task AddLapAsync(LapTime lapTime, CancellationToken cancellationToken = default)
    {
        dbContext.Laps.Add(new LapEntity { TotalMilliseconds = lapTime.TotalMilliseconds });
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
