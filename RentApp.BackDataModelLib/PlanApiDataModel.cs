namespace RentApp.BackDataModelLib;

public class PlanApiDataModel
{
    public int Id { set; get; }
    public string Description { set; get; } = "";
    public float PricePerDay { set; get; }
    public int Days { set; get; }
}