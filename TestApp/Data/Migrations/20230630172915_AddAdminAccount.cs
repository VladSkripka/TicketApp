using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Text;

#nullable disable

namespace TestApp.Data.Migrations
{
    public partial class AddAdminAccount : Migration
    {
        const string ADMIN_USER_GUID = "025693f3-62e5-4bb4-ab3a-72d8e72296f1";
        const string ADMIN_ROLE_GUID = "31186356-6deb-456a-9509-bfd0b1f99b86";

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //var hasher = new 
            var password = "adminuser";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("INSERT INTO AspNetUsers(Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PAsswordHash, " +
                "SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount)");
            sb.AppendLine("VALUES(");
            sb.AppendLine($"'{ADMIN_USER_GUID}'");
            sb.AppendLine(", 'admin@gmail.com'");
            sb.AppendLine(", 'ADMIN@GMAIL.COM'");
            sb.AppendLine(", 'admin@gmail.com'");
            sb.AppendLine(", 'ADMIN@GMAIL.COM'");
            sb.AppendLine(", 1");
            sb.AppendLine($", '{password}'");
            sb.AppendLine(", ''");
            sb.AppendLine(", ''");
            sb.AppendLine(", 0");
            sb.AppendLine(", 0");
            sb.AppendLine(", 0");
            sb.AppendLine(", ''");
            sb.AppendLine(", ''");
            sb.AppendLine(", 0");
            sb.AppendLine(")");

            migrationBuilder.Sql(sb.ToString());
            migrationBuilder.Sql($"INSERT INTO AspNetRoles (Id, Name, Normalizedname) VALUES('{ADMIN_ROLE_GUID}', 'Admin', 'ADMIN')");
            migrationBuilder.Sql($"INSERT INTO AspNetUserRoles (UserId, RoleId) VALUES('{ADMIN_USER_GUID}','{ADMIN_ROLE_GUID}')");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"DELETE FROM AspNetUserRoles WHERE UserId = '{ADMIN_USER_GUID}' AND RoleId = '{ADMIN_ROLE_GUID}'");
            migrationBuilder.Sql($"DELETE FROM AspNetUsers WHERE Id = '{ADMIN_USER_GUID}'");
            migrationBuilder.Sql($"DELETE FROM AspNetRoles WHERE Id = '{ADMIN_ROLE_GUID}'");
        }
    }
}
