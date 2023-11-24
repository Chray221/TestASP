using Microsoft.Extensions.DependencyInjection;
using TestASP.Core.IRepository;
using TestASP.Core.IRepository.Social;
using TestASP.Domain.Repository;
using TestASP.Domain.Repository.Social;

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
            
            return services;
        }

        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            // scope services
            //services.AddScoped<CommentListViewService>();
            //services.AddScoped<PostListViewService>();
            //services.AddScoped<IPopupService, PopupService>();
            //services.AddScoped<ImageService>();
            return services;
        }
    }
}

