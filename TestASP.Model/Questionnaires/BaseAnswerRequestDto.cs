using System;
namespace TestASP.Model.Questionnaires
{
    public class BaseAnswerRequestDto : BaseDto
    {
        public string Answer { get; set; }
        public int AnswerId { get; set; }
    }
}

