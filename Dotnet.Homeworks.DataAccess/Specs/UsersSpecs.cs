using Dotnet.Homeworks.DataAccess.Specs.Infrastructure;
using Dotnet.Homeworks.Domain.Entities;

namespace Dotnet.Homeworks.DataAccess.Specs;

public class UsersSpecs : IUsersSpecs
{
    public Specification<User> HasGoogleEmail() => new(user => user.Email.EndsWith("@gmail.com"));

    public Specification<User> HasYandexEmail() => new(user => user.Email.EndsWith("@yandex.ru"));

    public Specification<User> HasMailEmail() => new(user => user.Email.EndsWith("@mail.ru"));

    public Specification<User> HasPopularEmailVendor() =>
        HasGoogleEmail() | HasYandexEmail() | HasMailEmail();

    public Specification<User> HasLongName() => new(user => user.Name.Length > 15);

    public Specification<User> HasCompositeNameWithWhitespace() => new(user => user.Name.Contains(" "));

    public Specification<User> HasCompositeNameWithHyphen() => new(user => user.Name.Contains("-"));

    public Specification<User> HasCompositeName() =>
        HasCompositeNameWithWhitespace() | (HasCompositeNameWithHyphen());

    public Specification<User> HasComplexName() =>
        HasLongName() & (HasCompositeName());
}