using MongoApi.Services;

namespace MongoApi.Models
{
    public class DatabaseSettings : IDatabaseSettings
    {
        public string? DatabaseName { get; set; }
        public string? ConnectionStrings { get; set; }
        public string? CollectionName { get; set; }
    }
}
