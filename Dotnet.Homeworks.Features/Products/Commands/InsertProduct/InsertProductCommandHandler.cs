using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Features.Products.Mapping;
using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;
using Dotnet.Homeworks.Infrastructure.UnitOfWork;
using Dotnet.Homeworks.Shared.Dto;

namespace Dotnet.Homeworks.Features.Products.Commands.InsertProduct;

internal sealed class InsertProductCommandHandler : ICommandHandler<InsertProductCommand, InsertProductDto>
{
    private readonly IProductRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductMapper _productMapper;

    public InsertProductCommandHandler(IProductRepository repository,
        IUnitOfWork unitOfWork, IProductMapper productMapper)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _productMapper = productMapper;
    }

    public async Task<Result<InsertProductDto>> Handle(InsertProductCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var newProduct = _productMapper.MapToProduct(request);

            var productGuid = await _repository.InsertProductAsync(newProduct, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var dto = _productMapper.MapToInsertProductDto(productGuid);

            return ResultFactory.CreateResult<Result<InsertProductDto>>(true, value: dto
            );
        }
        catch (Exception ex)
        {
            return ResultFactory.CreateResult<Result<InsertProductDto>>(false, error: ex.Message);
        }
    }
}