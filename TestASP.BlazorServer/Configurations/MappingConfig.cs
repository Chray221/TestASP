using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TestASP.BlazorServer.Models;
using TestASP.BlazorServer.Models.Questionnaires.Admin;
using TestASP.Model.Questionnaires;
using TestASP.Model.Request.Questionnaires;
using TestASP.BlazorServer.Extensions;

namespace TestASP.BlazorServer.Configurations
{
	public class MappingConfig : Profile
	{
		public MappingConfig()
		{
            //Questionnaire response
            var questionMapping = CreateMap<QuestionnaireQuestionsResponseDto, BootStrapQuestionnaireQuestionsResponseDto>();
            questionMapping
                .ForMember(dest => dest.QuestionAnswers, opts => opts.Ignore())
                .AfterMap((src, dest, context) =>
                            dest.QuestionAnswers = (src.QuestionAnswers ?? new())
                               .Select(x => context.Mapper.Map<BootStrapQuestionAnswerSubQuestionAnswerResponseDto>(x))
                               .ToList());
            //questionMapping.ReverseMap();
            CreateMap<QuestionAnswerResponseDto, BootStrapQuestionAnswerResponseDto>()
                .IncludeAllDerived();
            //.ReverseMap();
            CreateMap<SubQuestionAnswerResponseDto, BootStrapSubQuestionAnswerResponseDto>();
                //.ReverseMap();

            // Question + Answer + SubQuestion + SubAnswer => BlazorServer Question + Answer + SubQuestion + SubAnswer
            var ansSubAnsMapping = CreateMap<QuestionAnswerSubQuestionAnswerResponseDto,
                                             BootStrapQuestionAnswerSubQuestionAnswerResponseDto>();
            ansSubAnsMapping
                .ForMember(dest => dest.SubQuestionAnswers, opts => opts.Ignore())
                .AfterMap((src, dest, context) =>
                            dest.SubQuestionAnswers = (src.SubQuestionAnswers ?? new())
                               .Select(x => context.Mapper.Map<BootStrapSubQuestionAnswerResponseDto>(x))
                               .ToList());

            //Questionnaire request
            CreateMap<BootStrapSubQuestionAnswerResponseDto, SubQuestionAnswerRequestDto>();
            CreateMap<BootStrapQuestionAnswerSubQuestionAnswerResponseDto, QuestionnaireAnswerSubAnswerRequestDto>()
                .ForMember( dest => dest.SubAnswers, map => map.Ignore())
                .AfterMap( (src, dest, context) =>
                    dest.SubAnswers = (src.SubQuestionAnswers ?? new ())
                       .Where( questionAnswer => !questionAnswer.HasNoAnswer())
                       .Select(questionAnswer => context.Mapper.Map<SubQuestionAnswerRequestDto>(questionAnswer))
                       .ToList() );
            CreateMap<BootStrapQuestionnaireQuestionsResponseDto, List<QuestionnaireAnswerSubAnswerRequestDto>>()
                .ConvertUsing((src, dest, context) =>
                {
                    return src.QuestionAnswers
                                .Select(questionAnswer =>
                                    context.Mapper.Map<QuestionnaireAnswerSubAnswerRequestDto>(questionAnswer))
                                .ToList();
                });
            MapForAdminQuestionnaire();
        }

        public void MapForAdminQuestionnaire()
        {
            CreateMap<BlazorAdminQuestionnaire, QuestionnaireSaveRequest>()
                .IgnoreMember(dest => dest.Questions)
                .AfterMap((src, dest, context) =>
                {
                    dest.Questions = src.Questions.SelectMapList<QuestionSubQuestionSaveRequestDto>(context.Mapper);
                })
                .ReverseMap()
                .IgnoreMember(dest => dest.Questions)
                .AfterMap((src, dest, context) =>
                {
                    dest.Questions = src.Questions?.SelectMapList<BlazorAdminQuestion>(context.Mapper) ?? new ();
                });
            
            CreateMap<BlazorAdminQuestion, QuestionSubQuestionSaveRequestDto>()
                .IgnoreMember(dest => dest.SubQuestions)
                .AfterMap((src, dest, context) =>
                {
                    dest.SubQuestions = src.SubQuestions?.SelectMapList<BaseQuestionResponseDto>(context.Mapper);
                })
                .ReverseMap()
                .IgnoreMember(dest => dest.SubQuestions)
                .AfterMap((src, dest, context) =>
                {
                    dest.SubQuestions = src.SubQuestions?.SelectMapList<BlazorAdminSubQuestion>(context.Mapper);
                });
            CreateMap<BlazorAdminSubQuestion, BaseQuestionResponseDto>().ReverseMap();
            
        }
    }
}

