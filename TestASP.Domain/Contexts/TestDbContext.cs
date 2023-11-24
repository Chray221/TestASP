using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using TestASP.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TestASP.Domain.Contexts
{
	public class TestDbContext : IdentityDbContext<ApplicationUser>
    //public class TestDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {

        //dbsets
        #region Tables
        public DbSet<DataTypeTable> DataTypeTables { get; set; }

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
#endif
            base.OnConfiguring(optionsBuilder);

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}

