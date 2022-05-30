using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class AddUserTicket : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Ticket_TicketId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_TicketId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "InventoryId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TicketId",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "TicketUser",
                columns: table => new
                {
                    TicketsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketUser", x => new { x.TicketsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_TicketUser_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketUser_Ticket_TicketsId",
                        column: x => x.TicketsId,
                        principalTable: "Ticket",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TicketUser_UsersId",
                table: "TicketUser",
                column: "UsersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TicketUser");

            migrationBuilder.AddColumn<int>(
                name: "InventoryId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "TicketId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TicketId",
                table: "AspNetUsers",
                column: "TicketId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Ticket_TicketId",
                table: "AspNetUsers",
                column: "TicketId",
                principalTable: "Ticket",
                principalColumn: "Id");
        }
    }
}
