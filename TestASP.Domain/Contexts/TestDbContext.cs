﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using TestASP.Data;
using TestASP.Data.Social;

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

        // SOCIALS
        public DbSet<Event> Events { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostComment> PostComments { get; set; }
        public DbSet<PostCommentReply> PostCommentReplies { get; set; }
        public DbSet<PostLike> PostLikes { get; set; }
        public DbSet<PostShare> PostShares { get; set; }
        public DbSet<PostImage> PostImages { get; set; }

        // QUESTIONNAIRE
        public DbSet<Questionnaire> Questionnaires { get; set; }
        public DbSet<QuestionnaireQuestion> QuestionnaireQuestions { get; set; }
        public DbSet<QuestionnaireSubQuestion> QuestionnaireSubQuestions { get; set; }
        public DbSet<QuestionnaireQuestionChoice> QuestionnaireQuestionChoices { get; set; }
        public DbSet<QuestionnaireAnswer> QuestionnaireAnswers { get; set; }
        public DbSet<QuestionnaireSubAnswer> QuestionnaireSubAnswers { get; set; }


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
            //options.UseSqlServer(connection, b => b.MigrationsAssembly("TestASP.API"))
            optionsBuilder.UseSqlite(
                _configuration.GetConnectionString("TESTDB_LITE"),
                b => b.MigrationsAssembly("TestASP.API"));
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
            modelBuilder.Entity<QuestionnaireQuestionChoice>(choiceBuilder =>
            {
                choiceBuilder.ToTable(choiceTableBuilder => choiceTableBuilder.HasCheckConstraint(
                    name: $"CH_{nameof(QuestionnaireQuestionChoice)}_Either{nameof(QuestionnaireQuestionChoice.QuestionId)}Or{nameof(QuestionnaireQuestionChoice.SubQuestionId)}",
                    sql: $"{nameof(QuestionnaireQuestionChoice.QuestionId)} NOT NULL OR " +
                         $"{nameof(QuestionnaireQuestionChoice.SubQuestionId)} NOT NULL"));
            });
        }
    }
}

