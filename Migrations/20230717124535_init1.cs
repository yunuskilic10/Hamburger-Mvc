using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCProjectHamburger.Migrations
{
    public partial class init1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MenuSize",
                table: "ShoppingCarts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "AppUserID",
                table: "ExtraIngredientOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "ExtraIngredientOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "da15f50e-e543-431a-984b-1fd5894f6a71");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "bb012711-99d7-457a-a64f-6dfc3841ed5a");

            migrationBuilder.CreateIndex(
                name: "IX_ExtraIngredientOrders_AppUserID",
                table: "ExtraIngredientOrders",
                column: "AppUserID");

            migrationBuilder.AddForeignKey(
                name: "FK_ExtraIngredientOrders_AspNetUsers_AppUserID",
                table: "ExtraIngredientOrders",
                column: "AppUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExtraIngredientOrders_AspNetUsers_AppUserID",
                table: "ExtraIngredientOrders");

            migrationBuilder.DropIndex(
                name: "IX_ExtraIngredientOrders_AppUserID",
                table: "ExtraIngredientOrders");

            migrationBuilder.DropColumn(
                name: "AppUserID",
                table: "ExtraIngredientOrders");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "ExtraIngredientOrders");

            migrationBuilder.AlterColumn<string>(
                name: "MenuSize",
                table: "ShoppingCarts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "c57b6587-70c5-4d99-a592-b9b7ef566639");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "d0472f20-30db-4d01-adc3-2965a4579be7");
        }
    }
}
