using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Features.UserManagement.Queries.GetAllUsers;
using Mapster;

namespace Dotnet.Homeworks.Features.UserManagement.Mapping;

public class RegisterUserManagementMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<User, GetUserDto>()
            .Map(dest => dest.Guid, src => src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Email, src => src.Email);

        config.NewConfig<IQueryable<User>, GetAllUsersDto>()
            .Map(dest => dest.Users, src => src.Select(user => user.Adapt<GetUserDto>()));
    }
}