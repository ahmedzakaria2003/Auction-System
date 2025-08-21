using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuctionSystem.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIsPaidAndPaymentIntent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "Auctions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PaymentIntentId",
                table: "Auctions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "Auctions");

            migrationBuilder.DropColumn(
                name: "PaymentIntentId",
                table: "Auctions");
        }
    }
}
