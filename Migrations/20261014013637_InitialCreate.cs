using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace quanlimonan.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BIT240154_DishCategories",
                columns: table => new
                {
                    BIT240154_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BIT240154_Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BIT240154_Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BIT240154_DishCategories", x => x.BIT240154_Id);
                });

            migrationBuilder.CreateTable(
                name: "BIT240154_Dishes",
                columns: table => new
                {
                    BIT240154_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BIT240154_Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    BIT240154_Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BIT240154_PreparationTime = table.Column<int>(type: "int", nullable: false),
                    BIT240154_IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    BIT240154_Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BIT240154_DishCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BIT240154_Dishes", x => x.BIT240154_Id);
                    table.ForeignKey(
                        name: "FK_BIT240154_Dishes_BIT240154_DishCategories_BIT240154_DishCategoryId",
                        column: x => x.BIT240154_DishCategoryId,
                        principalTable: "BIT240154_DishCategories",
                        principalColumn: "BIT240154_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BIT240154_DishImages",
                columns: table => new
                {
                    BIT240154_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BIT240154_ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    BIT240154_IsThumbnail = table.Column<bool>(type: "bit", nullable: false),
                    BIT240154_DishId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BIT240154_DishImages", x => x.BIT240154_Id);
                    table.ForeignKey(
                        name: "FK_BIT240154_DishImages_BIT240154_Dishes_BIT240154_DishId",
                        column: x => x.BIT240154_DishId,
                        principalTable: "BIT240154_Dishes",
                        principalColumn: "BIT240154_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BIT240154_Dishes_BIT240154_DishCategoryId_BIT240154_Name",
                table: "BIT240154_Dishes",
                columns: new[] { "BIT240154_DishCategoryId", "BIT240154_Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BIT240154_DishImages_BIT240154_DishId",
                table: "BIT240154_DishImages",
                column: "BIT240154_DishId",
                unique: true,
                filter: "[BIT240154_IsThumbnail] = 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BIT240154_DishImages");

            migrationBuilder.DropTable(
                name: "BIT240154_Dishes");

            migrationBuilder.DropTable(
                name: "BIT240154_DishCategories");
        }
    }
}
