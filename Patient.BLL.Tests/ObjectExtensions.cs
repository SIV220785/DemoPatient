using Newtonsoft.Json;

namespace Patient.BLL.Tests;

public static class ObjectExtensions
{
    public static string ToJson(object value) => JsonConvert.SerializeObject(value, Formatting.Indented);
}
