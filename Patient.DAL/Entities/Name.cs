using System.ComponentModel.DataAnnotations;

namespace Patient.DAL.Entities;

public class Name
{
    [Key]
    public Guid Id { get; set; }

    public string Use { get; set; }

    [Required]
    public string Family { get; set; }

    public List<string> Given { get; set; }
}