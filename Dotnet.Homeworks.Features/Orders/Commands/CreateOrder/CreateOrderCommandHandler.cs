using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Features.Helpers;
using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;
using Dotnet.Homeworks.Shared.Dto;
using Microsoft.AspNetCore.Http;

namespace Dotnet.Homeworks.Features.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, CreateOrderDto>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUserRepository _userRepository;
    private readonly HttpContext _httpContext;

    public CreateOrderCommandHandler(
        IOrderRepository orderRepository,
        IUserRepository userRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        _orderRepository = orderRepository;
        _userRepository = userRepository;
        _httpContext = httpContextAccessor.HttpContext!;
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

            var order = new Order
            {
                OrdererId = user.Id,
                ProductsIds = request.ProductsIds
            };

            var id = await _orderRepository.InsertOrderAsync(order, cancellationToken);
            var dto = new CreateOrderDto(id);
            return ResultFactory.CreateResult<Result<CreateOrderDto>>(true, value: dto);
        }
        catch (Exception ex)
        {
            return ResultFactory.CreateResult<Result<CreateOrderDto>>(false, error: ex.Message);
        }
    }
}