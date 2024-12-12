using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;
using Dotnet.Homeworks.Infrastructure.UnitOfWork;
using Dotnet.Homeworks.Infrastructure.Validation.Decorators;
using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;
using Dotnet.Homeworks.Shared.Dto;
using FluentValidation;

namespace Dotnet.Homeworks.Features.Users.Commands.DeleteUser;

public sealed class DeleteUserCommandHandler : 
    CqrsDecorator<DeleteUserCommand, Result>, 
    ICommandHandler<DeleteUserCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteUserCommandHandler(
        IEnumerable<IValidator<DeleteUserCommand>> validators,
        IPermissionCheck permissionCheck,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork
    ) : base(validators, permissionCheck)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public override async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
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
                return ResultFactory.CreateResult<Result>(false, error: $"User with id {request.Guid} not found");
            }
            
            await _userRepository.DeleteUserByGuidAsync(request.Guid, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return ResultFactory.CreateResult<Result>(true);
        }
        catch (Exception ex)
        {
            return ResultFactory.CreateResult<Result>(false, error: ex.Message);
        }
    }
}