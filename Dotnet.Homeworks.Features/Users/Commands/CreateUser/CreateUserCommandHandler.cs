﻿using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Features.Users.Mapping;
using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;
using Dotnet.Homeworks.Infrastructure.Services;
using Dotnet.Homeworks.Infrastructure.UnitOfWork;
using Dotnet.Homeworks.Infrastructure.Validation.Decorators;
using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;
using Dotnet.Homeworks.Shared.Dto;
using FluentValidation;

namespace Dotnet.Homeworks.Features.Users.Commands.CreateUser;

public class CreateUserCommandHandler :
    CqrsDecorator<CreateUserCommand, Result<CreateUserDto>>,
    ICommandHandler<CreateUserCommand, CreateUserDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRegistrationService _registrationService;
    private readonly IUserMapper _userMapper;

    public CreateUserCommandHandler(
        IEnumerable<IValidator<CreateUserCommand>> validators,
        IPermissionCheck permissionCheck,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IRegistrationService registrationService, IUserMapper userMapper) : base(validators, permissionCheck)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _registrationService = registrationService;
        _userMapper = userMapper;
    }

    public override async Task<Result<CreateUserDto>> Handle(CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        var res = await base.Handle(request, cancellationToken);
        if (res.IsFailure)
        {
            return res;
        }

        try
        {
            var user = _userMapper.MapToUser(request);
            var id = await _userRepository.InsertUserAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _registrationService.RegisterAsync(_userMapper.MapToRegisterUserDto(request));
            var dto = _userMapper.MapToCreateUserDto(id);

            return ResultFactory.CreateResult<Result<CreateUserDto>>(true, value: dto);
        }
        catch (Exception ex)
        {
            return ResultFactory.CreateResult<Result<CreateUserDto>>(true, error: ex.Message);
        }
    }
}