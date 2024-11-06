using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.VisualBasic;

namespace Patient.DAL.Entities;

public class PatientProfile
{
    [Key]
    public int Id { get; set; }

    [Required]
    [Column(TypeName = "date")]
    public DateTime RecordDate { get; set; }

    [Required]
    [ForeignKey("PatientDetail")]
    public Guid PatientDetailId { get; set; }
    public PatientDetail PatientDetail { get; set; }

    [ForeignKey("Gender")]
    public int GenderId { get; set; }
    public Gender Gender { get; set; }

    [ForeignKey("Active")]
    public int ActiveId { get; set; }

    public Active Active { get; set; }
}
