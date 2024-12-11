using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;
using Dotnet.Homeworks.Infrastructure.Dto;
using Dotnet.Homeworks.Infrastructure.Services;
using Dotnet.Homeworks.Infrastructure.UnitOfWork;
using Dotnet.Homeworks.Shared.Dto;

namespace Dotnet.Homeworks.Features.Users.Commands.CreateUser;

public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, CreateUserDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRegistrationService _registrationService;

    public CreateUserCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IRegistrationService registrationService
    )
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _registrationService = registrationService;
    }

    public async Task<Result<CreateUserDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = new User { Email = request.Email, Name = request.Name };
            var id = await _userRepository.InsertUserAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _registrationService.RegisterAsync(new RegisterUserDto(request.Name, request.Email));

            return new Result<CreateUserDto>(new CreateUserDto(id), true);
        }
        catch (Exception ex)
        {
            return new Result<CreateUserDto>(default, false, ex.Message);
        }
    }
}