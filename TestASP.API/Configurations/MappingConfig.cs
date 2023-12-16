using System;
using AutoMapper;
using TestASP.Model;
using TestASP.Data;
using static System.Net.Mime.MediaTypeNames;
using TestASP.API.Models;
using TestASP.Model.Questionnaires;
using TestASP.Model.Request.Questionnaires;
using TestASP.API.Extensions;
using TestASP.Data.Questionnaires;

namespace TestASP.API.Configurations
{
	public class MappingConfig : Profile
	{
		public MappingConfig()
		{
			CreateMap<User, UserDto>()
                .ForMember(
                    dest => dest.Image,
                    opt => opt.MapFrom((src, dest) => string.IsNullOrEmpty(src.Image) ? string.Empty : Setting.Current.GetUserFileUrl(src.Image)))
                .ReverseMap();
            CreateMap<User, PublicProfile>()
                .ForMember(
                    dest => dest.Image,
                    opt => opt.MapFrom((src,dest) => string.IsNullOrEmpty(src.Image) ? string.Empty : Setting.Current.GetUserFileUrl(src.Image)) )
                .ReverseMap();
            //CreateMap<User, SignInUserRequestDto>().ReverseMap();
            //CreateMap<User, SignUpUserRequestDto>().ReverseMap();
            CreateMap<SignInUserRequestDto, User>();
            CreateMap<SignUpUserRequestDto, User>();

            CreateMap<DataTypeTable, DataTypeDto>().ReverseMap();

            //questionnaire
            CreateMapForQuestionnaire();
            //questionnaire answer
            CreateMapForQuestionnaireAnswer();



        }

        private void CreateMapForQuestionnaire()
        {
            #region Response
            CreateMap<UserQuestionnaire, QuestionnaireQuestionsResponseDto>()
                .ConvertUsing((src, dest, context) =>
                {
                    dest = context.Mapper.Map<QuestionnaireQuestionsResponseDto>(src.Questionnaire?? new Questionnaire());
                    if(dest.QuestionAnswers?.Count > 0)
                    {
                        foreach(var qaDest in dest.QuestionAnswers)
                        {
                            if (src.QuestionAnswers?.FirstOrDefault(answer => answer.QuestionId == qaDest.QuestionId) is QuestionnaireAnswer qaSrc)
                            {
                                qaDest.Id = qaSrc.Id;
                                qaDest.Answer = qaSrc.Answer;
                                qaDest.AnswerId = qaSrc.AnswerId;
                                foreach (var sqaDest in qaDest.SubQuestionAnswers ?? new())
                                {
                                    if (qaSrc.SubAnswers?.FirstOrDefault(sub => sub.SubQuestionId == sqaDest.SubQuestionId) is QuestionnaireSubAnswer sqaSrc)
                                    {
                                        sqaDest.Id = sqaSrc.Id;
                                        sqaDest.Answer = sqaSrc.Answer;
                                        sqaDest.AnswerId = sqaSrc.AnswerId;
                                    }
                                }
                            }
                        }
                    }
                    dest.UserQuestionnaireId = src.Id;
                    return dest;
                });
            CreateMap<Questionnaire, QuestionnaireResponseDto>()
                .ReverseMap();

            CreateMap<Questionnaire, QuestionnaireQuestionsResponseDto>()
                .ForMember(dest => dest.QuestionAnswers, map => map.Ignore())
                .AfterMap((src, dest, context) =>
                    dest.QuestionAnswers = src.Questions?.Select(qstn => context.Mapper.Map<QuestionAnswerSubQuestionAnswerResponseDto>(qstn))
                                                        .ToList() ?? new List<QuestionAnswerSubQuestionAnswerResponseDto>());
            CreateMap<QuestionnaireQuestion, QuestionAnswerSubQuestionAnswerResponseDto>()
                //.ForMember(dest => dest.QuestionId, map => map.MapFrom( src => src.Id))
                .ForMemberMap(dest => dest.QuestionId, src => src.Id)
                .ForMember(dest => dest.SubQuestionAnswers, map => map.Ignore())
                .ForMember(dest => dest.Choices, map => map.Ignore())
                .AfterMap((src, dest, context) =>
                {
                    dest.SubQuestionAnswers = src.SubQuestions?.Select(subQstn => context.Mapper.Map<SubQuestionAnswerResponseDto>(subQstn))
                                                              .ToList();
                    dest.Choices = src.Choices?.Select(choice => context.Mapper.Map<QuestionChoiceDto>(choice))
                                                              .ToList();
                });
            CreateMap<QuestionnaireSubQuestion, SubQuestionAnswerResponseDto>()
                .ForMemberMap(dest => dest.SubQuestionId, src => src.Id)
                .ForMember(dest => dest.Choices, map => map.Ignore())
                .AfterMap((src, dest, context) =>
                {
                    dest.Choices = src.Choices?.Select(choice => context.Mapper.Map<QuestionChoiceDto>(choice))
                                                              .ToList();
                });

            CreateMap<QuestionnaireQuestionChoice, QuestionChoiceDto>()
                .ForMember(dest => dest.Name, map => map.MapFrom(src => src.Label))
                .ReverseMap()
                .ForMember(dest => dest.Label, map => map.MapFrom(src => src.Name));
            #endregion

            #region Request

            CreateMap<QuestionnaireSaveRequest,Questionnaire>()
                .ForMember(dest => dest.Questions, map => map.Ignore())
                .AfterMap((src, dest, context) =>
                    dest.Questions = src.Questions.Select(qstn => context.Mapper.Map<QuestionnaireQuestion>(qstn))
                                                        .ToList())
                .ReverseMap();

            CreateMap<QuestionSubQuestionSaveRequestDto, QuestionnaireQuestion>()
                .IgnoreMember(dest => dest.SubQuestions)
                .IgnoreMember(dest => dest.Choices)
                .AfterMap((src, dest, context) =>
                {
                    //dest.SubQuestions = src.SubQuestions?.Select(qstn => context.Mapper.Map<QuestionnaireSubQuestion>(qstn))
                    //                                    .ToList();
                    dest.SubQuestions = src.SubQuestions?.SelectMapList<QuestionnaireSubQuestion>(context.Mapper);
                    dest.Choices = src.Choices?.Select(choice => context.Mapper.Map<QuestionnaireQuestionChoice>(choice))
                                                        .ToList();
                })
                .ReverseMap()
                //.IgnoreMember(dest => dest.)
                .IgnoreMember(dest => dest.SubQuestions)
                .IgnoreMember(dest => dest.Choices)
                .AfterMap((src, dest, context) =>
                {
                    dest.SubQuestions = src.SubQuestions.Select(qstn => context.Mapper.Map<BaseQuestionResponseDto>(qstn))
                                                        .ToList();
                    dest.Choices = src.Choices?.Select(choice => context.Mapper.Map<QuestionChoiceDto>(choice))
                                                        .ToList();
                });

            CreateMap<BaseQuestionResponseDto, QuestionnaireSubQuestion>()
                .IgnoreMember(dest => dest.Choices)
                .AfterMap((src, dest, context) =>
                {
                    dest.Choices = src.Choices?.Select(choice => context.Mapper.Map<QuestionnaireQuestionChoice>(choice))
                                                        .ToList();
                })
                .ReverseMap()
                 .IgnoreMember(dest => dest.Choices)
                .AfterMap((src, dest, context) =>
                {
                    dest.Choices = src.Choices?.SelectMapList<QuestionChoiceDto>(context.Mapper);
                    //dest.Choices = src.Choices?.Select(choice => context.Mapper.Map<QuestionChoiceDto>(choice))
                    //                                    .ToList();
                });

            #endregion
            CreateMap<Questionnaire, UserQuestionnaireResponseDto>();
            CreateMap<UserQuestionnaire, UserQuestionnaireResponseDto>()
                .ConvertUsing((src, dest, context) =>
                {
                    context.Mapper.Map<UserQuestionnaireResponseDto>(src.Questionnaire ?? new Questionnaire());    
                    dest.UserQuestionnaireId = src.Id;
                    dest.DateAnswered = src.UpdatedAt ?? src.CreatedAt;
                    return dest;
                });
        }

        private void CreateMapForQuestionnaireAnswer()
        {
            CreateMap<QuestionnaireAnswerSubAnswerRequestDto,QuestionnaireAnswer>()
                .SetIgnoredMember(
                    dest => dest.SubAnswers,
                    (src, dest, mapper) => dest.SubAnswers = src.SubAnswers?.SelectMapList<QuestionnaireSubAnswer>(mapper))
                .ReverseMap()
                .SetIgnoredMember(
                    dest => dest.SubAnswers,
                    (src, dest, mapper) => dest.SubAnswers = src.SubAnswers?.SelectMapList<SubQuestionAnswerRequestDto>(mapper));
            CreateMap<SubQuestionAnswerRequestDto, QuestionnaireSubAnswer>()
                .ReverseMap();
        }
    }
}

