namespace RentApp.Web.Components.Features.SignUp.Model;


public record SignUpModel
(
    string Name = "",
    string CompanyCode = "",
    DateOnly BirthDate = new(),
    string DriverLicenseCode = "",
    string Category = ""
);