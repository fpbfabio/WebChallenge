using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentApp.MotorcycleApi.Models;

[Table("motorcycles")]
public class MotorcycleDatabaseModel
{
    [Key, Column("_id")]
    public string? LicensePlate { set; get; }
    [Column("identifier")]
    public string? Identifier { set; get; }
    [Column("year")]
    public int Year { set; get; }
    [Column("model_name")]
    public string? ModelName { set; get; }
}