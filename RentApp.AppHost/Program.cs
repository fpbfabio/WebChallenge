var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.RentApp_ApiService>("apiservice");

builder.AddProject<Projects.RentApp_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.Build().Run();
