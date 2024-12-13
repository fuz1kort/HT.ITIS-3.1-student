using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Features.Orders.Commands.CreateOrder;
using Dotnet.Homeworks.Features.Orders.Commands.UpdateOrder;
using Dotnet.Homeworks.Features.Orders.Queries.GetOrder;
using Dotnet.Homeworks.Features.Orders.Queries.GetOrders;
using Dotnet.Homeworks.Infrastructure.Utils;

namespace Dotnet.Homeworks.Features.Orders.Mapping;

public interface IOrderMapper: IMapper
{
    Order MapToOrder(CreateOrderCommand command, Guid ordererId);
    CreateOrderDto MapToCreateOrderDto(Guid id);
    Order MapToOrder(UpdateOrderCommand command, Guid ordererId);
    GetOrderDto MapToGetOrderDto(Order order);
    GetOrdersDto MapToGetOrdersDto(IEnumerable<Order> orders);
    
}