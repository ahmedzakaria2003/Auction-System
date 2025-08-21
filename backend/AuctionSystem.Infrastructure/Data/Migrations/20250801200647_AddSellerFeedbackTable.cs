using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuctionSystem.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSellerFeedbackTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SellerFeedbacks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SellerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BidderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuctionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellerFeedbacks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SellerFeedbacks_AspNetUsers_BidderId",
                        column: x => x.BidderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SellerFeedbacks_AspNetUsers_SellerId",
                        column: x => x.SellerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SellerFeedbacks_Auctions_AuctionId",
                        column: x => x.AuctionId,
                        principalTable: "Auctions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SellerFeedbacks_AuctionId",
                table: "SellerFeedbacks",
                column: "AuctionId");

            migrationBuilder.CreateIndex(
                name: "IX_SellerFeedbacks_BidderId",
                table: "SellerFeedbacks",
                column: "BidderId");

            migrationBuilder.CreateIndex(
                name: "IX_SellerFeedbacks_SellerId",
                table: "SellerFeedbacks",
                column: "SellerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SellerFeedbacks");
        }
    }
}
