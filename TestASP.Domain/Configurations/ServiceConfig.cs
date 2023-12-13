using Microsoft.Extensions.DependencyInjection;
using TestASP.Core.IRepository;
using TestASP.Core.IRepository.Social;
using TestASP.Core.IService;
using TestASP.Domain.Repository;
using TestASP.Domain.Repository.Social;
using TestASP.Domain.Services;

namespace TestASP.Domain.Configurations
{
    public static class ServiceConfig
	{
		public static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IImageFileRepository, ImageFileRepository>();
            services.AddTransient<IPostCommentRepository, PostCommentRepository>();
            services.AddTransient<IPostRepository, PostRepository>();
            services.AddTransient<IQuestionnaireRepository, QuestionnaireRepository>();
            services.AddTransient<IUserQuestionnaireRepository, UserQuestionnaireRepository>();

            return services;
        }

        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            // scope services
            //services.AddScoped<CommentListViewService>();
            //services.AddScoped<PostListViewService>();
            //services.AddScoped<IPopupService, PopupService>();
            //services.AddScoped<ImageService>();
            services.AddTransient<IDataValidationService, DataValidationService>();
            return services;
        }
    }
}

