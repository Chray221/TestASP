using System;
namespace TestASP.Data.Enums
{
	public enum AnswerTypeEnum
	{
		Text = 1,
		Number = 2,
		Date = 3,
		Boolean = 5,
		BooleanWithSubQuestion = 6,
		MultipleChoice = 8
    }

	public enum QuestionTypeEnum
	{
		Question = 1,
		SubQuestion = 2,
        BooleanYesSubQuestion = 3,
        BooleanNoSubQuestion = 4
    }
}

