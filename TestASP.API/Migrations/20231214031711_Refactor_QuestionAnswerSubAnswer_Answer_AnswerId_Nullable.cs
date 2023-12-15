using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestASP.API.Migrations
{
    /// <inheritdoc />
    public partial class Refactor_QuestionAnswerSubAnswer_Answer_AnswerId_Nullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Answer",
                table: "QuestionnaireSubAnswers",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<int>(
                name: "QuestionnaireAnswerId",
                table: "QuestionnaireSubAnswers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Answer",
                table: "QuestionnaireAnswers",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionnaireSubAnswers_QuestionnaireAnswerId",
                table: "QuestionnaireSubAnswers",
                column: "QuestionnaireAnswerId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionnaireSubAnswers_QuestionnaireAnswers_QuestionnaireAnswerId",
                table: "QuestionnaireSubAnswers",
                column: "QuestionnaireAnswerId",
                principalTable: "QuestionnaireAnswers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionnaireSubAnswers_QuestionnaireAnswers_QuestionnaireAnswerId",
                table: "QuestionnaireSubAnswers");

            migrationBuilder.DropIndex(
                name: "IX_QuestionnaireSubAnswers_QuestionnaireAnswerId",
                table: "QuestionnaireSubAnswers");

            migrationBuilder.DropColumn(
                name: "QuestionnaireAnswerId",
                table: "QuestionnaireSubAnswers");

            migrationBuilder.AlterColumn<string>(
                name: "Answer",
                table: "QuestionnaireSubAnswers",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Answer",
                table: "QuestionnaireAnswers",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);
        }
    }
}
