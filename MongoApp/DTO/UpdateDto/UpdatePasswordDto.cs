namespace MongoApp.DTO.UpdateDto
{
    public class UpdatePasswordDto
    {
        public string? Id { get; set; }
        public string? Curr_Password { get; set; }
        public string? New_Password { get; set; }
    }
}
