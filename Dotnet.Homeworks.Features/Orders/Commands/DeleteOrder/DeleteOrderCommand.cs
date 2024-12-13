using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;
using Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;

namespace Dotnet.Homeworks.Features.Orders.Commands.DeleteOrder;

public class DeleteOrderByGuidCommand : IOrderOwnerRequest, ICommand
{
    public DeleteOrderByGuidCommand(Guid orderId)
    {
        OrderId = orderId;
    }

    public Guid OrderId { get; init; }
}