var builder = DistributedApplication.CreateBuilder(args);
var mongoDb = builder.AddMongoDb("mongodb");
var driverApiService = builder.AddProject<Projects.RentApp_DriverApi>("driverapi")
    .WithReference(mongoDb);
var rentalApiService = builder.AddProject<Projects.RentApp_RentalApi>("rentalapi")
    .WithReference(mongoDb);
var planApiService = builder.AddProject<Projects.RentApp_PlanApi>("planapi");
var motorcycleApiService = builder.AddProject<Projects.RentApp_MotorcycleApi>("motorcycleapi")
    .WithReference(mongoDb);
var apiService = builder.AddProject<Projects.RentApp_ApiService>("apiservice")
    .WithReference(driverApiService)
    .WithReference(rentalApiService)
    .WithReference(planApiService)
    .WithReference(motorcycleApiService);

var keycloak = builder.AddKeycloak("keycloak", 8080)
    .WithRealmImport("../realms");

builder.AddProject<Projects.RentApp_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(keycloak)
    .WithReference(apiService);

builder.Build().Run();
