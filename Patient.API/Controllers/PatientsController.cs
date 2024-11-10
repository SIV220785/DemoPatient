using Microsoft.AspNetCore.Mvc;
using Patient.BLL.Interfaces;
using Patient.BLL.Models;

namespace Patient.API.Controllers;

/// <summary>
/// Controller for managing patient data.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PatientsController(IPatientService patientService, ILogger<PatientsController> logger) : ControllerBase
{
    /// <summary>
    /// Retrieve all registered patients.
    /// </summary>
    /// <returns>A list of patient profiles.</returns>
    /// <response code="200">When patients are successfully retrieved.</response>
    /// <response code="500">If an internal server error occurs.</response>
    [HttpGet("all-patients")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<PatientProfileDto>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllPatients()
    {
        try
        {
            var patients = await patientService.GetAllPatientsAsync();
            logger.LogInformation("Number of patients retrieved: {PatientCount}", patients.Count());
            return Ok(patients);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to retrieve all patients.");
            return StatusCode(500, $"Failed to retrieve all patients: {ex.Message}");
        }
    }

    /// <summary>
    /// Retrieve a specific patient by ID.
    /// </summary>
    /// <param name="id">The unique identifier of the patient.</param>
    /// <returns>A single patient profile if found.</returns>
    /// <response code="200">When the patient is found.</response>
    /// <response code="404">If no patient is found with the specified ID.</response>
    /// <response code="500">If an internal server error occurs.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PatientProfileDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPatient(int id)
    {
        try
        {
            var patientDto = await patientService.GetPatientByIdAsync(id);
            if (patientDto == null)
            {
                return NotFound();
            }

            return Ok(patientDto);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    /// <summary>
    /// Retrieves patients filtered by a specific date.
    /// </summary>
    /// <param name="dateParameter">The date to filter patients by.</param>
    /// <returns>A list of patient profiles that match the specified date.</returns>
    /// <response code="200">Returns the matched patients.</response>
    /// <response code="400">If the date parameter is missing or invalid.</response>
    /// <response code="500">If an internal server error occurs.</response>
    [HttpGet("by-date")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<PatientProfileDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPatientsByDate([FromQuery] string dateParameter)
    {
        if (string.IsNullOrWhiteSpace(dateParameter))
        {
            return BadRequest("Date parameter is required.");
        }

        try
        {
            var patients = await patientService.GetPatientsByDateAsync(dateParameter);
            return Ok(patients);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while retrieving patients: {ex.Message}");
        }
    }

    /// <summary>
    /// Retrieves patients filtered by a specific period around a date.
    /// </summary>
    /// <param name="dateParameter">The central date to include in search range.</param>
    /// <param name="startPeriod">The starting date of the search period.</param>
    /// <param name="endPeriod">The ending date of the search period.</param>
    /// <returns>A list of patient profiles within the specified date and period.</returns>
    /// <response code="200">Returns the matched patients.</response>
    /// <response code="400">If any parameters are missing or invalid.</response>
    /// <response code="500">If an internal server error occurs.</response>
    [HttpGet("by-date-period")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<PatientProfileDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPatientsByDateWithPeriod([FromQuery] string dateParameter,
        [FromQuery] DateTime startPeriod, [FromQuery] DateTime endPeriod)
    {
        if (string.IsNullOrWhiteSpace(dateParameter))
        {
            return BadRequest("Date parameter is required.");
        }

        if (startPeriod == default || endPeriod == default)
        {
            return BadRequest("Both startPeriod and endPeriod parameters are required.");
        }

        try
        {
            var patients = await patientService.GetPatientsByDateWithPeriodAsync(dateParameter, startPeriod, endPeriod);
            return Ok(patients);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while retrieving patients: {ex.Message}");
        }
    }

    /// <summary>
    /// Creates a new patient profile.
    /// </summary>
    /// <param name="patientProfileDto">The patient profile to create.</param>
    /// <returns>The created patient profile.</returns>
    /// <response code="201">Returns the newly created patient profile.</response>
    /// <response code="400">If the input is invalid.</response>
    /// <response code="500">If an internal server error occurs.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PatientProfileDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreatePatient([FromBody] PatientProfileDto patientProfileDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var createdPatientDto = await patientService.CreatePatientAsync(patientProfileDto);
            return CreatedAtAction(nameof(GetPatient), new { id = createdPatientDto.Id }, createdPatientDto);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    /// <summary>
    /// Updates an existing patient profile.
    /// </summary>
    /// <param name="id">The unique identifier of the patient to update.</param>
    /// <param name="patientProfileDto">The updated patient profile data.</param>
    /// <returns>The updated patient profile.</returns>
    /// <response code="200">If the patient profile is successfully updated.</response>
    /// <response code="400">If the input is invalid.</response>
    /// <response code="404">If no patient is found with the specified ID.</response>
    /// <response code="500">If an internal server error occurs.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PatientProfileDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdatePatient(int id, [FromBody] PatientProfileDto patientProfileDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            patientProfileDto.Id = id;
            var updatedPatientDto = await patientService.UpdatePatientAsync(patientProfileDto);

            if (updatedPatientDto is null)
            {
                return NotFound($"No patient found with ID {id}");
            }

            return Ok(updatedPatientDto);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    /// <summary>
    /// Deletes a patient profile.
    /// </summary>
    /// <param name="id">The unique identifier of the patient to delete.</param>
    /// <returns>A status indicating the outcome of the operation.</returns>
    /// <response code="204">Indicates successful deletion with no content returned.</response>
    /// <response code="404">If no patient is found with the specified ID.</response>
    /// <response code="500">If an internal server error occurs.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeletePatient(int id)
    {
        try
        {
            var patientExists = await patientService.GetPatientByIdAsync(id);

            if (patientExists == null)
            {
                return NotFound($"No patient found with ID {id}");
            }

            await patientService.DeletePatientAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
