using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SmartSchool.Modules.AIInquiry.Models;

namespace SmartSchool.Modules.AIInquiry.Persistence;

public interface IAIInquiryDbContext
{
    DatabaseFacade Database { get; }

    DbSet<HumanHandoffEntity> HumanHandoffs { get; }
    DbSet<InquiryConversationEntity> InquiryConversations { get; }
    DbSet<InquiryMessageEntity> InquiryMessages { get; }
    DbSet<LeadCaptureEntity> LeadCaptures { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// EF Core unit-of-work owned by the AIInquiry module.
/// This context is intentionally independent from ApplicationDbContext.
/// </summary>
public sealed class AIInquiryDbContext(DbContextOptions<AIInquiryDbContext> options)
    : DbContext(options), IAIInquiryDbContext
{
    public DbSet<HumanHandoffEntity> HumanHandoffs => Set<HumanHandoffEntity>();
    public DbSet<InquiryConversationEntity> InquiryConversations => Set<InquiryConversationEntity>();
    public DbSet<InquiryMessageEntity> InquiryMessages => Set<InquiryMessageEntity>();
    public DbSet<LeadCaptureEntity> LeadCaptures => Set<LeadCaptureEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(AIInquiryDbContext).Assembly,
            type => type.Namespace is not null
                && type.Namespace.StartsWith("SmartSchool.Modules.AIInquiry.Persistence.Configurations", StringComparison.Ordinal));
    }
}
