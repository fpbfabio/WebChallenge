using RentApp.Web.Components.Data.Converters;
using RentApp.Web.Components.Data.Source;
using RentApp.Web.Components.Features.Interfaces;
using RentApp.Web.Components.Features.Rent.Model;

namespace RentApp.Web.Components.Data.Repositories;

public class RentalRepository(RentalRemoteDataSource rentalRemoteDataSource) : IRentalGateway
{
    private RentalRemoteDataSource RentalDataSource => rentalRemoteDataSource;

    public void StartNewRental(string userId, PlanModel planModel, Action onSuccess, Action<string> onError)
    {
        RentalDataSource.StartNewRental(userId, planModel.Id, onSuccess, onError);
    }

    public void GetActiveUserRental(string userId, Action<RentalModel?> onResult, Action<string> onError)
    {
        RentalDataSource.GetActiveUserRental(userId, (data) =>
        {
            if (data is null)
            {
                onResult(null);
                return;
            }
            if (data.Plan is null || data.StartDate is null)
            {
                onError("Malformed data, rental needs to be associated with a plan and have a start date");
                return;
            }
            RentalModel rentalModel = new(
                Id:  data.Id,
                SelectedPlan: PlanModelConverter.ToPlanModel(data.Plan),
                StartDate: DateOnly.FromDayNumber((int)data.StartDate)
            );
            onResult(rentalModel);
        }, onError);
    }

    public void HasActiveRental(string userId, Action<bool> onResult, Action<string> onError)
    {
        GetActiveUserRental(
            userId: userId,
            onResult: (data) =>
            {
                onResult(data != null);
            },
            onError: onError);
    }

    public void GetPriceForDate(string rentalId, DateOnly endDate, Action<float> onResult, Action<string> onError)
    {
        RentalDataSource.GetPriceForDate(
            rentalId,
            endDate.DayNumber,
            onResult,
            onError);
    }

    public void EndRental(string rentalId, DateOnly endDate, Action onResult, Action<string> onError)
    {
        RentalDataSource.EndRental(
            rentalId,
            endDate.DayNumber,
            onResult,
            onError);        
    }
}