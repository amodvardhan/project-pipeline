namespace ProjectPipeline.Core.DTOs.Response
{
    /// <summary>
    /// Business unit response DTO
    /// </summary>
    public class BusinessUnitResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? HeadOfUnit { get; set; }
        public bool IsActive { get; set; }
    }
}
