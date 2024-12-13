using Dotnet.Homeworks.Infrastructure.Validation.RequestTypes.Base;

namespace Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;

public interface IOrderOwnerRequest : ISecureRequest
{
    public Guid OrderId { get; }
}