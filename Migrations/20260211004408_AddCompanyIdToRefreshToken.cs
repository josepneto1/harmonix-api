using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Harmonix.Migrations
{
    /// <inheritdoc />
    public partial class AddCompanyIdToRefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ux_refresh_tokens_token",
                table: "refresh_tokens");

            migrationBuilder.RenameIndex(
                name: "ux_users_email",
                table: "users",
                newName: "idx_users_email");

            migrationBuilder.RenameIndex(
                name: "ix_refresh_tokens_user_id",
                table: "refresh_tokens",
                newName: "idx_refresh_tokens_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_refresh_tokens_expires_at",
                table: "refresh_tokens",
                newName: "idx_refresh_tokens_expires_at");

            migrationBuilder.RenameIndex(
                name: "ux_companies_alias",
                table: "companies",
                newName: "idx_companies_alias");

            migrationBuilder.AddColumn<Guid>(
                name: "company_id",
                table: "refresh_tokens",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "idx_refresh_tokens_company_id",
                table: "refresh_tokens",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "idx_refresh_tokens_token_company",
                table: "refresh_tokens",
                columns: new[] { "token", "company_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "idx_refresh_tokens_company_id",
                table: "refresh_tokens");

            migrationBuilder.DropIndex(
                name: "idx_refresh_tokens_token_company",
                table: "refresh_tokens");

            migrationBuilder.DropColumn(
                name: "company_id",
                table: "refresh_tokens");

            migrationBuilder.RenameIndex(
                name: "idx_users_email",
                table: "users",
                newName: "ux_users_email");

            migrationBuilder.RenameIndex(
                name: "idx_refresh_tokens_user_id",
                table: "refresh_tokens",
                newName: "ix_refresh_tokens_user_id");

            migrationBuilder.RenameIndex(
                name: "idx_refresh_tokens_expires_at",
                table: "refresh_tokens",
                newName: "ix_refresh_tokens_expires_at");

            migrationBuilder.RenameIndex(
                name: "idx_companies_alias",
                table: "companies",
                newName: "ux_companies_alias");

            migrationBuilder.CreateIndex(
                name: "ux_refresh_tokens_token",
                table: "refresh_tokens",
                column: "token",
                unique: true);
        }
    }
}
