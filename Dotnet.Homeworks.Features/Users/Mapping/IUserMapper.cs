using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Features.Users.Commands.CreateUser;
using Dotnet.Homeworks.Features.Users.Queries.GetUser;
using Dotnet.Homeworks.Infrastructure.Dto;
using Dotnet.Homeworks.Infrastructure.Utils;
using Mapster;

namespace Dotnet.Homeworks.Features.Users.Mapping;

public interface IUserMapper: IMapper
{
    User MapToUser(CreateUserCommand command);
    RegisterUserDto MapToRegisterUserDto(CreateUserCommand command);
    CreateUserDto MapToCreateUserDto(Guid id);
    GetUserDto MapToGetUserDto(User user);
}

public class UserMapper : IUserMapper
{
    public User MapToUser(CreateUserCommand command)
    {
        var config = new TypeAdapterConfig();

        config.NewConfig<CreateUserCommand, User>()
            .Map(dest => dest.Id, src => Guid.NewGuid())
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Email, src => src.Email);

        return command.Adapt<User>(config);
    }

    public RegisterUserDto MapToRegisterUserDto(CreateUserCommand command)
    {
        return command.Adapt<RegisterUserDto>();
    }

    public CreateUserDto MapToCreateUserDto(Guid id)
    {
        return id.Adapt<CreateUserDto>();
    }

    public GetUserDto MapToGetUserDto(User user)
    {
        return user.Adapt<GetUserDto>();
    }
}