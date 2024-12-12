using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace movies_DAL.Migrations
{
    /// <inheritdoc />
    public partial class UserMovieDbSets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSeenMovie_Movies_MovieId",
                table: "UserSeenMovie");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSeenMovie_User_UserId",
                table: "UserSeenMovie");

            migrationBuilder.DropForeignKey(
                name: "FK_UserWatchlistMovie_Movies_MovieId",
                table: "UserWatchlistMovie");

            migrationBuilder.DropForeignKey(
                name: "FK_UserWatchlistMovie_User_UserId",
                table: "UserWatchlistMovie");

            migrationBuilder.DropTable(
                name: "UserFavoriteMovie");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserWatchlistMovie",
                table: "UserWatchlistMovie");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserSeenMovie",
                table: "UserSeenMovie");

            migrationBuilder.RenameTable(
                name: "UserWatchlistMovie",
                newName: "UserWatchlistMovies");

            migrationBuilder.RenameTable(
                name: "UserSeenMovie",
                newName: "UserSeenMovies");

            migrationBuilder.RenameIndex(
                name: "IX_UserWatchlistMovie_MovieId",
                table: "UserWatchlistMovies",
                newName: "IX_UserWatchlistMovies_MovieId");

            migrationBuilder.RenameIndex(
                name: "IX_UserSeenMovie_MovieId",
                table: "UserSeenMovies",
                newName: "IX_UserSeenMovies_MovieId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserWatchlistMovies",
                table: "UserWatchlistMovies",
                columns: new[] { "UserId", "MovieId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserSeenMovies",
                table: "UserSeenMovies",
                columns: new[] { "UserId", "MovieId" });

            migrationBuilder.CreateTable(
                name: "UserFavouriteMovies",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MovieId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFavouriteMovies", x => new { x.UserId, x.MovieId });
                    table.ForeignKey(
                        name: "FK_UserFavouriteMovies_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserFavouriteMovies_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserFavouriteMovies_MovieId",
                table: "UserFavouriteMovies",
                column: "MovieId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSeenMovies_Movies_MovieId",
                table: "UserSeenMovies",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSeenMovies_User_UserId",
                table: "UserSeenMovies",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserWatchlistMovies_Movies_MovieId",
                table: "UserWatchlistMovies",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserWatchlistMovies_User_UserId",
                table: "UserWatchlistMovies",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSeenMovies_Movies_MovieId",
                table: "UserSeenMovies");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSeenMovies_User_UserId",
                table: "UserSeenMovies");

            migrationBuilder.DropForeignKey(
                name: "FK_UserWatchlistMovies_Movies_MovieId",
                table: "UserWatchlistMovies");

            migrationBuilder.DropForeignKey(
                name: "FK_UserWatchlistMovies_User_UserId",
                table: "UserWatchlistMovies");

            migrationBuilder.DropTable(
                name: "UserFavouriteMovies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserWatchlistMovies",
                table: "UserWatchlistMovies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserSeenMovies",
                table: "UserSeenMovies");

            migrationBuilder.RenameTable(
                name: "UserWatchlistMovies",
                newName: "UserWatchlistMovie");

            migrationBuilder.RenameTable(
                name: "UserSeenMovies",
                newName: "UserSeenMovie");

            migrationBuilder.RenameIndex(
                name: "IX_UserWatchlistMovies_MovieId",
                table: "UserWatchlistMovie",
                newName: "IX_UserWatchlistMovie_MovieId");

            migrationBuilder.RenameIndex(
                name: "IX_UserSeenMovies_MovieId",
                table: "UserSeenMovie",
                newName: "IX_UserSeenMovie_MovieId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserWatchlistMovie",
                table: "UserWatchlistMovie",
                columns: new[] { "UserId", "MovieId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserSeenMovie",
                table: "UserSeenMovie",
                columns: new[] { "UserId", "MovieId" });

            migrationBuilder.CreateTable(
                name: "UserFavoriteMovie",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MovieId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFavoriteMovie", x => new { x.UserId, x.MovieId });
                    table.ForeignKey(
                        name: "FK_UserFavoriteMovie_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserFavoriteMovie_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserFavoriteMovie_MovieId",
                table: "UserFavoriteMovie",
                column: "MovieId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSeenMovie_Movies_MovieId",
                table: "UserSeenMovie",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSeenMovie_User_UserId",
                table: "UserSeenMovie",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserWatchlistMovie_Movies_MovieId",
                table: "UserWatchlistMovie",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserWatchlistMovie_User_UserId",
                table: "UserWatchlistMovie",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
