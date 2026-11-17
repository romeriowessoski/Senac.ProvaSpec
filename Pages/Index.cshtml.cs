using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Senac.ProvaSpec.Application;
using Senac.ProvaSpec.Domain;

namespace Senac.ProvaSpec.Pages;

public sealed class IndexModel(LapTimeApplicationService service) : PageModel
{
    [BindProperty]
    public string? Tempo { get; set; }

    public IReadOnlyList<LapRow> Laps { get; private set; } = [];
    public RaceStatistics? Statistics { get; private set; }
    public int LapsNeeded { get; private set; }
    public string? SuccessMessage { get; private set; }

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        SuccessMessage = TempData["SuccessMessage"] as string;
        await LoadAsync(cancellationToken);
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        try
        {
            await service.AddLapAsync(Tempo, cancellationToken);
            TempData["SuccessMessage"] = "Volta registrada com sucesso.";
            return RedirectToPage();
        }
        catch (InvalidLapTimeException exception)
        {
            ModelState.AddModelError(nameof(Tempo), exception.Message);
        }
        catch (LapLimitExceededException exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
        }

        await LoadAsync(cancellationToken);
        return Page();
    }

    private async Task LoadAsync(CancellationToken cancellationToken)
    {
        var summary = await service.GetSummaryAsync(cancellationToken);
        Statistics = summary.Statistics;
        LapsNeeded = summary.LapsNeededForStatistics;
        var fastest = Statistics?.Fastest;
        Laps = summary.Laps
            .Select((lap, index) => new LapRow(index + 1, lap, fastest.HasValue && lap == fastest.Value))
            .ToArray();
    }
}

public sealed record LapRow(int Number, LapTime Time, bool IsFastest);
