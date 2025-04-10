using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventManagerAPI_TP.Migrations
{
    /// <inheritdoc />
    public partial class modif_bdd_et_ajout_seeder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ZipCode",
                table: "Locations",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ZipCode",
                table: "Locations");
        }
    }
}
