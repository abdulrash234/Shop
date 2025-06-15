using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shop.Application.Interfaces;

namespace Shop.Application.Services;

public class MonthlyReportBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<MonthlyReportBackgroundService> _logger;
    private Timer? _timer;

    public MonthlyReportBackgroundService(
        IServiceScopeFactory scopeFactory,
        ILogger<MonthlyReportBackgroundService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        //For the sake of demo I have put the timespan to be quite short.
        _timer = new Timer(CheckAndSendReport, null, TimeSpan.Zero, TimeSpan.FromSeconds(6));
        return Task.CompletedTask;
    }

    private async void CheckAndSendReport(object? state)
    {
        using var scope = _scopeFactory.CreateScope();
        var reportService = scope.ServiceProvider.GetRequiredService<IMonthlyReportService>();

        try
        {
            var today = DateTime.UtcNow.Date;
            var tomorrow = today.AddDays(1);

            bool isLastDayOfMonth = tomorrow.Month != today.Month; 

            var currentMonth = new DateTime(today.Year, today.Month, 1);

            if (isLastDayOfMonth)
            {
                _logger.LogInformation("Sending monthly reports for {Month}", currentMonth.ToString("yyyy-MM"));
                await reportService.GenerateAndSendMonthlyReportsAsync();
                _logger.LogInformation("Monthly reports sent and logged.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during monthly report check.");
        }
    }
    
    public override void Dispose()
    {
        _timer?.Dispose();
        base.Dispose();
    }
    
}