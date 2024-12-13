using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Features.Products.Commands.InsertProduct;
using Dotnet.Homeworks.Features.Products.Commands.UpdateProduct;
using Dotnet.Homeworks.Features.Products.Queries.GetProducts;
using Mapster;

namespace Dotnet.Homeworks.Features.Products.Mapping;

public class RegisterProductMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<InsertProductCommand, Product>()
            .Map(dest => dest.Name, src => src.Name);

        config.NewConfig<Guid, InsertProductDto>()
            .Map(dest => dest.Guid, src => src);
        
        config.NewConfig<UpdateProductCommand, Product>()
            .Map(dest => dest.Id, src => src.Guid)
            .Map(dest => dest.Name, src => src.Name);
        
        config.NewConfig<Product, GetProductDto>()
            .Map(dest => dest.Guid, src => src.Id)
            .Map(dest => dest.Name, src => src.Name);

        
        config.NewConfig<IEnumerable<Product>, GetProductsDto>()
            .Map(dest => dest.Products, src => src.Select(x => x.Adapt<GetProductDto>()));
    }
}