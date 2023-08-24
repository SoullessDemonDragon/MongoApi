namespace MongoApi.DTO
{
    public class UpdateUserPasswordDto
    {
        public string? Id { get; set; }
        public string? Curr_Password { get; set; }
        public string? New_Password { get; set; }
    }
}
