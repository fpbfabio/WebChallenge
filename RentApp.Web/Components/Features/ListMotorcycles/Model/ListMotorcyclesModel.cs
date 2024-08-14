namespace RentApp.Web.Components.Features.ListMotorcycles.Model;

public record ListMotorcyclesModel
(
    List<MotorcycleItemModel>? MotorcycleItemModels = null,
    string? EditingLicensePlate = null
);