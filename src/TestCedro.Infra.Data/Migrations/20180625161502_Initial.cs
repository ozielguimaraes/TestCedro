using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TestCedro.Infra.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Restaurants",
                columns: table => new
                {
                    RestaurantId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restaurants", x => x.RestaurantId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    Email = table.Column<string>(maxLength: 50, nullable: false),
                    Password = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(maxLength: 30, nullable: false),
                    LastName = table.Column<string>(maxLength: 30, nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    IsApproved = table.Column<bool>(nullable: false),
                    PasswordFailuresSinceLastSuccess = table.Column<int>(nullable: false),
                    LastPasswordFailureDate = table.Column<DateTime>(nullable: true),
                    LastActivityDate = table.Column<DateTime>(nullable: true),
                    LastLockoutDate = table.Column<DateTime>(nullable: true),
                    LastLoginDate = table.Column<DateTime>(nullable: true),
                    ConfirmationToken = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: true),
                    IsLockedOut = table.Column<bool>(nullable: false),
                    LastPasswordChangedDate = table.Column<DateTime>(nullable: true),
                    PasswordVerificationToken = table.Column<string>(nullable: true),
                    PrivateKey = table.Column<string>(nullable: true),
                    PasswordVerificationTokenExpirationDate = table.Column<DateTime>(nullable: true),
                    PictureUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Dishs",
                columns: table => new
                {
                    DishId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Value = table.Column<decimal>(nullable: false),
                    RestaurantId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dishs", x => x.DishId);
                    table.ForeignKey(
                        name: "FK_Dishs_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "RestaurantId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dishs_RestaurantId",
                table: "Dishs",
                column: "RestaurantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Dishs");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Restaurants");
        }
    }
}
