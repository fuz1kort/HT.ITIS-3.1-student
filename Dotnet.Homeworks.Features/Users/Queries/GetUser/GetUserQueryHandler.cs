﻿using Dotnet.Homeworks.Domain.Abstractions.Repositories;
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

    public GetUserQueryHandler(
        IEnumerable<IValidator<GetUserQuery>> validators,
        IPermissionCheck permissionCheck,
        IUserRepository userRepository
    ) : base(validators, permissionCheck)
    {
        _userRepository = userRepository;
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