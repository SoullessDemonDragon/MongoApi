﻿namespace MongoApi.DTO
{
    public class CreateUserDto
    {
        public string? Id { get; set; }
        public string? Name { get; set; }

        public string? UserName { get; set; }

        public int Age { get; set; }

        public string? Email { get; set; }

        public string? Phonenumber { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
        public string? Status { get; set; }
    }
}
