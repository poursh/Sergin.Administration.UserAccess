using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sergin.Administration.UserAccess.Infrastructure.Data.Migrations;

/// <summary>
/// The whole <c>ua</c> schema in one migration, plus the rows every environment needs before anyone can sign in: the
/// scope tree's single root and the two built-in roles, owned by it and inheritable. Ids are fixed, so the same role is
/// the same row everywhere. Audit stamps use the migration time and the platform's fixed system actor (the outbox relay
/// identity's id).
/// </summary>
public partial class InitialCreate : Migration
{
    private const string RootId = "01920000-0000-7000-8000-0000000000b1";
    private const string AdministratorRoleId = "01920000-0000-7000-8000-0000000000a1";
    private const string ViewerRoleId = "01920000-0000-7000-8000-0000000000a2";
    private const string SystemActor = "01920000-0000-7000-8000-00000000000f";

    // Only this module's own permissions: the migration must not depend on which other modules a host composes. A
    // host grants the viewer its modules' reads itself (the Compose dev seed does it for DeviceManagement).
    private static readonly string[] ViewerPermissions =
    [
        "permission.ua.users.read",
    ];

    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            name: "ua");

        migrationBuilder.AlterDatabase()
            .Annotation("Npgsql:PostgresExtension:ltree", ",,");

        migrationBuilder.CreateTable(
            name: "inbox_messages",
            schema: "ua",
            columns: table => new
            {
                message_id = table.Column<Guid>(type: "uuid", nullable: false),
                handler = table.Column<string>(type: "text", nullable: false),
                processed_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_inbox_messages", x => new { x.message_id, x.handler });
            });

        migrationBuilder.CreateTable(
            name: "outbox_messages",
            schema: "ua",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                type = table.Column<string>(type: "text", nullable: false),
                content = table.Column<string>(type: "jsonb", nullable: false),
                occurred_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                correlation_id = table.Column<string>(type: "text", nullable: false),
                causation_id = table.Column<Guid>(type: "uuid", nullable: true),
                attempts = table.Column<int>(type: "integer", nullable: false),
                next_attempt_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                processed_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                error = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_outbox_messages", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "scopes",
            schema: "ua",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                parent_id = table.Column<Guid>(type: "uuid", nullable: true),
                path = table.Column<string>(type: "ltree", nullable: false),
                created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                created_by = table.Column<Guid>(type: "uuid", nullable: false),
                deleted_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                deleted_by = table.Column<Guid>(type: "uuid", nullable: true),
                modified_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                modified_by = table.Column<Guid>(type: "uuid", nullable: true),
                row_version = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_scopes", x => x.id);
                table.CheckConstraint("ck_scopes_soft_delete", "(deleted_at_utc IS NULL) = (deleted_by IS NULL)");
                table.ForeignKey(
                    name: "fk_scopes_scopes_parent_id",
                    column: x => x.parent_id,
                    principalSchema: "ua",
                    principalTable: "scopes",
                    principalColumn: "id");
            });

        migrationBuilder.CreateTable(
            name: "users",
            schema: "ua",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                user_name = table.Column<string>(type: "text", nullable: false),
                is_active = table.Column<bool>(type: "boolean", nullable: false),
                external_id = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                email = table.Column<string>(type: "character varying(320)", maxLength: 320, nullable: true),
                first_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                last_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_users", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "roles",
            schema: "ua",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                owner_scope_id = table.Column<Guid>(type: "uuid", nullable: false),
                is_inheritable = table.Column<bool>(type: "boolean", nullable: false),
                created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                created_by = table.Column<Guid>(type: "uuid", nullable: false),
                deleted_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                deleted_by = table.Column<Guid>(type: "uuid", nullable: true),
                modified_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                modified_by = table.Column<Guid>(type: "uuid", nullable: true),
                row_version = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_roles", x => x.id);
                table.CheckConstraint("ck_roles_soft_delete", "(deleted_at_utc IS NULL) = (deleted_by IS NULL)");
                table.ForeignKey(
                    name: "fk_roles_scopes_owner_scope_id",
                    column: x => x.owner_scope_id,
                    principalSchema: "ua",
                    principalTable: "scopes",
                    principalColumn: "id");
            });

        migrationBuilder.CreateTable(
            name: "role_assignments",
            schema: "ua",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                role_id = table.Column<Guid>(type: "uuid", nullable: false),
                scope_id = table.Column<Guid>(type: "uuid", nullable: false),
                include_subtree = table.Column<bool>(type: "boolean", nullable: false),
                valid_from_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                valid_until_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                revoked_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                revoked_by = table.Column<Guid>(type: "uuid", nullable: true),
                user_id = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_role_assignments", x => x.id);
                table.ForeignKey(
                    name: "fk_role_assignments_roles_role_id",
                    column: x => x.role_id,
                    principalSchema: "ua",
                    principalTable: "roles",
                    principalColumn: "id");
                table.ForeignKey(
                    name: "fk_role_assignments_scopes_scope_id",
                    column: x => x.scope_id,
                    principalSchema: "ua",
                    principalTable: "scopes",
                    principalColumn: "id");
                table.ForeignKey(
                    name: "fk_role_assignments_users_user_id",
                    column: x => x.user_id,
                    principalSchema: "ua",
                    principalTable: "users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "role_permissions",
            schema: "ua",
            columns: table => new
            {
                permission = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                role_id = table.Column<Guid>(type: "uuid", nullable: false),
                created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                created_by = table.Column<Guid>(type: "uuid", nullable: false),
                modified_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                modified_by = table.Column<Guid>(type: "uuid", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_role_permissions", x => new { x.role_id, x.permission });
                table.ForeignKey(
                    name: "fk_role_permissions_roles_role_id",
                    column: x => x.role_id,
                    principalSchema: "ua",
                    principalTable: "roles",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "ix_outbox_messages_unprocessed",
            schema: "ua",
            table: "outbox_messages",
            column: "id",
            filter: "processed_on_utc IS NULL");

        migrationBuilder.CreateIndex(
            name: "ix_role_assignments_role_id",
            schema: "ua",
            table: "role_assignments",
            column: "role_id");

        migrationBuilder.CreateIndex(
            name: "ix_role_assignments_scope_id",
            schema: "ua",
            table: "role_assignments",
            column: "scope_id");

        migrationBuilder.CreateIndex(
            name: "ix_role_assignments_user_id",
            schema: "ua",
            table: "role_assignments",
            column: "user_id",
            filter: "revoked_at_utc IS NULL");

        migrationBuilder.CreateIndex(
            name: "ix_roles_owner_scope_id_name",
            schema: "ua",
            table: "roles",
            columns: ["owner_scope_id", "name"],
            unique: true,
            filter: "deleted_at_utc IS NULL");

        migrationBuilder.CreateIndex(
            name: "ix_scopes_parent_id_name",
            schema: "ua",
            table: "scopes",
            columns: ["parent_id", "name"],
            unique: true,
            filter: "deleted_at_utc IS NULL");

        migrationBuilder.CreateIndex(
            name: "ix_scopes_path",
            schema: "ua",
            table: "scopes",
            column: "path")
            .Annotation("Npgsql:IndexMethod", "gist");

        migrationBuilder.CreateIndex(
            name: "ix_users_external_id",
            schema: "ua",
            table: "users",
            column: "external_id",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "ix_users_user_name",
            schema: "ua",
            table: "users",
            column: "user_name",
            unique: true);

        Seed(migrationBuilder);
    }

    private static void Seed(MigrationBuilder migrationBuilder)
    {
        // The single root (Scope.RootId). The tree has one, and nothing in the application creates another. Raw SQL
        // because the path is an ltree: the root's id without dashes.
        migrationBuilder.Sql(
            $"""
            INSERT INTO ua.scopes (id, name, parent_id, path, created_at_utc, created_by, row_version)
            VALUES ('{RootId}', 'Root', NULL, replace('{RootId}', '-', '')::ltree, now(), '{SystemActor}', gen_random_uuid());
            """);

        // Without at least one role a just-provisioned user signs in and then sees nothing, with no way inside the
        // product to grant themselves anything. New users get viewer at the root; the dev user gets administrator.
        InsertRole(migrationBuilder, AdministratorRoleId, "administrator");
        InsertRole(migrationBuilder, ViewerRoleId, "viewer");

        // One statement per row, as InsertData would be (CA1814 rejects its rectangular values array), but in SQL so
        // the audit stamp is the database's now().
        GrantPermission(migrationBuilder, AdministratorRoleId, "permission.sys.platform-all");

        foreach (string permission in ViewerPermissions)
        {
            GrantPermission(migrationBuilder, ViewerRoleId, permission);
        }
    }

    private static void InsertRole(MigrationBuilder migrationBuilder, string id, string name) =>
        migrationBuilder.Sql(
            $"""
            INSERT INTO ua.roles (id, name, owner_scope_id, is_inheritable, created_at_utc, created_by, row_version)
            VALUES ('{id}', '{name}', '{RootId}', true, now(), '{SystemActor}', gen_random_uuid());
            """);

    private static void GrantPermission(MigrationBuilder migrationBuilder, string roleId, string permission) =>
        migrationBuilder.Sql(
            $"""
            INSERT INTO ua.role_permissions (role_id, permission, created_at_utc, created_by)
            VALUES ('{roleId}', '{permission}', now(), '{SystemActor}');
            """);

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "inbox_messages",
            schema: "ua");

        migrationBuilder.DropTable(
            name: "outbox_messages",
            schema: "ua");

        migrationBuilder.DropTable(
            name: "role_assignments",
            schema: "ua");

        migrationBuilder.DropTable(
            name: "role_permissions",
            schema: "ua");

        migrationBuilder.DropTable(
            name: "users",
            schema: "ua");

        migrationBuilder.DropTable(
            name: "roles",
            schema: "ua");

        migrationBuilder.DropTable(
            name: "scopes",
            schema: "ua");
    }
}
