﻿namespace MediBuyApi.Models.DTO
{
    public class LoginResponseDTO
    {
        public string UserId { get; set; }
        public string JwtToken { get; set; }
        public List<string> Roles { get; set; }
    }
}
