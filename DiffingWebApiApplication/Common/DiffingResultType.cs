using System.Text.Json.Serialization;

namespace DiffingWebApiApplication
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DiffingResultType
    {
        Equals,
        SizeDoNotMatch,
        ContentDoNotMatch
    }
}