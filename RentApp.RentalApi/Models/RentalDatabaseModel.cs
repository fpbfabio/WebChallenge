using System.ComponentModel.DataAnnotations.Schema;


namespace RentApp.RentalApi.Models;

[Table("rental")]
public class RentalDatabaseModel
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("_id")]
    public string? Id { set; get; }
    [Column("user_id")]
    public string? UserId { set; get; }
    [Column("plan_id")]
    public int PlanId { set; get; }
    [Column("start_date")]
    public int? StartDate { set; get; }
    [Column("end_date")]
    public int? EndDate { set; get; }
}