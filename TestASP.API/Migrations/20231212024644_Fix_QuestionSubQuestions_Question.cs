using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestASP.API.Migrations
{
    /// <inheritdoc />
    public partial class Fix_QuestionSubQuestions_Question : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Question",
                table: "QuestionnaireSubQuestions",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Question",
                table: "QuestionnaireSubQuestions");
        }
    }
}
