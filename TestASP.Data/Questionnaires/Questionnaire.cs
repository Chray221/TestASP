using System.ComponentModel.DataAnnotations.Schema;

namespace TestASP.Data
{
    public class Questionnaire : BaseData
	{
		public string Name { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public string? Content { get; set; } = string.Empty;

		public virtual List<QuestionnaireQuestion> Questions { get; set; }
	}
}

