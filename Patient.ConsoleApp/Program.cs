using System.Text;
using Bogus;
using Newtonsoft.Json;
using Patient.BLL.Models;

namespace Patient.ConsoleApp;

class Program
{
    private static readonly HttpClient Client = new();

    private static async Task Main()
    {
        var baseUri = "http://localhost:5116/api/patients";

        var patientGenerator = new Faker<PatientProfileDto>()
            .RuleFor(p => p.PatientDetail, f => new PatientDetailsDto
            {
                Id = Guid.NewGuid(),
                Use = f.PickRandom("official", "personal", "temporary", "anonymous"),
                Family = f.Name.LastName(),
                Givens = GenerateRandomGivens(f)
            })
            .RuleFor(p => p.Gender, f => f.PickRandom("male", "female", "other", "unknown"))
            .RuleFor(p => p.BirthDate, f => f.Date.Past(30))
            .RuleFor(p => p.IsActive, f => f.Random.Bool());

        for (var i = 0; i < 100; i++)
        {
            var patient = patientGenerator.Generate();

            var json = JsonConvert.SerializeObject(patient);

            using var content = new StringContent(json, Encoding.UTF8, "application/json");

            var uri = new Uri(baseUri);
            var response = await Client.PostAsync(uri, content);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Patient {i + 1} added.");
            }
            else
            {
                Console.WriteLine($"Failed to add Patient {i + 1}: {response.StatusCode}");
            }
        }
    }
    private static IEnumerable<string> GenerateRandomGivens(Faker faker)
    {
        var count = faker.Random.Int(1, 5);

        return Enumerable.Range(0, count).Select(_ => faker.Name.FirstName()).ToList();
    }
}
