using System;
using System.Security.AccessControl;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using Newtonsoft.Json;
using TestASP.Data;
using TestASP.Data.Enums;
using TestASP.Data.Questionnaires;
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

        public DbSet<UserQuestionnaire> UserQuestionnaires { get; set; }



        #endregion

        public DbSet<AuditLog> AuditLogs { get; set; }


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

            modelBuilder.Entity<AuditLog>(entity =>
            {
                var converter = new ValueConverter<Dictionary<string,object>, string>(
                            keys => keys == null || keys.Count <= 0
                                        ? string.Empty
                                        : JsonConvert.SerializeObject(keys),
                            keyStr => string.IsNullOrEmpty(keyStr)
                                        ? new Dictionary<string, object>()
                                        : JsonConvert.DeserializeObject<Dictionary<string, object>>(keyStr));
                entity.Property(audit => audit.KeyValues).HasConversion(converter);
                entity.Property(audit => audit.OldValues).HasConversion(converter);
                entity.Property(audit => audit.NewValues).HasConversion(converter);
                entity.Property(audit => audit.AffectedColumns).HasConversion(
                    affCol => affCol == null || affCol.Count <= 0
                                    ? string.Empty
                                    : JsonConvert.SerializeObject(affCol),
                    affColStr => string.IsNullOrEmpty(affColStr)
                                    ? new List<string>()
                                    : JsonConvert.DeserializeObject<List<string>>(affColStr));
            });
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            OnBeforeSaveChanges();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void OnBeforeSaveChanges()
        {
            ChangeTracker.DetectChanges();
            var auditEntries = new List<AuditEntry>();
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is AuditLog || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;
                var auditEntry = new AuditEntry(entry);
                auditEntry.TableName = entry.Entity.GetType().Name;
                auditEntries.Add(auditEntry);
                foreach (var property in entry.Properties)
                {
                    string propertyName = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[propertyName] = property.CurrentValue;
                        continue;
                    }
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.AuditType = AuditType.Create;
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                            if (entry.Entity is BaseData added)
                            {
                                auditEntry.Actor = added.CreatedBy;
                            }
                            break;
                        case EntityState.Deleted:
                            auditEntry.AuditType = AuditType.Delete;
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            if (entry.Entity is BaseData deleted)
                            {
                                auditEntry.Actor = deleted.UpdatedBy ?? "System";
                            }
                            break;
                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                auditEntry.ChangedColumns.Add(propertyName);
                                auditEntry.OldValues[propertyName] = property.OriginalValue;
                                auditEntry.NewValues[propertyName] = property.CurrentValue;

                                if (entry.Entity is BaseData updated)
                                {
                                    auditEntry.Actor = updated.UpdatedBy ?? "System";
                                    auditEntry.AuditType = updated.IsDeleted ? AuditType.Delete : AuditType.Update;
                                }
                            }
                            break;
                    }
                }
            }
            foreach (var auditEntry in auditEntries)
            {
                AuditLogs.Add(auditEntry.ToAudit());
            }
        }
    }
}

