using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RarePuppers.Migrations
{
    public partial class updated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AttributeTypes",
                columns: table => new
                {
                    attribute_type_id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttributeTypes", x => x.attribute_type_id);
                });

            migrationBuilder.CreateTable(
                name: "HomeTypes",
                columns: table => new
                {
                    home_type_id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    capacity = table.Column<int>(nullable: false),
                    home_image_src = table.Column<string>(nullable: true),
                    name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeTypes", x => x.home_type_id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    role_id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    role_name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.role_id);
                });

            migrationBuilder.CreateTable(
                name: "Attributes",
                columns: table => new
                {
                    attribute_id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    attribute_type_id = table.Column<int>(nullable: false),
                    image_src = table.Column<string>(nullable: true),
                    name = table.Column<string>(nullable: true),
                    rarity = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attributes", x => x.attribute_id);
                    table.ForeignKey(
                        name: "FK_Attributes_AttributeTypes_attribute_type_id",
                        column: x => x.attribute_type_id,
                        principalTable: "AttributeTypes",
                        principalColumn: "attribute_type_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    user_id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    hashedPassword = table.Column<string>(nullable: true),
                    role_id = table.Column<int>(nullable: false),
                    username = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_role_id",
                        column: x => x.role_id,
                        principalTable: "Roles",
                        principalColumn: "role_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Homes",
                columns: table => new
                {
                    home_id = table.Column<int>(nullable: false),
                    user_id = table.Column<int>(nullable: false),
                    home_type_id = table.Column<int>(nullable: false),
                    name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Homes", x => new { x.home_id, x.user_id });
                    table.ForeignKey(
                        name: "FK_Homes_HomeTypes_home_type_id",
                        column: x => x.home_type_id,
                        principalTable: "HomeTypes",
                        principalColumn: "home_type_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Homes_Users_user_id",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Puppers",
                columns: table => new
                {
                    pupper_id = table.Column<int>(nullable: false),
                    user_id = table.Column<int>(nullable: false),
                    color = table.Column<int>(nullable: false),
                    ears = table.Column<int>(nullable: false),
                    eyes = table.Column<int>(nullable: false),
                    tail = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Puppers", x => new { x.pupper_id, x.user_id });
                    table.ForeignKey(
                        name: "FK_Puppers_Users_user_id",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attributes_attribute_type_id",
                table: "Attributes",
                column: "attribute_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_Homes_home_type_id",
                table: "Homes",
                column: "home_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_Homes_user_id",
                table: "Homes",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Puppers_user_id",
                table: "Puppers",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_role_id",
                table: "Users",
                column: "role_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attributes");

            migrationBuilder.DropTable(
                name: "Homes");

            migrationBuilder.DropTable(
                name: "Puppers");

            migrationBuilder.DropTable(
                name: "AttributeTypes");

            migrationBuilder.DropTable(
                name: "HomeTypes");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
