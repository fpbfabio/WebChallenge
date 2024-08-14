using System.Globalization;
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
                SelectedPlan: PlanModelConverter.ToPlanModel(data.Plan),
                StartDate: DateOnly.ParseExact(data.StartDate,  "yyyy.MM.dd", CultureInfo.InvariantCulture)
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
}