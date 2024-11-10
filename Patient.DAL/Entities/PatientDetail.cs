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
    public ICollection<Given> Givens { get; } = new HashSet<Given>();

    public void ClearGivens()
    {
        Givens.Clear();
    }

    public void ReplaceGivens(IEnumerable<Given> newGivens)
    {
        Givens.Clear();
        foreach (var given in newGivens)
        {
            Givens.Add(given);
        }
    }
}
