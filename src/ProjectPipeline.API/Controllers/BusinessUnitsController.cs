using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectPipeline.Core.DTOs.Common;
using ProjectPipeline.Core.DTOs.Response;
using ProjectPipeline.Infrastructure.Data.Context;

namespace ProjectPipeline.API.Controllers
{
    /// <summary>
    /// Business Units controller for managing organizational units
    /// </summary>
    [Route("api/business-units")]
    [ApiController]
    [Authorize]
    public class BusinessUnitsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<BusinessUnitsController> _logger;

        public BusinessUnitsController(ApplicationDbContext context, ILogger<BusinessUnitsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Get all business units
        /// </summary>
        /// <returns>List of business units</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<BusinessUnitResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBusinessUnits()
        {
            try
            {
                var businessUnits = await _context.BusinessUnits
                    .Where(bu => bu.IsActive && !bu.IsDeleted)
                    .Select(bu => new BusinessUnitResponse
                    {
                        Id = bu.Id,
                        Name = bu.Name,
                        Code = bu.Code,
                        Description = bu.Description,
                        HeadOfUnit = bu.HeadOfUnit,
                        IsActive = bu.IsActive
                    })
                    .ToListAsync();

                return Ok(ApiResponse<IEnumerable<BusinessUnitResponse>>.Success(businessUnits));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving business units");
                return StatusCode(500, ApiResponse<IEnumerable<BusinessUnitResponse>>.Failure("An error occurred while retrieving business units"));
            }
        }

        /// <summary>
        /// Get business unit by ID
        /// </summary>
        /// <param name="id">Business unit ID</param>
        /// <returns>Business unit details</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<BusinessUnitResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBusinessUnit(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(ApiResponse<string>.Failure("Invalid business unit ID"));
                }

                var businessUnit = await _context.BusinessUnits
                    .Where(bu => bu.Id == id && bu.IsActive && !bu.IsDeleted)
                    .Select(bu => new BusinessUnitResponse
                    {
                        Id = bu.Id,
                        Name = bu.Name,
                        Code = bu.Code,
                        Description = bu.Description,
                        HeadOfUnit = bu.HeadOfUnit,
                        IsActive = bu.IsActive
                    })
                    .FirstOrDefaultAsync();

                if (businessUnit == null)
                {
                    return NotFound(ApiResponse<string>.Failure("Business unit not found"));
                }

                return Ok(ApiResponse<BusinessUnitResponse>.Success(businessUnit));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving business unit {BusinessUnitId}", id);
                return StatusCode(500, ApiResponse<BusinessUnitResponse>.Failure("An error occurred while retrieving the business unit"));
            }
        }
    }
}
