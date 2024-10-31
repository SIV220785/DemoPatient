using System.ComponentModel.DataAnnotations;

namespace Patient.DAL.Entities;

public class Active
{
    [Key]
    public int Id { get; set; }

    public bool IsActive { get; set; }
}