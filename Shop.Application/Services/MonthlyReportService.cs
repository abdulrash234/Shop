using FluentEmail.Core;
using QuestPDF.Fluent;
using Shop.Application.Interfaces;
using Shop.Application.Models;

namespace Shop.Application.Services;

public class MonthlyReportService : IMonthlyReportService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IFluentEmail _email;
    private readonly IUserRepository _userRepository;

    public MonthlyReportService(IOrderRepository orderRepository, IFluentEmail email, IUserRepository userRepository)
    {
        _orderRepository = orderRepository;
        _email = email;
        _userRepository = userRepository;
    }

    public async Task GenerateAndSendMonthlyReportsAsync()
    {
        var startOfMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
        var endOfMonth = startOfMonth.AddMonths(1);

        var users = await _userRepository.GetAllAsync(); 

        foreach (var user in users)
        {
            var orders = await _orderRepository.GetOrdersByUserIdAsync(user.Id);

            var monthlyOrders = orders
                .Where(o => o.CreatedAt >= startOfMonth && o.CreatedAt < endOfMonth)
                .ToList();

            if (!monthlyOrders.Any()) continue;

            var pdf = GeneratePdf(user, monthlyOrders);

            await _email
                .To(user.Email, user.Name)
                .Subject("Your Monthly Order Summary")
                .Body("Attached is your monthly summary of orders.")
                .Attach(new FluentEmail.Core.Models.Attachment
                {
                    Data = new MemoryStream(pdf),
                    Filename = "MonthlyReport.pdf",
                    ContentType = "application/pdf"
                })
                .SendAsync();
        }
    }

    private byte[] GeneratePdf(User user, List<Order> orders)
    {
        using var stream = new MemoryStream();

        Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(30);
                page.Header().Text($"Monthly Order Summary for {user.Name}")
                    .FontSize(18).Bold();

                page.Content().Column(col =>
                {
                    foreach (var order in orders)
                    {
                        col.Item().Text($"Order ID: {order.Id} — {order.CreatedAt:yyyy-MM-dd}").Bold();

                        foreach (var item in order.Items)
                        {
                            col.Item().Text($"• {item.Product.Name} x{item.Quantity} @ {item.UnitPrice:C}");
                        }

                        col.Item().Text(""); 
                    }
                });

                page.Footer().Text($"Generated on {DateTime.UtcNow:yyyy-MM-dd HH:mm} UTC");
            });
        }).GeneratePdf(stream);

        return stream.ToArray();
    }
}