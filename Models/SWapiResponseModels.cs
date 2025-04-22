using System.Text.Json.Serialization;

namespace ResponseModels
{
    public class PersonProperties
    {
        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonPropertyName("height")]
        public required string Height { get; set; }

        [JsonPropertyName("mass")]
        public required string Mass { get; set; }

        [JsonPropertyName("birth_year")]
        public required string BirthYear { get; set; }
    }

    public class PersonResult
    {
        [JsonPropertyName("properties")]
        public required PersonProperties Properties { get; set; }
    }

    public class SWapiResponse
    {
        [JsonPropertyName("result")]
        public required PersonResult Result { get; set; }
    }
}