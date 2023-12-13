using System;
namespace TestASP.Model.Questionnaires
{
    public class QuestionnaireResponseDto : BaseDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Content { get; set; }
        public bool? IsAnswered { get; set; }
    }
}

