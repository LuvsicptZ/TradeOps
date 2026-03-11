using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TradeOps.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedDemoTenantsUsersCustomers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Tenants",
                columns: new[] { "Id", "ABN", "BusinessName", "CreatedAt" },
                values: new object[,]
                {
                    { new Guid("a1111111-1111-4111-8111-111111111101"), "11111111111", "Acme Cleaning Pty Ltd", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("b2222222-2222-4222-8222-222222222202"), "22222222222", "Beta Maintenance Co", new DateTime(2025, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "Address", "Name", "Phone", "TenantId" },
                values: new object[,]
                {
                    { new Guid("e5555555-5555-4555-8555-555555555505"), "Sydney NSW", "仅租户 A 可见的客户", "0400000001", new Guid("a1111111-1111-4111-8111-111111111101") },
                    { new Guid("f6666666-6666-4666-8666-666666666606"), "Melbourne VIC", "仅租户 B 可见的客户", "0400000002", new Guid("b2222222-2222-4222-8222-222222222202") }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "PasswordHash", "Role", "TenantId" },
                values: new object[,]
                {
                    { new Guid("c3333333-3333-4333-8333-333333333303"), "admin@acme.demo", "$2a$11$QySpUPrhdV9dQ5HEFA9dv..Ou2rlJLQQYNRVUpF3Pc//YqpRvT/IW", "Admin", new Guid("a1111111-1111-4111-8111-111111111101") },
                    { new Guid("d4444444-4444-4444-8444-444444444404"), "admin@beta.demo", "$2a$11$QySpUPrhdV9dQ5HEFA9dv..Ou2rlJLQQYNRVUpF3Pc//YqpRvT/IW", "Admin", new Guid("b2222222-2222-4222-8222-222222222202") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: new Guid("e5555555-5555-4555-8555-555555555505"));

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: new Guid("f6666666-6666-4666-8666-666666666606"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("c3333333-3333-4333-8333-333333333303"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d4444444-4444-4444-8444-444444444404"));

            migrationBuilder.DeleteData(
                table: "Tenants",
                keyColumn: "Id",
                keyValue: new Guid("a1111111-1111-4111-8111-111111111101"));

            migrationBuilder.DeleteData(
                table: "Tenants",
                keyColumn: "Id",
                keyValue: new Guid("b2222222-2222-4222-8222-222222222202"));
        }
    }
}
