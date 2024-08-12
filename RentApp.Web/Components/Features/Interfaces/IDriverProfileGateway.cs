using RentApp.Web.Components.Features.RegisterProfile.Model;

namespace RentApp.Web.Components.Features.Interfaces;

public interface IDriverProfileGateway
{
    void RegisterProfile(string id, RegisterProfileModel model);
    void ProfileExists(string id, Action<bool> onResult, Action<string> onError);
}