namespace Application.Domain.Dtos
{
    using System.Text.Json.Serialization;

    public class IpLocationDto
    {
        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonPropertyName("country")]
        public string? Country { get; set; }

        [JsonPropertyName("countryCode")]
        public string? CountryCode { get; set; }

        [JsonPropertyName("region")]
        public string? Region { get; set; }

        [JsonPropertyName("regionName")]
        public string? RegionName { get; set; }

        [JsonPropertyName("city")]
        public string? City { get; set; }

        [JsonPropertyName("zip")]
        public string? Zip { get; set; }

        [JsonPropertyName("lat")]
        public double? Latitude { get; set; }

        [JsonPropertyName("lon")]
        public double? Longitude { get; set; }

        [JsonPropertyName("timezone")]
        public string? Timezone { get; set; }

        [JsonPropertyName("isp")]
        public string? ISP { get; set; }

        [JsonPropertyName("org")]
        public string? Org { get; set; }

        [JsonPropertyName("as")]
        public string? AS { get; set; }

        [JsonPropertyName("query")]
        public string? Query { get; set; }
    }
}
