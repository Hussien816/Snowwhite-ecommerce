using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcommerceApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserNameHandling : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop existing FirstName and LastName columns if they exist
            if (migrationBuilder.ActiveProvider == "Microsoft.EntityFrameworkCore.SqlServer")
            {
                // Check if FirstName column exists
                migrationBuilder.Sql(
                    @"IF EXISTS (SELECT 1 
                        FROM INFORMATION_SCHEMA.COLUMNS 
                        WHERE TABLE_NAME = 'Users' 
                        AND COLUMN_NAME = 'FirstName')
                    BEGIN
                        ALTER TABLE Users DROP COLUMN FirstName
                    END");

                // Check if LastName column exists
                migrationBuilder.Sql(
                    @"IF EXISTS (SELECT 1 
                        FROM INFORMATION_SCHEMA.COLUMNS 
                        WHERE TABLE_NAME = 'Users' 
                        AND COLUMN_NAME = 'LastName')
                    BEGIN
                        ALTER TABLE Users DROP COLUMN LastName
                    END");
            }

            // Ensure UserName column exists and has correct configuration
            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            // Add UpdatedAt column if it doesn't exist
            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Users",
                type: "datetime2",
                nullable: true);

            // Add unique index on Email
            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove unique index on Email
            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            // Remove UpdatedAt column
            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Users");

            // Revert UserName column configuration
            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            // Note: We don't restore FirstName and LastName columns in Down migration
            // as that would require data migration which could be lossy
        }
    }
}
