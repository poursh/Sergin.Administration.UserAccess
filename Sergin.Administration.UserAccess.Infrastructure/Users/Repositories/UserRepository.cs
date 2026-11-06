using Microsoft.EntityFrameworkCore;
using Sergin.SharedKernel.Infrastructure.Data.EFCore.Repositories;
using Sergin.Administration.UserAccess.Domain.Roles;
using Sergin.Administration.UserAccess.Domain.Scopes;
using Sergin.Administration.UserAccess.Domain.Users;
using Sergin.Administration.UserAccess.Infrastructure.Data;

namespace Sergin.Administration.UserAccess.Infrastructure.Users.Repositories;

internal class UserRepository(IUserAccessDbContext dbContext)
    : EfRepository<User, UserInternalId>(dbContext), IUserRepository
{
    public Task<User?> GetByUserName(UserName userName, CancellationToken cancellationToken = default)
    {
        return Set.SingleOrDefaultAsync(u => u.UserName == userName, cancellationToken);
    }

    public Task<User?> GetByExternalId(ExternalUserId externalId, CancellationToken cancellationToken = default)
    {
        return Set.SingleOrDefaultAsync(u => u.ExternalId == externalId, cancellationToken);
    }

    public Task<User?> GetWithAssignmentsAsync(UserInternalId id, CancellationToken cancellationToken = default)
    {
        return Set.Include(u => u.Assignments).SingleOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyCollection<User>> GetWithLiveAssignmentsOfRoleAsync(Guid roleId, CancellationToken cancellationToken = default)
    {
        var role = new RoleId(roleId);

        return await Set
            .Include(u => u.Assignments)
            .Where(u => u.Assignments.Any(a => a.RoleId == role && a.RevokedAtUtc == null))
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<User>> GetWithLiveAssignmentsAtScopeAsync(Guid scopeId, CancellationToken cancellationToken = default)
    {
        var scope = new ScopeId(scopeId);

        return await Set
            .Include(u => u.Assignments)
            .Where(u => u.Assignments.Any(a => a.ScopeId == scope && a.RevokedAtUtc == null))
            .ToListAsync(cancellationToken);
    }

    public Task<bool> IsTakenAsync(UserName key, CancellationToken cancellationToken = default)
        => AnyAsync(u => u.UserName == key, cancellationToken);
}
