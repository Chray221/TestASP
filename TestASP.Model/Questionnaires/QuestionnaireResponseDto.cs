using System;
namespace TestASP.Model.Questionnaires
{
    public class QuestionnaireResponseDto : BaseDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Content { get; set; }
    }

    public class UserQuestionnaireResponseDto : QuestionnaireResponseDto
    {
        public string TempId {get;set;} = "";
        public int? UserQuestionnaireId { get; set; }
        public bool IsAnswered { get { return UserQuestionnaireId != null; } }
        public DateTime? DateAnswered { get; set; }
    }
}

