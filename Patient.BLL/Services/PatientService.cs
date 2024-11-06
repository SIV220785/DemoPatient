
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Patient.BLL.Interfaces;
using Patient.BLL.Models;
using Patient.BLL.Services.Base;
using Patient.BLL.Utilities;
using Patient.DAL.Entities;
using Patient.DAL.Interfaces;

namespace Patient.BLL.Services
{
    public class PatientService : HealthRecordsService<PatientProfile>, IPatientService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PatientService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork)
        {
            ArgumentNullException.ThrowIfNull(mapper);

            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PatientProfileDto> CreatePatientAsync(PatientProfileDto patientProfileDto)
        {
            ArgumentNullException.ThrowIfNull(patientProfileDto);

            var patientProfile = _mapper.Map<PatientProfile>(patientProfileDto);

            await CheckData(patientProfile);

            var patient = await CreateAsync(patientProfile);

            var patientDto = _mapper.Map<PatientProfileDto>(patient);
            return patientDto;
        }

        public async Task<PatientProfileDto> GetPatientByIdAsync(int id)
        {
            var patient = await _unitOfWork.PatientProfiles
                .Include(query => query
                    .Include(p => p.Gender)
                    .Include(p => p.Active)
                    .Include(p => p.PatientDetail)
                        .ThenInclude(pd => pd.Givens))
                .FirstOrDefaultAsync(p => p.Id == id);

            var patientDto = _mapper.Map<PatientProfileDto>(patient);

            return patientDto;
        }
   
        public async Task<IEnumerable<PatientProfileDto>> GetAllPatientsAsync()
        {
            var patients = await _unitOfWork.PatientProfiles
                .Include(query => query
                    .Include(p => p.Gender)
                    .Include(p => p.Active)
                    .Include(p => p.PatientDetail)
                        .ThenInclude(pd => pd.Givens))
                .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<PatientProfileDto>>(patients);

            return dtos;
        }

        public async Task DeletePatientAsync(int id) => await DeleteAsync(id);

        public async Task<IEnumerable<PatientProfileDto>> GetPatientsByDateAsync(string dateParameter)
        {
            var expression = DateFilterExpressionBuilder.BuildExpression(dateParameter);

            var patients = await _unitOfWork.PatientProfiles
                .Include(query => query
                    .Include(p => p.Gender)
                    .Include(p => p.Active)
                    .Include(p => p.PatientDetail)
                        .ThenInclude(pd => pd.Givens))
                .Where(expression).ToListAsync();

            var dtos = _mapper.Map<IEnumerable<PatientProfileDto>>(patients);
            return dtos;
        }

        public async Task<IEnumerable<PatientProfileDto>> GetPatientsByDateWithPeriodAsync(string dateParameter,
            DateTime startPeriod = default, DateTime endPeriod = default)
        {
            var expression = DateFilterExpressionBuilder.BuildExpressionByPeriod(dateParameter, startPeriod, endPeriod);

            var patients = await _unitOfWork.PatientProfiles
                .Include(query => query
                    .Include(p => p.Gender)
                    .Include(p => p.Active)
                    .Include(p => p.PatientDetail)
                        .ThenInclude(pd => pd.Givens))
                .Where(expression).ToListAsync();

            var dtos = _mapper.Map<IEnumerable<PatientProfileDto>>(patients);

            return dtos;
        }


        public async Task<PatientProfileDto> UpdatePatientAsync(PatientProfileDto patientProfileDto)
        {
            var patientProfile = _mapper.Map<PatientProfile>(patientProfileDto);

            await CheckData(patientProfile);

            await _unitOfWork.PatientDetails.UpdateAsync(patientProfile.PatientDetail);

            var updatedPatient = await UpdateAsync(patientProfile);

            var dtos = _mapper.Map<PatientProfileDto>(updatedPatient);

            return dtos;
        }

        private async Task CheckData(PatientProfile patientProfile)
        {
            if (patientProfile.Gender != null)
            {
                var existingGenders = await _unitOfWork.Genders.GetAllAsync();

                if (existingGenders != null)
                {
                    var gender = existingGenders.FirstOrDefault(g => g.Type.Equals(patientProfile.Gender.Type, StringComparison.OrdinalIgnoreCase));
                    patientProfile.Gender = gender;

                }
            }

            if (patientProfile.Active != null)
            {
                var existingActive = await _unitOfWork.Actives.GetAllAsync();

                if (existingActive != null)
                {
                    var active = existingActive.FirstOrDefault(g => g.IsActive.Equals(patientProfile.Active.IsActive));
                    patientProfile.Active = active;
                }
            }

            if (patientProfile.PatientDetail.Givens.Any())
            {
                var existingGivens = await _unitOfWork.Givens.GetAllAsync();

                IEnumerable<Given> enumerable = existingGivens as Given[] ?? existingGivens.ToArray();

                if (enumerable.Any())
                {
                    var givens = enumerable.Where(x => x.PatientDetailsId.Equals(patientProfile.PatientDetailId));

                    var uniqueGivens = patientProfile.PatientDetail.Givens.Concat(givens)
                        .GroupBy(given => given.Name)
                        .Where(group => group.Count() == 1)
                        .Select(group => group.First())
                        .ToList();

                    patientProfile.PatientDetail.Givens = uniqueGivens;
                }
            }

            foreach (var given in patientProfile.PatientDetail.Givens)
            {
                given.PatientDetail = patientProfile.PatientDetail;
            }
        }
    }
}
