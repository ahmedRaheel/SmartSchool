namespace SmartSchool.Api.Seed;

public sealed class SampleActorSeeder(
    ILogger<SampleActorSeeder> logger)
{
    public Task SeedAsync()
    {
        foreach (var actor in SampleActors.All)
        {
            logger.LogInformation(
                "Development actor {UserName} with role {Role} and reference {ReferenceNumber}",
                actor.UserName,
                actor.Role,
                actor.ReferenceNumber);
        }

        return Task.CompletedTask;
    }
}
