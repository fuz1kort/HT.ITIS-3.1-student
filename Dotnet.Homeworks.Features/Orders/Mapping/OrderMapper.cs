using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Features.Orders.Commands.CreateOrder;
using Dotnet.Homeworks.Features.Orders.Commands.UpdateOrder;
using Dotnet.Homeworks.Features.Orders.Queries.GetOrder;
using Dotnet.Homeworks.Features.Orders.Queries.GetOrders;
using Mapster;

namespace Dotnet.Homeworks.Features.Orders.Mapping;

public class OrderMapper : IOrderMapper
{
    public Order MapToOrder(CreateOrderCommand command, Guid ordererId)
    {
        var config = new TypeAdapterConfig();

        config.NewConfig<CreateOrderCommand, Order>()
            .Map(dest => dest.Id, src => Guid.NewGuid())
            .Map(dest => dest.OrdererId, src => ordererId)
            .Map(dest => dest.ProductsIds, src => src.ProductsIds);

        return command.Adapt<Order>(config);
    }

    public CreateOrderDto MapToCreateOrderDto(Guid id)
    {
        return id.Adapt<CreateOrderDto>();
    }

    public Order MapToOrder(UpdateOrderCommand command, Guid ordererId)
    {
        var config = new TypeAdapterConfig();

        config.NewConfig<UpdateOrderCommand, Order>()
            .Map(dest => dest.Id, src => src.OrderId)
            .Map(dest => dest.OrdererId, src => ordererId)
            .Map(dest => dest.ProductsIds, src => src.ProductsIds);

        return command.Adapt<Order>(config);
    }

    public GetOrderDto MapToGetOrderDto(Order order)
    {
        return order.Adapt<GetOrderDto>();
    }

    public GetOrdersDto MapToGetOrdersDto(IEnumerable<Order> orders)
    {
        return orders.Adapt<GetOrdersDto>();
    }
}