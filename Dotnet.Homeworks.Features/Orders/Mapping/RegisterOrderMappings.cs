using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Features.Orders.Commands.CreateOrder;
using Dotnet.Homeworks.Features.Orders.Queries.GetOrder;
using Dotnet.Homeworks.Features.Orders.Queries.GetOrders;
using Mapster;

namespace Dotnet.Homeworks.Features.Orders.Mapping;

public class RegisterOrderMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Guid, CreateOrderDto>()
            .Map(dest => dest.Id, src => src);

        config.NewConfig<Order, GetOrderDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.ProductsIds, src => src.ProductsIds);
        
        config.NewConfig<IEnumerable<Order>, GetOrdersDto>()
            .Map(dest => dest.Orders, src => src.Select(x => x.Adapt<GetOrderDto>()));
    }
}