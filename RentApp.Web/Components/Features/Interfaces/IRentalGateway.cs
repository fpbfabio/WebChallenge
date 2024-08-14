using RentApp.Web.Components.Features.Rent.Model;

namespace RentApp.Web.Components.Features.Interfaces;


public interface IRentalGateway
{
    void StartNewRental(string userId, PlanModel planModel, Action onSuccess, Action<string> onError);
    void HasActiveRental(string userId, Action<bool> onResult, Action<string> onError);
    void GetActiveUserRental(string userId, Action<RentalModel?> onResult, Action<string> onError);
    void GetPriceForDate(string rentalId, DateOnly endDate, Action<float> onResult, Action<string> onError);
    void EndRental(string rentalId, DateOnly endDate, Action onResult, Action<string> onError);
}