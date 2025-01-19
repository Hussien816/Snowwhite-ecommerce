using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcommerceApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateShippingAddressColumns_v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StreetAddress",
                table: "ShippingAddresses",
                newName: "Street");

            migrationBuilder.RenameColumn(
                name: "ShippingAddressId",
                table: "ShippingAddresses",
                newName: "AddressId");

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "ShippingAddresses",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "ShippingAddresses");

            migrationBuilder.RenameColumn(
                name: "Street",
                table: "ShippingAddresses",
                newName: "StreetAddress");

            migrationBuilder.RenameColumn(
                name: "AddressId",
                table: "ShippingAddresses",
                newName: "ShippingAddressId");
        }
    }
}
