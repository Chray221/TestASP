using System;
using TestASP.Model.Questionnaires;

namespace TestASP.BlazorServer.Models.Questionnaires.Admin
{
    public class BlazorAdminQuestion : BaseBlazorAdminQuestionResponseDto
    {
        public List<BlazorAdminSubQuestion>? SubQuestions { get; set; }
    }
}

