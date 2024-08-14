namespace RentApp.Web.Components.Features.RegisterMotorcycle.Model;

public record RegisterMotorcycleModel
(
    string? Identifier = null,
    DateOnly? Year = null,
    string? ModelName = null,
    string? LicensePlate = null
);