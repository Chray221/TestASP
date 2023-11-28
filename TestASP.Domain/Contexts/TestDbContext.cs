using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using Microsoft.Extensions.Options;
using TestASP.Data;
using TestASP.Data.Social;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TestASP.Domain.Contexts
{
	public class TestDbContext : IdentityDbContext<ApplicationUser>
    //public class TestDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {

        //dbsets
        #region Tables
        // TEST
        public DbSet<DataTypeTable> DataTypeTables { get; set; }

        // USERS
        public DbSet<User> User { get; set; }
        public DbSet<ImageFile> Image { get; set; }

        //SOCIALS
        public DbSet<Event> Events { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostComment> PostComments { get; set; }
        public DbSet<PostCommentReply> PostCommentReplies { get; set; }
        public DbSet<PostLike> PostLikes { get; set; }
        public DbSet<PostShare> PostShares { get; set; }
        public DbSet<PostImage> PostImages { get; set; }

        #endregion

        ConfigurationManager _configuration;

        public TestDbContext(DbContextOptions<TestDbContext> options, ConfigurationManager configuration) : base(options)
        {
            //public TestDbContext(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions, ConfigurationManager configuration)
            //    : base(options, operationalStoreOptions)
            //{
            _configuration = configuration;

            //Database.EnsureCreated();

            //if (Database.GetPendingMigrations().Any())
            //{
            //    Database.Migrate();
            //}
        }

        public static readonly LoggerFactory DbCommandDebugLoggerFactory
          = new LoggerFactory(new[] {
              new DebugLoggerProvider()
          });

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(_configuration.GetConnectionString("TESTDB"),
            //sqlOption => sqlOption.EnableRetryOnFailure(3));
            optionsBuilder.UseSqlite(
                _configuration.GetConnectionString("TESTDB_LITE"),
                b => b.MigrationsAssembly("TestASP.API"));
            //options.UseSqlServer(connection, b => b.MigrationsAssembly("TestASP.API"))
#if DEBUG
            optionsBuilder.EnableDetailedErrors();
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder
                .UseLoggerFactory(DbCommandDebugLoggerFactory) // to set the logger for DB query
                .EnableSensitiveDataLogging(); // enable logging
#endif
            base.OnConfiguring(optionsBuilder);

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}

