using Microsoft.EntityFrameworkCore;
using SmartSchool.Infrastructure.Persistence;
using SmartSchool.Modules.AICore.Persistence;
using SmartSchool.Modules.AIInquiry.Persistence;
using SmartSchool.Modules.AIParent.Persistence;
using SmartSchool.Modules.AIPrediction.Persistence;
using SmartSchool.Modules.AITutor.Persistence;
using SmartSchool.Modules.Activities.Persistence;
using SmartSchool.Modules.Admissions.Persistence;
using SmartSchool.Modules.Audit.Persistence;
using SmartSchool.Modules.Communication.Persistence;
using SmartSchool.Modules.Documents.Persistence;
using SmartSchool.Modules.Examinations.Persistence;
using SmartSchool.Modules.Finance.Persistence;
using SmartSchool.Modules.HR.Persistence;
using SmartSchool.Modules.Inventory.Persistence;
using SmartSchool.Modules.Learning.Persistence;
using SmartSchool.Modules.Library.Persistence;
using SmartSchool.Modules.Organization.Persistence;
using SmartSchool.Modules.Reference.Persistence;
using SmartSchool.Modules.Students.Persistence;
using SmartSchool.Modules.Transport.Persistence;
using SmartSchool.Modules.Workflow.Persistence;

internal static class ModuleDbContexts
{
    public static IServiceCollection AddModuleDbContexts(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        Add<AICoreDbContext>(services, configuration);
        Add<AIInquiryDbContext>(services, configuration);
        Add<AIParentDbContext>(services, configuration);
        Add<AIPredictionDbContext>(services, configuration);
        Add<AITutorDbContext>(services, configuration);
        Add<ActivitiesDbContext>(services, configuration);
        Add<AdmissionsDbContext>(services, configuration);
        Add<AuditDbContext>(services, configuration);
        Add<CommunicationDbContext>(services, configuration);
        Add<DocumentsDbContext>(services, configuration);
        Add<ExaminationsDbContext>(services, configuration);
        Add<FinanceDbContext>(services, configuration);
        Add<HRDbContext>(services, configuration);
        Add<InventoryDbContext>(services, configuration);
        Add<LearningDbContext>(services, configuration);
        Add<LibraryDbContext>(services, configuration);
        Add<OrganizationDbContext>(services, configuration);
        Add<ReferenceDbContext>(services, configuration);
        Add<StudentsDbContext>(services, configuration);
        Add<TransportDbContext>(services, configuration);
        Add<WorkflowDbContext>(services, configuration);

        return services;
    }

    private static void Add<TContext>(
        IServiceCollection services,
        IConfiguration configuration)
        where TContext : DbContext
    {
        var persistence = configuration
            .GetSection(PersistenceOptions.SectionName)
            .Get<PersistenceOptions>() ?? new PersistenceOptions();

        var connectionString = configuration.GetConnectionString(
            persistence.ConnectionStringName);

        services.AddDbContext<TContext>(options =>
        {
            switch (persistence.Provider)
            {
                case PersistenceProvider.Mock:
                    options.UseInMemoryDatabase($"SmartSchool-{typeof(TContext).Name}");
                    break;

                case PersistenceProvider.PostgreSql:
                    EnsureConnectionString(connectionString, persistence.ConnectionStringName);
                    options.UseNpgsql(connectionString, provider => provider.EnableRetryOnFailure(5));
                    break;

                case PersistenceProvider.SqlServer:
                    EnsureConnectionString(connectionString, persistence.ConnectionStringName);
                    options.UseSqlServer(connectionString, provider => provider.EnableRetryOnFailure(5));
                    break;

                default:
                    throw new InvalidOperationException(
                        $"Unsupported persistence provider '{persistence.Provider}'.");
            }

            if (persistence.EnableSensitiveDataLogging)
            {
                options.EnableSensitiveDataLogging();
            }
        });
    }

    private static void EnsureConnectionString(string? connectionString, string name)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                $"Connection string '{name}' is required for the configured persistence provider.");
        }
    }
}
