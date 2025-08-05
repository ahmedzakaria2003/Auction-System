using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuctionSystem.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateApplicationUserDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SellerFeedbacks_AspNetUsers_SellerId",
                table: "SellerFeedbacks");

            migrationBuilder.AddForeignKey(
                name: "FK_SellerFeedbacks_AspNetUsers_SellerId",
                table: "SellerFeedbacks",
                column: "SellerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SellerFeedbacks_AspNetUsers_SellerId",
                table: "SellerFeedbacks");

            migrationBuilder.AddForeignKey(
                name: "FK_SellerFeedbacks_AspNetUsers_SellerId",
                table: "SellerFeedbacks",
                column: "SellerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
