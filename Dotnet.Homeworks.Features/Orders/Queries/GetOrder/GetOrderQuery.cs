using Dotnet.Homeworks.Infrastructure.Cqrs.Queries;
using Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;

namespace Dotnet.Homeworks.Features.Orders.Queries.GetOrder;

public class GetOrderQuery : IOrderOwnerRequest, IQuery<GetOrderDto>
{
    public GetOrderQuery(Guid orderId)
    {
        OrderId = orderId;
    }

    public Guid OrderId { get; init; }
}