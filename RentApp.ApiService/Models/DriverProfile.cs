using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentApp.ApiService.Models;

[Table("driver_profile")]
public class DriverProfile
{
    [Column("_id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public Guid Id { get; set; }
    [Column("name")]
    public string? Name { get; set; }
    [Column("company_code")]
    public string? CompanyCode { get; set; }
    [Column("birth_date")]
    public string? BirthDate { get; set; }
    [Column("license_code")]
    public string? DriverLicenseCode { get; set; }
    [Column("category")]
    public string? Category { get; set; }
}