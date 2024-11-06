namespace Patient.BLL.Models;

public class PatientProfileDto
{
    public int Id { get; set; }

    public PatientDetailsDto PatientDetail { get; set; }

    public string Gender { get; set; }

    public DateTime BirthDate { get; set; }

    public bool IsActive { get; set; }
}
