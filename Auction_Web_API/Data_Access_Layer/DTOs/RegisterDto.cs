﻿

namespace Data_Access_Layer.DTOs
{
    public class RegisterDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string? Role { get; set; }
        public bool IsBanned { get; set; }
    }
}
