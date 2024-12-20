﻿using Dotnet.Homeworks.Infrastructure.Cqrs.Queries;
using Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;

namespace Dotnet.Homeworks.Features.Users.Queries.GetUser;

public class GetUserQuery : IClientRequest, IQuery<GetUserDto>
{
    public Guid Guid { get; init; }

    public GetUserQuery(Guid guid)
    {
        Guid = guid;
    }
};