using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TestASP.Web.Models;
// using TestASP.Web.Models.Questionnaires.Admin;
using TestASP.Model.Questionnaires;
using TestASP.Model.Request.Questionnaires;
using TestASP.Web.Extensions;
using TestASP.Web.Models.ViewModels;
using TestASP.Model;
using TestASP.Web.Models.ViewModels.Questionnaires;

namespace TestASP.Web.Configurations
{
	public class MappingConfig : Profile
	{
		public MappingConfig()
		{
            MapForAuthentication();
            MapForQuestionnaireAnswer();
            MapForAdminQuestionnaire();
        }

        public void MapForAuthentication()
        {
            CreateMap<LoginViewModel,SignInUserRequestDto>()
                .ReverseMap();
        }

        public void MapForQuestionnaireAnswer()
        {
            // //Questionnaire response
            #region Questionnaire Response
            // questionnaires
            CreateMap<List<UserQuestionnaireResponseDto>, QuestionnaireViewModel>()
                .ConvertUsing( (src, dest, context) =>
                {
                    dest = dest ?? new QuestionnaireViewModel(src);
                    return dest;
                });
            
            // questionnaire
            CreateMap<QuestionnaireQuestionsResponseDto,QuestionnaireQuestionAnswerViewModel>()
                .IgnoreMember( dest => dest.QuestionAnswers)
                .AfterMap( (src, dest, context) =>
                {
                    dest.QuestionAnswers = src.QuestionAnswers.SelectMapList<QuestionAnswerViewModel>(context.Mapper);
                });
            
            // question
            CreateMap<QuestionAnswerSubQuestionAnswerResponseDto,QuestionAnswerViewModel>()
                .IgnoreMember( dest => dest.SubQuestionAnswers)
                .IgnoreMember( dest => dest.Choices)
                .AfterMap( (src, dest, context) =>
                {
                    dest.Choices = src.Choices?.SelectMapList<QuestionChoiceViewModel>(context.Mapper);
                    dest.SubQuestionAnswers = src.SubQuestionAnswers?.SelectMapList<SubQuestionAnswerViewModel>(context.Mapper);
                });
            
            // sub question
            CreateMap<SubQuestionAnswerResponseDto,SubQuestionAnswerViewModel>()
                .IgnoreMember( dest => dest.Choices)
                .AfterMap( (src, dest, context) =>
                {
                    dest.Choices = src.Choices?.SelectMapList<QuestionChoiceViewModel>(context.Mapper);
                });
                
            CreateMap<QuestionChoiceDto, QuestionChoiceViewModel>();
                
            #endregion

            #region Questionnaire Request
            CreateMap<QuestionnaireQuestionAnswerViewModel, List<QuestionnaireAnswerSubAnswerRequestDto>>()
                .ConvertUsing((src, dest, context) =>
                {
                    dest = src.QuestionAnswers.SelectMapList<QuestionnaireAnswerSubAnswerRequestDto>(context.Mapper);
                    return dest;
                });
            
            CreateMap<QuestionAnswerViewModel, QuestionnaireAnswerSubAnswerRequestDto>()
                .IgnoreMember(dest => dest.SubAnswers)
                .AfterMap( (src, dest, context) =>
                {
                    dest.SubAnswers = src.SubQuestionAnswers?.SelectMapList<SubQuestionAnswerRequestDto>(context.Mapper);
                });

            CreateMap<SubQuestionAnswerViewModel, SubQuestionAnswerRequestDto>();                
            #endregion

            // var questionMapping = CreateMap<QuestionnaireQuestionsResponseDto, BootStrapQuestionnaireQuestionsResponseDto>();
            // questionMapping
            //     .ForMember(dest => dest.QuestionAnswers, opts => opts.Ignore())
            //     .AfterMap((src, dest, context) =>
            //                 dest.QuestionAnswers = (src.QuestionAnswers ?? new())
            //                    .Select(x => context.Mapper.Map<BootStrapQuestionAnswerSubQuestionAnswerResponseDto>(x))
            //                    .ToList());
            // //questionMapping.ReverseMap();
            // CreateMap<QuestionAnswerResponseDto, BootStrapQuestionAnswerResponseDto>()
            //     .IncludeAllDerived();
            // //.ReverseMap();
            // CreateMap<SubQuestionAnswerResponseDto, BootStrapSubQuestionAnswerResponseDto>();
            //     //.ReverseMap();

            // // Question + Answer + SubQuestion + SubAnswer => BlazorServer Question + Answer + SubQuestion + SubAnswer
            // var ansSubAnsMapping = CreateMap<QuestionAnswerSubQuestionAnswerResponseDto,
            //                                  BootStrapQuestionAnswerSubQuestionAnswerResponseDto>();
            // ansSubAnsMapping
            //     .ForMember(dest => dest.SubQuestionAnswers, opts => opts.Ignore())
            //     .AfterMap((src, dest, context) =>
            //                 dest.SubQuestionAnswers = (src.SubQuestionAnswers ?? new())
            //                    .Select(x => context.Mapper.Map<BootStrapSubQuestionAnswerResponseDto>(x))
            //                    .ToList());

            // //Questionnaire request
            // CreateMap<BootStrapSubQuestionAnswerResponseDto, SubQuestionAnswerRequestDto>();
            // CreateMap<BootStrapQuestionAnswerSubQuestionAnswerResponseDto, QuestionnaireAnswerSubAnswerRequestDto>()
            //     .ForMember( dest => dest.SubAnswers, map => map.Ignore())
            //     .AfterMap( (src, dest, context) =>
            //         dest.SubAnswers = (src.SubQuestionAnswers ?? new ())
            //            .Where( questionAnswer => !questionAnswer.HasNoAnswer())
            //            .Select(questionAnswer => context.Mapper.Map<SubQuestionAnswerRequestDto>(questionAnswer))
            //            .ToList() );
            // CreateMap<BootStrapQuestionnaireQuestionsResponseDto, List<QuestionnaireAnswerSubAnswerRequestDto>>()
            //     .ConvertUsing((src, dest, context) =>
            //     {
            //         return src.QuestionAnswers
            //                     .Select(questionAnswer =>
            //                         context.Mapper.Map<QuestionnaireAnswerSubAnswerRequestDto>(questionAnswer))
            //                     .ToList();
            //     });
        }

        public void MapForAdminQuestionnaire()
        {
            // CreateMap<BlazorAdminQuestionnaire, QuestionnaireSaveRequest>()
            //     .IgnoreMember(dest => dest.Questions)
            //     .AfterMap((src, dest, context) =>
            //     {
            //         dest.Questions = src.Questions.SelectMapList<QuestionSubQuestionSaveRequestDto>(context.Mapper);
            //     })
            //     .ReverseMap()
            //     .IgnoreMember(dest => dest.Questions)
            //     .AfterMap((src, dest, context) =>
            //     {
            //         dest.Questions = src.Questions?.SelectMapList<BlazorAdminQuestion>(context.Mapper) ?? new ();
            //     });
            
            // CreateMap<BlazorAdminQuestion, QuestionSubQuestionSaveRequestDto>()
            //     .IgnoreMember(dest => dest.SubQuestions)
            //     .AfterMap((src, dest, context) =>
            //     {
            //         dest.SubQuestions = src.SubQuestions?.SelectMapList<BaseQuestionResponseDto>(context.Mapper);
            //     })
            //     .ReverseMap()
            //     .IgnoreMember(dest => dest.SubQuestions)
            //     .AfterMap((src, dest, context) =>
            //     {
            //         dest.SubQuestions = src.SubQuestions?.SelectMapList<BlazorAdminSubQuestion>(context.Mapper);
            //     });
            // CreateMap<BlazorAdminSubQuestion, BaseQuestionResponseDto>().ReverseMap();            
        }
    }
}

