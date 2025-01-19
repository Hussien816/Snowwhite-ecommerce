using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcommerceApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMenCategoryId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Update Men's category ID from 5 to 9
            migrationBuilder.Sql("UPDATE Categories SET CategoryId = 9 WHERE CategoryId = 5");
            
            // Update products that reference the old category ID
            migrationBuilder.Sql("UPDATE Products SET CategoryId = 9 WHERE CategoryId = 5");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert Men's category ID from 9 to 5
            migrationBuilder.Sql("UPDATE Products SET CategoryId = 5 WHERE CategoryId = 9");
            migrationBuilder.Sql("UPDATE Categories SET CategoryId = 5 WHERE CategoryId = 9");
        }
    }
}
