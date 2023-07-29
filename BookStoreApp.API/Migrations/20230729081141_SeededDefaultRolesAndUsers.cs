using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStoreApp.API.Migrations
{
    public partial class SeededDefaultRolesAndUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0720f3b6-7d35-4239-a9fc-c0f8beb01c3a", "022621e7-a6e3-49bf-a2a3-7069201d9358", "Administrator", "ADMINISTRATOR" },
                    { "23cb4de6-901e-41f0-9c37-811f0219e91b", "b0d4ca36-b1fd-463a-8936-7e9b7fb7061c", "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "6d887c8c-d649-4f11-a84f-0ca02a003f07", 0, "06726b9f-93b5-41a0-9fbd-7f92ad8db5ae", "admin@bookstore.com", false, "System", "Admin", false, null, "ADMIN@BOOKSTORE.COM", "ADMIN@BOOKSTORE.COM", "AQAAAAEAACcQAAAAEDGKt2nMVG4ytrf9h5mXKsX8CRw7rIfc7s4xiLLdIMU5i9G4/gKLchNKswzD+gg+yw==", null, false, "ff857d49-9432-458d-9738-05ff5a354264", false, "admin@bookstore.com" },
                    { "e58f3f49-0be4-4449-b304-71c4cb6e406d", 0, "e8193107-ab44-4563-9779-60b0b5063af8", "user@bookstore.com", false, "System", "User", false, null, "USER@BOOKSTORE.COM", "USER@BOOKSTORE.COM", "AQAAAAEAACcQAAAAEK62HY7n7S37wwFExIJcDWQD4hZ5+MJXPn+xhZ4a2nlffQ05aTOhzWlKIWAPBgv9Eg==", null, false, "13227119-05a1-4775-aeef-97a6f2364767", false, "user@bookstore.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "0720f3b6-7d35-4239-a9fc-c0f8beb01c3a", "6d887c8c-d649-4f11-a84f-0ca02a003f07" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "23cb4de6-901e-41f0-9c37-811f0219e91b", "e58f3f49-0be4-4449-b304-71c4cb6e406d" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "0720f3b6-7d35-4239-a9fc-c0f8beb01c3a", "6d887c8c-d649-4f11-a84f-0ca02a003f07" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "23cb4de6-901e-41f0-9c37-811f0219e91b", "e58f3f49-0be4-4449-b304-71c4cb6e406d" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0720f3b6-7d35-4239-a9fc-c0f8beb01c3a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "23cb4de6-901e-41f0-9c37-811f0219e91b");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6d887c8c-d649-4f11-a84f-0ca02a003f07");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "e58f3f49-0be4-4449-b304-71c4cb6e406d");
        }
    }
}
