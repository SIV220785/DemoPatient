using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Patient.DAL.Entities;

public class Patient
{
    [Key]
    public int Id { get; set; }

    [Required]
    [Column(TypeName = "date")]
    public DateTime BirthDate { get; set; }

    [Required]
    [ForeignKey("Name")]
    public Guid NameId { get; set; }
    public Name Name { get; set; }
    

    [ForeignKey("Gender")]
    public int GenderId { get; set; }
    public Gender Gender { get; set; }

    [ForeignKey("Active")]
    public int ActiveId { get; set; }
    public Active Active { get; set; }
}