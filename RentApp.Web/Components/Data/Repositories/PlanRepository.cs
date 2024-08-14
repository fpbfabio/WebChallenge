using RentApp.Web.Components.Data.Converters;
using RentApp.Web.Components.Data.Source;
using RentApp.Web.Components.Features.Interfaces;
using RentApp.Web.Components.Features.Rent.Model;

namespace RentApp.Web.Components.Data.Repositories;

public class PlanRepository(PlanRemoteDataSource planRemoteDataSource) : IPlanGateway
{
    private PlanRemoteDataSource DataSource => planRemoteDataSource;


    public void GetAvailablePlans(Action<List<PlanModel>> onResult, Action<string> onError)
    {
        DataSource.GetAvailablePlans((datas) =>
        {
            var models = from x in datas select PlanModelConverter.ToPlanModel(x);
            onResult(models.ToList());
        }, onError);
    }
}