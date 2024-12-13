using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Features.Products.Commands.InsertProduct;
using Dotnet.Homeworks.Features.Products.Commands.UpdateProduct;
using Dotnet.Homeworks.Features.Products.Queries.GetProducts;
using Dotnet.Homeworks.Infrastructure.Utils;
using Mapster;

namespace Dotnet.Homeworks.Features.Products.Mapping;

public interface IProductMapper: IMapper
{
    Product MapToProduct(InsertProductCommand command);
    InsertProductDto MapToInsertProductDto(Guid id);
    Product MapToProduct(UpdateProductCommand command);
    GetProductsDto MapToGetProductsDto(IEnumerable<Product> products);
}

public class ProductMapper : IProductMapper
{
    public Product MapToProduct(InsertProductCommand command)
    {
        return command.Adapt<Product>();
    }

    public InsertProductDto MapToInsertProductDto(Guid id)
    {
        return id.Adapt<InsertProductDto>();
    }

    public Product MapToProduct(UpdateProductCommand command)
    {
        return command.Adapt<Product>();
    }

    public GetProductsDto MapToGetProductsDto(IEnumerable<Product> products)
    {
        return products.Adapt<GetProductsDto>();
    }
}