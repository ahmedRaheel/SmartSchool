var builder = DistributedApplication.CreateBuilder(args);

// Infrastructure is already running in Docker. Aspire orchestrates the two .NET hosts
// while their normal configuration points to localhost Docker ports.
var identity = builder.AddProject<Projects.SmartSchool_Identity_Api>("identity-api")
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development");

builder.AddProject<Projects.SmartSchool_Api>("smartschool-api")
    .WithReference(identity)
    .WaitFor(identity)
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development");

builder.Build().Run();
