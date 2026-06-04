using Microsoft.EntityFrameworkCore;
using Homework3.Models;
namespace Homework3.Data;
public class AppDbContext : DbContext
{
    public DbSet<Server> Servers { get; set; }
    public DbSet<Database> Databases { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite("Data Source=servers.db");
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Server>().HasData(
            new Server { Id = 1, Name = "Oracle Enterprise" },
            new Server { Id = 2, Name = "Microsoft SQL Server" },
            new Server { Id = 3, Name = "PostgreSQL" },
            new Server { Id = 4, Name = "MongoDB Atlas" },
            new Server { Id = 5, Name = "IBM Db2" }
        );
        modelBuilder.Entity<Database>().HasData(
            new Database { Id = 1, Name = "CRM_Prod", SizeGb = 512, ServerId = 1 },
            new Database { Id = 2, Name = "ERP_Finance", SizeGb = 1024, ServerId = 1 },
            new Database { Id = 3, Name = "Analytics_Dev", SizeGb = 256, ServerId = 2 },
            new Database { Id = 4, Name = "Reporting", SizeGb = 128, ServerId = 2 },
            new Database { Id = 5, Name = "MainDB", SizeGb = 64, ServerId = 3 },
            new Database { Id = 6, Name = "BackupDB", SizeGb = 32, ServerId = 3 },
            new Database { Id = 7, Name = "UserData", SizeGb = 512, ServerId = 4 },
            new Database { Id = 8, Name = "Logs", SizeGb = 256, ServerId = 4 },
            new Database { Id = 9, Name = "Warehouse", SizeGb = 2048, ServerId = 5 },
            new Database { Id = 10, Name = "Staging", SizeGb = 1024, ServerId = 5 },
            new Database { Id = 11, Name = "TestEnv", SizeGb = 128, ServerId = 1 },
            new Database { Id = 12, Name = "Archive", SizeGb = 16, ServerId = 3 },
            new Database { Id = 13, Name = "TempDB", SizeGb = 64, ServerId = 2 },
            new Database { Id = 14, Name = "ConfigDB", SizeGb = 32, ServerId = 4 },
            new Database { Id = 15, Name = "Metabase", SizeGb = 512, ServerId = 5 }
        );
    }
}
