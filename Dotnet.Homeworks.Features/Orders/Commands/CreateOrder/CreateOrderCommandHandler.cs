using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Features.Helpers;
using Dotnet.Homeworks.Features.Orders.Mapping;
using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;
using Dotnet.Homeworks.Shared.Dto;
using Microsoft.AspNetCore.Http;

namespace Dotnet.Homeworks.Features.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, CreateOrderDto>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUserRepository _userRepository;
    private readonly HttpContext _httpContext;
    private readonly IOrderMapper _orderMapper;

    public CreateOrderCommandHandler(
        IOrderRepository orderRepository,
        IUserRepository userRepository,
        IHttpContextAccessor httpContextAccessor,
        IOrderMapper orderMapper)
    {
        _orderRepository = orderRepository;
        _userRepository = userRepository;
        _httpContext = httpContextAccessor.HttpContext!;
        _orderMapper = orderMapper;
    }

    public async Task<Result<CreateOrderDto>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = _httpContext.User.GetUserId();
            if (userId is null)
            {
                return ResultFactory.CreateResult<Result<CreateOrderDto>>(false, error: "User is not logged in");
            }

            var user = await _userRepository.GetUserByGuidAsync(userId.Value, cancellationToken);
            if (user is null)
            {
                return ResultFactory.CreateResult<Result<CreateOrderDto>>(false, error: "User is not found");
            }

            var order = _orderMapper.MapToOrder(request, userId.Value);

            var id = await _orderRepository.InsertOrderAsync(order, cancellationToken);
            var dto = _orderMapper.MapToCreateOrderDto(id);
            
            return ResultFactory.CreateResult<Result<CreateOrderDto>>(true, value: dto);
        }
        catch (Exception ex)
        {
            return ResultFactory.CreateResult<Result<CreateOrderDto>>(false, error: ex.Message);
        }
    }
}