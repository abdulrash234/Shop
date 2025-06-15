namespace Shop.Application.Interfaces;

public interface IMonthlyReportService
{
    Task GenerateAndSendMonthlyReportsAsync();
}