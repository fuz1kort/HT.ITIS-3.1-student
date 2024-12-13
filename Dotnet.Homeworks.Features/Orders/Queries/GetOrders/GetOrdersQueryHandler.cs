using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Features.Helpers;
using Dotnet.Homeworks.Features.Orders.Mapping;
using Dotnet.Homeworks.Infrastructure.Cqrs.Queries;
using Dotnet.Homeworks.Shared.Dto;
using Microsoft.AspNetCore.Http;

namespace Dotnet.Homeworks.Features.Orders.Queries.GetOrders;

public class GetOrdersQueryHandler : IQueryHandler<GetOrdersQuery, GetOrdersDto>
{
    private readonly IOrderRepository _orderRepository;
    private readonly HttpContext _httpContext;
    private IOrderMapper _orderMapper;

    public GetOrdersQueryHandler(
        IOrderRepository orderRepository,
        IHttpContextAccessor httpContextAccessor, IOrderMapper orderMapper)
    {
        _orderRepository = orderRepository;
        _orderMapper = orderMapper;
        _httpContext = httpContextAccessor.HttpContext!;
    }

    public async Task<Result<GetOrdersDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = _httpContext.User.GetUserId();
            if (userId == null)
            {
                return ResultFactory.CreateResult<Result<GetOrdersDto>>(false, error: "User is not logged in");
            }

            var orders = await _orderRepository.GetAllOrdersFromUserAsync(userId.Value, cancellationToken);
            var dto = _orderMapper.MapToGetOrdersDto(orders);
            return ResultFactory.CreateResult<Result<GetOrdersDto>>(true, value: dto);
        }
        catch (Exception ex)
        {
            return ResultFactory.CreateResult<Result<GetOrdersDto>>(false, error: ex.Message);
        }
    }
}