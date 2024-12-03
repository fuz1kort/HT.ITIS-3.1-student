using Dotnet.Homeworks.Data.DatabaseContext;
using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dotnet.Homeworks.DataAccess.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _dbContext;

    public ProductRepository(AppDbContext dbContext) => _dbContext = dbContext;

    public async Task<IEnumerable<Product>> GetAllProductsAsync(CancellationToken cancellationToken)
        => await _dbContext.Products
            .AsNoTracking()
            .ToListAsync(cancellationToken);

    public async Task DeleteProductByGuidAsync(Guid id, CancellationToken cancellationToken)
    {
        var product = await _dbContext.Products
                          .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
                      ?? throw new ApplicationException("Не найден пользователь");

        _dbContext.Products.Remove(product);
    }

    public Task UpdateProductAsync(Product product, CancellationToken cancellationToken)
    {
        _dbContext.Products.Update(product);
        return Task.CompletedTask;
    }

    public async Task<Product?> GetProductByIdAsync(Guid id, CancellationToken cancellationToken)
        => await _dbContext.Products
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<Guid> InsertProductAsync(Product product, CancellationToken cancellationToken)
    {
        await _dbContext.Products.AddAsync(product, cancellationToken);
        return product.Id;
    }
}