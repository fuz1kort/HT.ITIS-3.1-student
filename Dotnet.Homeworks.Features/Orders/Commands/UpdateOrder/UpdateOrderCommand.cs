using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;
using Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;

namespace Dotnet.Homeworks.Features.Orders.Commands.UpdateOrder;

public class UpdateOrderCommand : IOrderOwnerRequest, ICommand
{
    public UpdateOrderCommand(Guid orderId, IEnumerable<Guid> productsIds)
    {
        OrderId = orderId;
        ProductsIds = productsIds;
    }

    public Guid OrderId { get; init; }
    public IEnumerable<Guid> ProductsIds { get; init; }
}