using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScreenSound.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMusicas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Update Musicas set ArtistaId = 1 where Id = 1");
            migrationBuilder.Sql("Update Musicas set ArtistaId = 1 where Id = 2");
            migrationBuilder.Sql("Update Musicas set ArtistaId = 1 where Id = 3");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Update Musicas set ArtistasId = null");
        }
    }
}
