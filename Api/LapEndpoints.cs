using System.Globalization;
using Senac.ProvaSpec.Application;
using Senac.ProvaSpec.Domain;

namespace Senac.ProvaSpec.Api;

public static class LapEndpoints
{
    public static IEndpointRouteBuilder MapLapEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/voltas");

        group.MapGet("/", async (LapTimeApplicationService service, CancellationToken cancellationToken) =>
        {
            var summary = await service.GetSummaryAsync(cancellationToken);
            return Results.Ok(ToResponse(summary));
        });

        group.MapPost("/", async (AddLapRequest request, LapTimeApplicationService service, CancellationToken cancellationToken) =>
        {
            try
            {
                var summary = await service.AddLapAsync(request.Tempo, cancellationToken);
                return Results.Ok(ToResponse(summary));
            }
            catch (InvalidLapTimeException exception)
            {
                return Results.BadRequest(new { erro = exception.Message });
            }
            catch (LapLimitExceededException exception)
            {
                return Results.Conflict(new { erro = exception.Message });
            }
        });

        return endpoints;
    }

    private static object ToResponse(RaceSummary summary)
    {
        var fastest = summary.Statistics?.Fastest;
        var laps = summary.Laps.Select((lap, index) => new
        {
            volta = index + 1,
            tempo = lap.ToString(),
            maisRapida = fastest.HasValue && lap == fastest.Value
        }).ToArray();

        object? statistics = null;
        if (summary.Statistics is { } result)
        {
            var modes = result.Modes.Select(mode => mode.ToString()).ToArray();
            statistics = new
            {
                media = result.Average.ToString(),
                mediana = result.Median.ToString(),
                moda = modes,
                mensagemModa = modes.Length == 0 ? "Não houve moda." : null,
                tempoTotal = FormatTotal(result.TotalMilliseconds),
                voltaMaisRapida = result.Fastest.ToString()
            };
        }

        return new
        {
            voltas = laps,
            voltasRestantesParaEstatisticas = summary.LapsNeededForStatistics,
            estatisticas = statistics
        };
    }

    private static string FormatTotal(long milliseconds)
    {
        var hours = milliseconds / 3_600_000;
        var minutes = milliseconds / 60_000 % 60;
        var seconds = milliseconds / 1_000 % 60;
        var remainder = milliseconds % 1_000;

        return hours > 0
            ? string.Create(CultureInfo.InvariantCulture, $"{hours:00}:{minutes:00}:{seconds:00}.{remainder:000}")
            : string.Create(CultureInfo.InvariantCulture, $"{milliseconds / 60_000:00}:{seconds:00}.{remainder:000}");
    }
}
