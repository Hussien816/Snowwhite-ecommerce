using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcommerceApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateShippingAddressValidation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Street",
                table: "ShippingAddresses",
                newName: "StreetAddress");

            migrationBuilder.RenameColumn(
                name: "AddressId",
                table: "ShippingAddresses",
                newName: "ShippingAddressId");

            migrationBuilder.AlterColumn<string>(
                name: "PostalCode",
                table: "ShippingAddresses",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StreetAddress",
                table: "ShippingAddresses",
                newName: "Street");

            migrationBuilder.RenameColumn(
                name: "ShippingAddressId",
                table: "ShippingAddresses",
                newName: "AddressId");

            migrationBuilder.AlterColumn<string>(
                name: "PostalCode",
                table: "ShippingAddresses",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(5)",
                oldMaxLength: 5);
        }
    }
}
