using System.ComponentModel.DataAnnotations;

namespace Patient.DAL.Entities;

public class Gender
{
    [Key]
    public int Id { get; set; }

    public string Type { get; set; }
}
