using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcommerceApi.Migrations
{
    /// <inheritdoc />
    public partial class SeedProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add Men's Products
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "ProductName", "Description", "Price", "CategoryId", "ImagePath", "CreatedAt" },
                values: new object[,]
                {
                    { 1, "Men's T-Shirt", "Comfortable cotton t-shirt", 29.99m, 5, "images/f1.JPEG", DateTime.UtcNow },
                    { 2, "Men's Jeans", "Classic blue jeans", 49.99m, 5, "images/f2.jpg", DateTime.UtcNow },
                    { 3, "Men's Jacket", "Stylish winter jacket", 89.99m, 5, "images/f3.jpg", DateTime.UtcNow }
                }
            );

            // Add Product Sizes for Men's Products
            migrationBuilder.InsertData(
                table: "ProductSizes",
                columns: new[] { "ProductSizeId", "ProductId", "Size", "Quantity" },
                values: new object[,]
                {
                    { 1, 1, "S", 10 },
                    { 2, 1, "M", 15 },
                    { 3, 1, "L", 20 },
                    { 4, 2, "32", 8 },
                    { 5, 2, "34", 12 },
                    { 6, 2, "36", 10 },
                    { 7, 3, "M", 5 },
                    { 8, 3, "L", 8 },
                    { 9, 3, "XL", 6 }
                }
            );

            // Add Women's Products
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "ProductName", "Description", "Price", "CategoryId", "ImagePath", "CreatedAt" },
                values: new object[,]
                {
                    { 4, "Women's Dress", "Elegant summer dress", 59.99m, 7, "images/f4.jpg", DateTime.UtcNow, null },
                    { 5, "Women's Blouse", "Casual blouse", 34.99m, 7, "images/f5.jpg", DateTime.UtcNow, null },
                    { 6, "Women's Skirt", "Floral pattern skirt", 39.99m, 7, "images/f6.jpg", DateTime.UtcNow, null }
                }
            );

            // Add Product Sizes for Women's Products
            migrationBuilder.InsertData(
                table: "ProductSizes",
                columns: new[] { "ProductSizeId", "ProductId", "Size", "Quantity" },
                values: new object[,]
                {
                    { 10, 4, "S", 12 },
                    { 11, 4, "M", 15 },
                    { 12, 4, "L", 10 },
                    { 13, 5, "S", 8 },
                    { 14, 5, "M", 12 },
                    { 15, 5, "L", 7 },
                    { 16, 6, "S", 10 },
                    { 17, 6, "M", 13 },
                    { 18, 6, "L", 9 }
                }
            );

            // Add Kids' Products
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "ProductName", "Description", "Price", "CategoryId", "ImagePath", "CreatedAt" },
                values: new object[,]
                {
                    { 7, "Kids' T-Shirt", "Colorful kids t-shirt", 19.99m, 8, "images/f7.jpg", DateTime.UtcNow, null },
                    { 8, "Kids' Pants", "Comfortable pants", 24.99m, 8, "images/f8.jpg", DateTime.UtcNow, null }
                }
            );

            // Add Product Sizes for Kids' Products
            migrationBuilder.InsertData(
                table: "ProductSizes",
                columns: new[] { "ProductSizeId", "ProductId", "Size", "Quantity" },
                values: new object[,]
                {
                    { 19, 7, "4T", 15 },
                    { 20, 7, "5T", 18 },
                    { 21, 7, "6T", 12 },
                    { 22, 8, "4T", 10 },
                    { 23, 8, "5T", 14 },
                    { 24, 8, "6T", 8 }
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProductSizes",
                keyColumn: "ProductSizeId",
                keyValues: new object[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 }
            );

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValues: new object[] { 1, 2, 3, 4, 5, 6, 7, 8 }
            );
        }
    }
}
