namespace RentApp.Web.Components.Features.ListMotorcycles.Model;

public record MotorcycleItemModel
(
    string? Identifier = null,
    int? Year = null,
    string? ModelName = null,
    string? LicensePlate = null
);