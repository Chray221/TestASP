using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.CodeAnalysis;
using TestASP.Data;
using TestASP.Data.Enums;
using TestASP.Model;
using TestASP.Model.Questionnaires;
using TestASP.Model.Request.Questionnaires;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TestASP.API.Extensions
{
	public static class DataUpdateExtensions
	{
        #region Questionnaire
        public static bool Update(this Questionnaire data, QuestionnaireSaveRequest update, [NotNull] IMapper mapper)
        {
            bool isUpdated = false;

            if (data != null && update != null)
            {
                if (TryUpdatedValue(data.Name, update.Name, out string name))
                {
                    data.Name = name;
                    isUpdated = true;
                }
                if (TryUpdatedValue(data.Content, update.Content ?? "", out string content))
                {
                    data.Content = content;
                    isUpdated = true;
                }
                if (TryUpdatedValue(data.Description, update.Description ?? "", out string description))
                {
                    data.Description = description;
                    isUpdated = true;
                }
                if (data.Questions?.Count > 0)
                {
                    //foreach (var question in data.Questions)
                    //{
                    //    var questionUpdate = update.Questions.FirstOrDefault(updQstn => updQstn.Id == question.Id);
                    //    if (questionUpdate != null)
                    //    {
                    //        if (question.Update(questionUpdate,mapper))
                    //        {
                    //            isUpdated = true;
                    //        }
                    //    }
                    //}
                    data.Questions = data.Questions ?? new();
                    foreach (var updateQuestion in update.Questions)
                    {
                        if(data.Questions.TryUpsertList(updateQuestion, mapper, src => src.Update(updateQuestion, mapper)))
                        {
                            isUpdated = true;
                        }
                        //if (updateQuestion.Id > 0)
                        //{
                        //    var question = data.Questions.FirstOrDefault(Qstn => Qstn.Id == updateQuestion.Id);
                        //    if (question != null && question.Update(updateQuestion, mapper))
                        //    {
                        //        isUpdated = true;
                        //    }
                        //}
                        //else
                        //{
                        //    data.Questions.Add(mapper.Map<QuestionnaireQuestion>(updateQuestion));
                        //    isUpdated = true;
                        //}
                    }
                }
            }
            return isUpdated;
        }

        public static bool Update(this QuestionnaireQuestion data, QuestionSubQuestionSaveRequestDto update, [NotNull] IMapper mapper)
        {
            bool isUpdated = false;

            if (data != null && update != null)
            {
                if (data.BaseUpdate(update,mapper))
                {
                    isUpdated = true;
                }
                if(update.SubQuestions?.Count > 0)
                {
                    data.SubQuestions = data.SubQuestions ?? new List<QuestionnaireSubQuestion>();
                    foreach (var updateSubQuestion in update.SubQuestions)
                    {

                        if(data.SubQuestions.TryUpsertList(updateSubQuestion, mapper, src => src.BaseUpdate(updateSubQuestion, mapper)))
                        {
                            isUpdated = true;
                        }
                        //if (updateSubQuestion.Id > 0)
                        //{
                        //    var subQuestion = data.SubQuestions.FirstOrDefault(subQstn => subQstn.Id == updateSubQuestion.Id);
                        //    if (subQuestion != null && subQuestion.BaseUpdate(updateSubQuestion, mapper))
                        //    {
                        //        isUpdated = true;
                        //    }
                        //}
                        //else
                        //{
                        //    data.SubQuestions.Add(mapper.Map<QuestionnaireSubQuestion>(updateSubQuestion));
                        //    isUpdated = true;
                        //}
                    }
                }
            }
            return isUpdated;
        }

        public static bool Update(this QuestionnaireQuestionChoice data, QuestionChoiceDto update)
        {
            bool isUpdated = false;

            if (data != null && update != null)
            {
                if (TryUpdatedValue(data.Label, update.Name, out string Label))
                {
                    data.Label = Label;
                    isUpdated = true;
                }
                if (TryUpdatedValue(data.Value, update.Value, out string Value))
                {
                    data.Value = Value;
                    isUpdated = true;
                }
            }

            return isUpdated;
        }
        #endregion

        public static bool BaseUpdate(this BaseQuestionnaireQuestion data, BaseQuestionResponseDto update, [NotNull] IMapper mapper)
        {
            bool isUpdated = false;

            if (data != null && update != null)
            {
                if (TryUpdatedValue(data.AnswerTypeId, update.AnswerTypeId, out AnswerTypeEnum AnswerTypeId))
                {
                    data.AnswerTypeId = AnswerTypeId;
                    isUpdated = true;
                }
                if (TryUpdatedValue(data.QuestionTypeId, update.QuestionTypeId, out QuestionTypeEnum QuestionTypeId))
                {
                    data.QuestionTypeId = QuestionTypeId;
                    isUpdated = true;
                }
                if (TryUpdatedValue(data.Question, update.Question, out string Question))
                {
                    data.Question = Question;
                    isUpdated = true;
                }
                if (TryUpdatedValue(data.Number, update.Number, out string Number))
                {
                    data.Number = Number;
                    isUpdated = true;
                }
                if (update.Choices?.Count > 0)
                {
                    data.Choices = data.Choices ?? new List<QuestionnaireQuestionChoice>();
                    foreach (var updateChoice in update.Choices)
                    {
                        if(data.Choices.TryUpsertList(updateChoice, mapper, choice => choice!.Update(updateChoice)))
                        {
                            isUpdated = true;
                        }
                        //if (updateChoice.Id > 0)
                        //{
                        //    var choice = data.Choices.FirstOrDefault(item => item.Id == updateChoice.Id);
                        //    if (choice != null && choice!.Update(updateChoice))
                        //    {
                        //        isUpdated = true;
                        //    }
                        //}
                        //else
                        //{
                        //    data.Choices.Add(mapper.Map<QuestionnaireQuestionChoice>(updateChoice));
                        //    isUpdated = true;
                        //}
                    }
                }
            }
            return isUpdated;
        }

        public static bool TryUpsertList<TSource,TRequest>(this IList<TSource> data, TRequest update, [NotNull] IMapper mapper, Func<TSource,bool> updateAction)
            where TSource: BaseData
            where TRequest: BaseDto
        {
            bool isUpdated = false;
            if (update.Id > 0)
            {
                var question = data.FirstOrDefault(Qstn => Qstn.Id == update.Id);
                if (question != null && updateAction.Invoke(question))
                {
                    isUpdated = true;
                }
            }
            else
            {
                data.Add(mapper.Map<TSource>(update));
                isUpdated = true;
            }
            return isUpdated;
        }

        /// <summary>
        /// return true if updated else false
        /// </summary>
        /// <param name="src">DataModel</param>
        /// <param name="dtoValue">From Request</param>
        /// <param name="value">Updated Value</param>
        /// <returns></returns>
        private static bool TryUpdatedValue<T>(T src, T dtoValue, out T value)
        {
            var isUpdated = !IsNullable(src) ? !(dtoValue?.Equals(default) ?? true) && !dtoValue!.Equals(src) : !(dtoValue?.Equals(src) ?? false);
            value = isUpdated ? dtoValue : src;
            return isUpdated;
        }

        private static bool IsNullable<T>(T obj)
        {
            if (obj == null) return true; // obvious
            Type type = typeof(T);
            if (!type.IsValueType) return true; // ref-type
            if (Nullable.GetUnderlyingType(type) != null) return true; // Nullable<T>
            return false; // value-type
        }
    }
}

