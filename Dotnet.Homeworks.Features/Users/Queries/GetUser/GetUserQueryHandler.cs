using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Infrastructure.Cqrs.Queries;
using Dotnet.Homeworks.Shared.Dto;

namespace Dotnet.Homeworks.Features.Users.Queries.GetUser;

public class GetUserQueryHandler : IQueryHandler<GetUserQuery, GetUserDto>
{
    private readonly IUserRepository _userRepository;

    public GetUserQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<GetUserDto>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userRepository.GetUserByGuidAsync(request.Guid, cancellationToken);
            
            return user == null 
                ? new Result<GetUserDto>(default, false, $"User with id {request.Guid} not found") 
                : new Result<GetUserDto>(new GetUserDto(user.Id, user.Name, user.Email), true);
        }
        catch (Exception ex)
        {
            return new Result<GetUserDto>(default, false, ex.Message);
        }
    }
}