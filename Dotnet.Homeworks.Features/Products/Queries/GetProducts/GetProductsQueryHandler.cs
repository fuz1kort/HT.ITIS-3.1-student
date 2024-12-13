using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Features.Products.Mapping;
using Dotnet.Homeworks.Infrastructure.Cqrs.Queries;
using Dotnet.Homeworks.Shared.Dto;

namespace Dotnet.Homeworks.Features.Products.Queries.GetProducts;

internal sealed class GetProductsQueryHandler : IQueryHandler<GetProductsQuery, GetProductsDto>
{
    private readonly IProductRepository _repository;
    private IProductMapper _productMapper;

    public GetProductsQueryHandler(IProductRepository repository, IProductMapper productMapper)
    {
        _repository = repository;
        _productMapper = productMapper;
    }

    public async Task<Result<GetProductsDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var allProducts = await _repository.GetAllProductsAsync(cancellationToken);
            var dto = _productMapper.MapToGetProductsDto(allProducts);

            return ResultFactory.CreateResult<Result<GetProductsDto>>(true, value: dto);
        }
        catch (Exception ex)
        {
            return ResultFactory.CreateResult<Result<GetProductsDto>>(false, error: ex.Message);
        }
    }
}