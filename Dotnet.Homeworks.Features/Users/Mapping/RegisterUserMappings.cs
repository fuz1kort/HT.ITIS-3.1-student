using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Features.Users.Commands.CreateUser;
using Dotnet.Homeworks.Features.Users.Queries.GetUser;
using Dotnet.Homeworks.Infrastructure.Dto;
using Mapster;

namespace Dotnet.Homeworks.Features.Users.Mapping;

public class RegisterUserMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateUserCommand, RegisterUserDto>()
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Email, src => src.Email);

        config.NewConfig<Guid, CreateUserDto>()
            .Map(dest => dest.Guid, src => src);
        
        config.NewConfig<User, GetUserDto>()
            .Map(dest => dest.Guid, src => src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Email, src => src.Email);
    }
}