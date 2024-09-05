﻿namespace TPOS.Api.Dtos
{
    public class ContactInfoDto
    {
        public int ContactID { get; set; }
        public required string Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
    }
}
