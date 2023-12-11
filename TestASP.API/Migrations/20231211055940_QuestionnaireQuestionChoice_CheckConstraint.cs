using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestASP.API.Migrations
{
    /// <inheritdoc />
    public partial class QuestionnaireQuestionChoice_CheckConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "CH_QuestionnaireQuestionChoice_EitherQuestionIdOrSubQuestionId",
                table: "QuestionnaireQuestionChoices",
                sql: "QuestionId NOT NULL OR SubQuestionId NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CH_QuestionnaireQuestionChoice_EitherQuestionIdOrSubQuestionId",
                table: "QuestionnaireQuestionChoices");
        }
    }
}
