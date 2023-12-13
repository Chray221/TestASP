using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestASP.Data.Questionnaires
{
	public class UserQuestionnaire : BaseData
	{
		public int UserId { get; set; }
		public int QuestionnaireId { get; set; }
		#region ForeignKey
		[ForeignKey(nameof(UserId))]
		public User? User { get; set; }
		[ForeignKey(nameof(QuestionnaireId))]
		public Questionnaire? Questionnaire { get; set; }
		#endregion
		public UserQuestionnaire()
		{
		}
	}
}

