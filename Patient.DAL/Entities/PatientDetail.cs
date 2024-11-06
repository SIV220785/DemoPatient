using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Patient.DAL.Entities;

public class PatientDetail
{
    [Key]
    public Guid Id { get; set; }

    public string Use { get; set; }

    [Required]
    public string Family { get; set; }

    [InverseProperty("PatientDetail")]
    public ICollection<Given> Givens { get; set; } = new HashSet<Given>();
}
