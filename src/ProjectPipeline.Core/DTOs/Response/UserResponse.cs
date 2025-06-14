namespace ProjectPipeline.Core.DTOs.Response
{
    /// <summary>
    /// User response DTO
    /// </summary>
    public class UserResponse
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Department { get; set; }
        public string? Designation { get; set; }
        public int? BusinessUnitId { get; set; }
        public string? BusinessUnitName { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; } // Changed from string to DateTime
        public DateTime? LastLoginDate { get; set; } // Changed from string to DateTime?
    }
}
