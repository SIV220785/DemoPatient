namespace Patient.BLL.Models;

public class PatientDetailsDto
{
    public Guid Id { get; set; }

    public string Use { get; set; }

    public string Family { get; set; }

    public IEnumerable<string> Givens { get; set; }
}
