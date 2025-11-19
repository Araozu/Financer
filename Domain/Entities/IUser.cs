namespace Financer.Domain.Entities;

public interface IUser
{
    string Id { get; }
    string? Email { get; }
    string? UserName { get; }
}
