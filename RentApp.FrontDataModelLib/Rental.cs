namespace RentApp.FrontDataModelLib;


public class Rental
{
    public string? Id { set; get; }
    public Plan? Plan { set; get; }
    public int? StartDate { set; get; }
}