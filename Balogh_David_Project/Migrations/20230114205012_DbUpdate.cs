using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BaloghDavidProject.Migrations
{
    /// <inheritdoc />
    public partial class DbUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Distributor_DistributorID",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_DistributorID",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "DistributorID",
                table: "Order");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DistributorID",
                table: "Order",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Order_DistributorID",
                table: "Order",
                column: "DistributorID");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Distributor_DistributorID",
                table: "Order",
                column: "DistributorID",
                principalTable: "Distributor",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
