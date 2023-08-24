namespace MongoApi.Services
{
    public interface IDatabaseSettings
    {
        string DatabaseName { get; set; }
        string ConnectionStrings { get; set; }
        string CollectionName { get; set; }

    }
}
