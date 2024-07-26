using AppAny.Quartz.EntityFrameworkCore.Migrations;
using AppAny.Quartz.EntityFrameworkCore.Migrations.SqlServer;
using Microsoft.EntityFrameworkCore;

namespace QuartzDemoWebApi.Database;

public class DemoDbContext : DbContext
{
    protected DemoDbContext()
    {
    }

    public DemoDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.AddQuartz(builder => builder.UseSqlServer());
    }
}
