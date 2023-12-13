using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestASP.API.Migrations
{
    /// <inheritdoc />
    public partial class Add_UserQuestionnaire_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserQuestionnaireId",
                table: "QuestionnaireAnswers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "UserQuestionnaires",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    QuestionnaireId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserQuestionnaires", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserQuestionnaires_Questionnaires_QuestionnaireId",
                        column: x => x.QuestionnaireId,
                        principalTable: "Questionnaires",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserQuestionnaires_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuestionnaireAnswers_UserQuestionnaireId",
                table: "QuestionnaireAnswers",
                column: "UserQuestionnaireId");

            migrationBuilder.CreateIndex(
                name: "IX_UserQuestionnaires_QuestionnaireId",
                table: "UserQuestionnaires",
                column: "QuestionnaireId");

            migrationBuilder.CreateIndex(
                name: "IX_UserQuestionnaires_UserId",
                table: "UserQuestionnaires",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionnaireAnswers_UserQuestionnaires_UserQuestionnaireId",
                table: "QuestionnaireAnswers",
                column: "UserQuestionnaireId",
                principalTable: "UserQuestionnaires",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionnaireAnswers_UserQuestionnaires_UserQuestionnaireId",
                table: "QuestionnaireAnswers");

            migrationBuilder.DropTable(
                name: "UserQuestionnaires");

            migrationBuilder.DropIndex(
                name: "IX_QuestionnaireAnswers_UserQuestionnaireId",
                table: "QuestionnaireAnswers");

            migrationBuilder.DropColumn(
                name: "UserQuestionnaireId",
                table: "QuestionnaireAnswers");
        }
    }
}
