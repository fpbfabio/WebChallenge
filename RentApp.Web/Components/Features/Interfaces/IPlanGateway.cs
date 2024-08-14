using RentApp.Web.Components.Features.Rent.Model;

namespace RentApp.Web.Components.Features.Interfaces;


public interface IPlanGateway
{
    void GetAvailablePlans(Action<List<PlanModel>> onResult, Action<string> onError);
}