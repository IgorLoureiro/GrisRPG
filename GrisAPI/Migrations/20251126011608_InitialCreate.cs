using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GrisAPI.Migrations
{
    [ExcludeFromCodeCoverage]
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Symbol = table.Column<int>(type: "integer", nullable: false),
                    Manifestation = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Decks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    NumberOfCards = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Decks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExtraDecks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NumberOfCards = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtraDecks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Jokers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jokers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IsMaster = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CardDeck",
                columns: table => new
                {
                    CardsId = table.Column<int>(type: "integer", nullable: false),
                    DecksId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardDeck", x => new { x.CardsId, x.DecksId });
                    table.ForeignKey(
                        name: "FK_CardDeck_Cards_CardsId",
                        column: x => x.CardsId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardDeck_Decks_DecksId",
                        column: x => x.DecksId,
                        principalTable: "Decks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CardExtraDeck",
                columns: table => new
                {
                    CardsId = table.Column<int>(type: "integer", nullable: false),
                    ExtraDecksId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardExtraDeck", x => new { x.CardsId, x.ExtraDecksId });
                    table.ForeignKey(
                        name: "FK_CardExtraDeck_Cards_CardsId",
                        column: x => x.CardsId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardExtraDeck_ExtraDecks_ExtraDecksId",
                        column: x => x.ExtraDecksId,
                        principalTable: "ExtraDecks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Creatures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ExtraDeckId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Creatures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Creatures_ExtraDecks_ExtraDeckId",
                        column: x => x.ExtraDeckId,
                        principalTable: "ExtraDecks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeckJoker",
                columns: table => new
                {
                    DecksId = table.Column<int>(type: "integer", nullable: false),
                    JokersId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeckJoker", x => new { x.DecksId, x.JokersId });
                    table.ForeignKey(
                        name: "FK_DeckJoker_Decks_DecksId",
                        column: x => x.DecksId,
                        principalTable: "Decks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeckJoker_Jokers_JokersId",
                        column: x => x.JokersId,
                        principalTable: "Jokers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExtraDeckJoker",
                columns: table => new
                {
                    ExtraDecksId = table.Column<int>(type: "integer", nullable: false),
                    JokersId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtraDeckJoker", x => new { x.ExtraDecksId, x.JokersId });
                    table.ForeignKey(
                        name: "FK_ExtraDeckJoker_ExtraDecks_ExtraDecksId",
                        column: x => x.ExtraDecksId,
                        principalTable: "ExtraDecks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExtraDeckJoker_Jokers_JokersId",
                        column: x => x.JokersId,
                        principalTable: "Jokers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CreatureDeck",
                columns: table => new
                {
                    CreaturesId = table.Column<int>(type: "integer", nullable: false),
                    DecksId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreatureDeck", x => new { x.CreaturesId, x.DecksId });
                    table.ForeignKey(
                        name: "FK_CreatureDeck_Creatures_CreaturesId",
                        column: x => x.CreaturesId,
                        principalTable: "Creatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreatureDeck_Decks_DecksId",
                        column: x => x.DecksId,
                        principalTable: "Decks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CreatureUser",
                columns: table => new
                {
                    CreaturesId = table.Column<int>(type: "integer", nullable: false),
                    UsersId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreatureUser", x => new { x.CreaturesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_CreatureUser_Creatures_CreaturesId",
                        column: x => x.CreaturesId,
                        principalTable: "Creatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreatureUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CardDeck_DecksId",
                table: "CardDeck",
                column: "DecksId");

            migrationBuilder.CreateIndex(
                name: "IX_CardExtraDeck_ExtraDecksId",
                table: "CardExtraDeck",
                column: "ExtraDecksId");

            migrationBuilder.CreateIndex(
                name: "IX_CreatureDeck_DecksId",
                table: "CreatureDeck",
                column: "DecksId");

            migrationBuilder.CreateIndex(
                name: "IX_Creatures_ExtraDeckId",
                table: "Creatures",
                column: "ExtraDeckId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CreatureUser_UsersId",
                table: "CreatureUser",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_DeckJoker_JokersId",
                table: "DeckJoker",
                column: "JokersId");

            migrationBuilder.CreateIndex(
                name: "IX_ExtraDeckJoker_JokersId",
                table: "ExtraDeckJoker",
                column: "JokersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CardDeck");

            migrationBuilder.DropTable(
                name: "CardExtraDeck");

            migrationBuilder.DropTable(
                name: "CreatureDeck");

            migrationBuilder.DropTable(
                name: "CreatureUser");

            migrationBuilder.DropTable(
                name: "DeckJoker");

            migrationBuilder.DropTable(
                name: "ExtraDeckJoker");

            migrationBuilder.DropTable(
                name: "Cards");

            migrationBuilder.DropTable(
                name: "Creatures");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Decks");

            migrationBuilder.DropTable(
                name: "Jokers");

            migrationBuilder.DropTable(
                name: "ExtraDecks");
        }
    }
}
