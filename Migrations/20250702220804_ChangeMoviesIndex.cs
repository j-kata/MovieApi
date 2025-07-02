using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieApi.Migrations
{
    /// <inheritdoc />
    public partial class ChangeMoviesIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieDetails_Movies_MovieId",
                table: "MovieDetails");

            migrationBuilder.DropIndex(
                name: "IX_MovieDetails_MovieId",
                table: "MovieDetails");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_MovieDetailsId",
                table: "Movies",
                column: "MovieDetailsId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_MovieDetails_MovieDetailsId",
                table: "Movies",
                column: "MovieDetailsId",
                principalTable: "MovieDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_MovieDetails_MovieDetailsId",
                table: "Movies");

            migrationBuilder.DropIndex(
                name: "IX_Movies_MovieDetailsId",
                table: "Movies");

            migrationBuilder.CreateIndex(
                name: "IX_MovieDetails_MovieId",
                table: "MovieDetails",
                column: "MovieId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieDetails_Movies_MovieId",
                table: "MovieDetails",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
