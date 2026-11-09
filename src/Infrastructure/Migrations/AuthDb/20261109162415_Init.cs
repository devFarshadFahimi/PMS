using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations.AuthDb
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Base");

            migrationBuilder.CreateTable(
                name: "TblDataProtectionKey",
                schema: "Base",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FriendlyName = table.Column<string>(type: "character varying(32767)", maxLength: 32767, nullable: true),
                    Xml = table.Column<string>(type: "character varying(32767)", maxLength: 32767, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblDataProtectionKey", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TblRole",
                schema: "Base",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TblUser",
                schema: "Base",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(32767)", maxLength: 32767, nullable: true),
                    SecurityStamp = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    PhoneNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TblRoleClaim",
                schema: "Base",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    ClaimType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ClaimValue = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblRoleClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TblRoleClaim_TblRole_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Base",
                        principalTable: "TblRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TblUserClaim",
                schema: "Base",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    ClaimType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ClaimValue = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblUserClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TblUserClaim_TblUser_UserId",
                        column: x => x.UserId,
                        principalSchema: "Base",
                        principalTable: "TblUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TblUserLogin",
                schema: "Base",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ProviderKey = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblUserLogin", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_TblUserLogin_TblUser_UserId",
                        column: x => x.UserId,
                        principalSchema: "Base",
                        principalTable: "TblUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TblUserRole",
                schema: "Base",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    Id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblUserRole", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_TblUserRole_TblRole_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Base",
                        principalTable: "TblRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TblUserRole_TblUser_UserId",
                        column: x => x.UserId,
                        principalSchema: "Base",
                        principalTable: "TblUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TblUserToken",
                schema: "Base",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ValidTo = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    LoginProvider = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Value = table.Column<string>(type: "character varying(32767)", maxLength: 32767, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblUserToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TblUserToken_TblUser_UserId",
                        column: x => x.UserId,
                        principalSchema: "Base",
                        principalTable: "TblUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "Base",
                table: "TblRole",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TblRoleClaim_RoleId",
                schema: "Base",
                table: "TblRoleClaim",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "Base",
                table: "TblUser",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "Base",
                table: "TblUser",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TblUserClaim_UserId",
                schema: "Base",
                table: "TblUserClaim",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TblUserLogin_UserId",
                schema: "Base",
                table: "TblUserLogin",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TblUserRole_RoleId",
                schema: "Base",
                table: "TblUserRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_TblUserToken_UserId",
                schema: "Base",
                table: "TblUserToken",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TblDataProtectionKey",
                schema: "Base");

            migrationBuilder.DropTable(
                name: "TblRoleClaim",
                schema: "Base");

            migrationBuilder.DropTable(
                name: "TblUserClaim",
                schema: "Base");

            migrationBuilder.DropTable(
                name: "TblUserLogin",
                schema: "Base");

            migrationBuilder.DropTable(
                name: "TblUserRole",
                schema: "Base");

            migrationBuilder.DropTable(
                name: "TblUserToken",
                schema: "Base");

            migrationBuilder.DropTable(
                name: "TblRole",
                schema: "Base");

            migrationBuilder.DropTable(
                name: "TblUser",
                schema: "Base");
        }
    }
}
