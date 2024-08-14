var builder = DistributedApplication.CreateBuilder(args);

var driverApiService = builder.AddProject<Projects.RentApp_DriverApi>("driverapi");
var rentalApiService = builder.AddProject<Projects.RentApp_RentalApi>("rentalapi");
var planApiService = builder.AddProject<Projects.RentApp_PlanApi>("planapi");
var apiService = builder.AddProject<Projects.RentApp_ApiService>("apiservice")
    .WithReference(driverApiService)
    .WithReference(rentalApiService)
    .WithReference(planApiService);

var keycloak = builder.AddKeycloak("keycloak", 8080);
builder.AddProject<Projects.RentApp_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(keycloak)
    .WithReference(apiService);

builder.Build().Run();
