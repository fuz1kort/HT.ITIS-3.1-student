using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Infrastructure.UnitOfWork;
using Dotnet.Homeworks.MainProject.Controllers;
using Dotnet.Homeworks.Mediator;

namespace Dotnet.Homeworks.Tests.Cqrs.Helpers;

internal class CqrsEnvironment
{
    public CqrsEnvironment(ProductManagementController productManagementController, IUnitOfWork unitOfWorkMock,
        MediatR.IMediator mediatR, IMediator customMediator,
        IUserRepository userRepository)
    {
        ProductManagementController = productManagementController;
        CustomMediator = customMediator;
        UserRepository = userRepository;
        MediatR = mediatR;
        UnitOfWorkMock = unitOfWorkMock;
    }

    public ProductManagementController ProductManagementController { get; }
    public IUnitOfWork UnitOfWorkMock { get; }
    public MediatR.IMediator MediatR { get; }
    public IMediator CustomMediator { get; }
    public IUserRepository UserRepository { get; }
}