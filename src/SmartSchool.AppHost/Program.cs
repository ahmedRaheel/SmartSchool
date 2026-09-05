var builder = DistributedApplication.CreateBuilder(args);

var identityApi =
    builder
        .AddProject<Projects.SmartSchoolIdentityApi>(
            "identity-api")
        .WithExternalHttpEndpoints();

builder
    .AddProject<Projects.SmartSchoolApi>(
        "smartschool-api")
    .WaitFor(identityApi)
    .WithReference(identityApi)
    .WithExternalHttpEndpoints();

builder.Build().Run();
