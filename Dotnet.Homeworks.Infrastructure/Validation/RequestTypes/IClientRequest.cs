﻿using Dotnet.Homeworks.Infrastructure.Validation.RequestTypes.Base;

namespace Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;

public interface IClientRequest : ISecureRequest
{
    public Guid Guid { get; }
}