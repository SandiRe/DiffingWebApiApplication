using System.Text.Json.Serialization;

namespace DiffingWebApiApplication
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DiffingResultIype
    {
        Equals,
        SizeDoNotMatch,
        ContentDoNotMatch
    }
}