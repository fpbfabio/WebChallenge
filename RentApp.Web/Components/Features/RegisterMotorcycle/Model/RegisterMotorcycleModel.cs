namespace RentApp.Web.Components.Features.RegisterMotorcycle.Model;

public record RegisterMotorcycleModel
(
    string? Identifier = null,
    int? Year = null,
    string? ModelName = null,
    string? LicensePlate = null
);