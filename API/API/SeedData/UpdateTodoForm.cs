using Newtonsoft.Json;

namespace API.Data.SeedData
{
    public class UpdateTodoForm
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("task")]
        public string Task { get; set; }

        [JsonProperty("done")]
        public bool Done { get; set; }

    }
}
