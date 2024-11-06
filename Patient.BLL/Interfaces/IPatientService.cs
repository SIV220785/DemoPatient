using Patient.BLL.Models;

namespace Patient.BLL.Interfaces
{
    public interface IPatientService
    {
        Task<PatientProfileDto> CreatePatientAsync(PatientProfileDto patientProfileDto);

        Task<PatientProfileDto> GetPatientByIdAsync(int id);

        Task<IEnumerable<PatientProfileDto>> GetAllPatientsAsync();

        Task DeletePatientAsync(int id);

        Task<IEnumerable<PatientProfileDto>> GetPatientsByDateAsync(string dateParameter);

        Task<IEnumerable<PatientProfileDto>> GetPatientsByDateWithPeriodAsync(string dateParameter,
            DateTime startPeriod = default, DateTime endPeriod = default);

        Task<PatientProfileDto> UpdatePatientAsync(PatientProfileDto patientProfileDto);
    }
}
