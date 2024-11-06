using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Patient.DAL.Entities;

public class Given
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; }

    [ForeignKey("PatientDetail")]
    public Guid PatientDetailsId { get; set; }

    public PatientDetail PatientDetail { get; set; }
}
