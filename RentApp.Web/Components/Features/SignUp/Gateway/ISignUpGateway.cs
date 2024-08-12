using RentApp.Web.Components.Features.SignUp.Model;

namespace RentApp.Web.Components.Features.SignUp.Gateway;

public interface ISignUpGateway
{
    void RegisterDriver(SignUpModel model);
}