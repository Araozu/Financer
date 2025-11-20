using Financer.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Financer.Infrastructure.Entities;

public class User : IdentityUser, IUser
{
    // IdentityUser already has Id, Email, UserName, etc.
    // We just need to satisfy IUser which is already done by IdentityUser
}
