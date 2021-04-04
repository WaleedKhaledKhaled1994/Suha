using Microsoft.EntityFrameworkCore.Migrations;

namespace AD.Server.Migrations
{
    public partial class _7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "StorePaymentMethodId",
                table: "Orders",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_StorePaymentMethodId",
                table: "Orders",
                column: "StorePaymentMethodId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_StorePaymentMethods_StorePaymentMethodId",
                table: "Orders",
                column: "StorePaymentMethodId",
                principalTable: "StorePaymentMethods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_StorePaymentMethods_StorePaymentMethodId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_StorePaymentMethodId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "StorePaymentMethodId",
                table: "Orders");
        }
    }
}
