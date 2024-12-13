using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Features.Users.Mapping;
using Dotnet.Homeworks.Infrastructure.Cqrs.Queries;
using Dotnet.Homeworks.Infrastructure.Validation.Decorators;
using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;
using Dotnet.Homeworks.Shared.Dto;
using FluentValidation;

namespace Dotnet.Homeworks.Features.Users.Queries.GetUser;

public class GetUserQueryHandler :
    CqrsDecorator<GetUserQuery, Result<GetUserDto>>,
    IQueryHandler<GetUserQuery, GetUserDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserMapper _userMapper;

    public GetUserQueryHandler(
        IEnumerable<IValidator<GetUserQuery>> validators,
        IPermissionCheck permissionCheck,
        IUserRepository userRepository, 
        IUserMapper userMapper) : base(validators, permissionCheck)
    {
        _userRepository = userRepository;
        _userMapper = userMapper;
    }

    public override async Task<Result<GetUserDto>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var res = await base.Handle(request, cancellationToken);
        if (res.IsFailure)
        {
            return res;
        }

        try
        {
            var user = await _userRepository.GetUserByGuidAsync(request.Guid, cancellationToken);

            if (user == null)
            {
                return ResultFactory.CreateResult<Result<GetUserDto>>(false, error: $"User with id {request.Guid} not found");
            }

            var dto = _userMapper.MapToGetUserDto(user);
            
            return ResultFactory.CreateResult<Result<GetUserDto>>(true, value: dto);
        }
        catch (Exception ex)
        {
            return ResultFactory.CreateResult<Result<GetUserDto>>(false, error: ex.Message);
        }
    }
}