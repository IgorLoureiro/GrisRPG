using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrisAPI.Migrations
{
    /// <inheritdoc />
    public partial class updatingCreatureExtraDeckRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Creatures_ExtraDecks_ExtraDeckId",
                table: "Creatures");

            migrationBuilder.DropIndex(
                name: "IX_Creatures_ExtraDeckId",
                table: "Creatures");

            migrationBuilder.DropColumn(
                name: "NumberOfCards",
                table: "Decks");

            migrationBuilder.DropColumn(
                name: "ExtraDeckId",
                table: "Creatures");

            migrationBuilder.RenameColumn(
                name: "NumberOfCards",
                table: "ExtraDecks",
                newName: "CreatureId");

            migrationBuilder.CreateIndex(
                name: "IX_ExtraDecks_CreatureId",
                table: "ExtraDecks",
                column: "CreatureId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ExtraDecks_Creatures_CreatureId",
                table: "ExtraDecks",
                column: "CreatureId",
                principalTable: "Creatures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExtraDecks_Creatures_CreatureId",
                table: "ExtraDecks");

            migrationBuilder.DropIndex(
                name: "IX_ExtraDecks_CreatureId",
                table: "ExtraDecks");

            migrationBuilder.RenameColumn(
                name: "CreatureId",
                table: "ExtraDecks",
                newName: "NumberOfCards");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfCards",
                table: "Decks",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ExtraDeckId",
                table: "Creatures",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Creatures_ExtraDeckId",
                table: "Creatures",
                column: "ExtraDeckId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Creatures_ExtraDecks_ExtraDeckId",
                table: "Creatures",
                column: "ExtraDeckId",
                principalTable: "ExtraDecks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
