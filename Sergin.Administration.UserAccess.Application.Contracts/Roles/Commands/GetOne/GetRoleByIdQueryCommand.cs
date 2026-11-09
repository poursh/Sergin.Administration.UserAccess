using Sergin.SharedKernel.Application.Commands.Queries;
using Sergin.Administration.UserAccess.Domain.Roles;

namespace Sergin.Administration.UserAccess.Application.Contracts.Roles.Commands.GetOne;

public sealed record GetRoleByIdQueryCommand(RoleId Id) : IQuery<RoleQueryResponse>, IGetOneQuery<Role, RoleId>;
