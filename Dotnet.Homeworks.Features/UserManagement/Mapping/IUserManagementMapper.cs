using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Features.UserManagement.Queries.GetAllUsers;
using Dotnet.Homeworks.Infrastructure.Utils;
using Mapster;

namespace Dotnet.Homeworks.Features.UserManagement.Mapping;

public interface IUserManagementMapper : IMapper
{
    GetAllUsersDto MapToGetAllUsersDto(IQueryable<User> users);
}

public class UserManagementMapper : IUserManagementMapper
{
    public GetAllUsersDto MapToGetAllUsersDto(IQueryable<User> users)
    {
        return users.Adapt<GetAllUsersDto>();
    }
}