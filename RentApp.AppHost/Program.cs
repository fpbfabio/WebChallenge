var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.RentApp_ApiService>("apiservice");

var keycloak = builder.AddKeycloak("keycloak", 8080);
builder.AddProject<Projects.RentApp_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(keycloak)
    .WithReference(apiService);

builder.Build().Run();
