namespace RentApp.Web.Components.Features.RegisterProfile.Model;


public record RegisterProfileModel
(
    string Name = "",
    string CompanyCode = "",
    DateOnly BirthDate = new(),
    string DriverLicenseCode = "",
    string Category = ""
);