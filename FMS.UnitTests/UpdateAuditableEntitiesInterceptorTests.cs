using FMS.API.Entities;
using FMS.API.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace FMS.UnitTests;

public class UpdateAuditableEntitiesInterceptorTests
{
    private class AuditableEntity : IAuditableEntity
    {
        public int Id { get; set; }
        public DateTimeOffset CreatedOnUtc { get; set; }
        public DateTimeOffset? ModifiedOnUtc { get; set; }
    }
    
    private class TestDbContext : DbContext
    {
        public DbSet<AuditableEntity> AuditableEntities { get; set; }

        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuditableEntity>().ToTable("AuditableEntities");
        }
    }

    [Fact]
    public void SavingChanges_ShouldSetCreatedOnUtc_WhenEntityIsAdded()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase("TestDatabase")
            .AddInterceptors(new UpdateAuditableEntitiesInterceptor())
            .Options;

        using var context = new TestDbContext(options);
        var entity = new AuditableEntity();

        // Act
        context.AuditableEntities.Add(entity);
        context.SaveChanges();

        // Assert
        Assert.NotEqual(default(DateTimeOffset), entity.CreatedOnUtc);
        Assert.True(entity.CreatedOnUtc <= DateTimeOffset.UtcNow);
    }

    [Fact]
    public void SavingChanges_ShouldSetModifiedOnUtc_WhenEntityIsModified()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase("TestDatabase")
            .AddInterceptors(new UpdateAuditableEntitiesInterceptor())
            .Options;

        using var context = new TestDbContext(options);
        var entity = new AuditableEntity { CreatedOnUtc = DateTimeOffset.UtcNow };
        context.AuditableEntities.Add(entity);
        context.SaveChanges();

        // Act
        entity.ModifiedOnUtc = DateTimeOffset.Now.AddDays(-1);
        context.AuditableEntities.Update(entity);
        context.SaveChanges();

        // Assert
        Assert.NotNull(entity.ModifiedOnUtc);
        Assert.True(entity.ModifiedOnUtc <= DateTimeOffset.UtcNow);
    }

    [Fact]
    public async Task SavingChangesAsync_ShouldSetCreatedOnUtc_WhenEntityIsAdded()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase("TestDatabase")
            .AddInterceptors(new UpdateAuditableEntitiesInterceptor())
            .Options;

        using var context = new TestDbContext(options);
        var entity = new AuditableEntity();

        // Act
        context.AuditableEntities.Add(entity);
        await context.SaveChangesAsync();

        // Assert
        Assert.NotEqual(default(DateTimeOffset), entity.CreatedOnUtc);
        Assert.True(entity.CreatedOnUtc <= DateTimeOffset.UtcNow);
    }

    [Fact]
    public async Task SavingChangesAsync_ShouldSetModifiedOnUtc_WhenEntityIsModified()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase("TestDatabase")
            .AddInterceptors(new UpdateAuditableEntitiesInterceptor())
            .Options;

        using var context = new TestDbContext(options);
        var entity = new AuditableEntity { CreatedOnUtc = DateTimeOffset.UtcNow };
        context.AuditableEntities.Add(entity);
        await context.SaveChangesAsync();

        // Act
        entity.ModifiedOnUtc = DateTimeOffset.Now.AddDays(-1);
        context.AuditableEntities.Update(entity);
        await context.SaveChangesAsync();

        // Assert
        Assert.NotNull(entity.ModifiedOnUtc);
        Assert.True(entity.ModifiedOnUtc <= DateTimeOffset.UtcNow);
    }
}
