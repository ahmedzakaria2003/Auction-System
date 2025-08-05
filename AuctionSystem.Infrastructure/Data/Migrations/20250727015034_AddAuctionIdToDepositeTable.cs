using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuctionSystem.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAuctionIdToDepositeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AuctionId",
                table: "UserDeposites",
                type: "uniqueidentifier",
                nullable: true);
               

            migrationBuilder.CreateIndex(
                name: "IX_UserDeposites_AuctionId",
                table: "UserDeposites",
                column: "AuctionId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserDeposites_Auctions_AuctionId",
                table: "UserDeposites",
                column: "AuctionId",
                principalTable: "Auctions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserDeposites_Auctions_AuctionId",
                table: "UserDeposites");

            migrationBuilder.DropIndex(
                name: "IX_UserDeposites_AuctionId",
                table: "UserDeposites");

            migrationBuilder.DropColumn(
                name: "AuctionId",
                table: "UserDeposites");
        }
    }
}
