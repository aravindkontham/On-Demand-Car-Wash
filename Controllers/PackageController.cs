using AutoMapper;
using CarWash.DTO;
using CarWash.Interfaces;
using CarWash.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CarWash.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Customer")]
    public class PackageController : ControllerBase
    {
        private readonly IPackageRepository _packageRepository;
        private readonly UserManager<User> _userManager;
        private readonly ILogService _logService;
        private readonly IMapper _mapper;

        public PackageController(
            IPackageRepository packageRepository,
            UserManager<User> userManager,
            ILogService logService,
            IMapper mapper)
        {
            _packageRepository = packageRepository;
            _userManager = userManager;
            _logService = logService;
            _mapper = mapper;
        }

        [HttpPost("Add_Package")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddPackage([FromBody] PackageDto dto)
        {
            try
            {
                var currentUserId = GetUserId();

                var package = _mapper.Map<Package>(dto);
                var added = await _packageRepository.AddPackageAsync(package);

                await _logService.LogAsync("Info", $"Package added: {added.Code}", null, currentUserId);
                return Ok(_mapper.Map<PackageDto>(added));
            }
            catch (Exception ex)
            {
                await _logService.LogAsync("Error", "Error adding package", ex.Message, GetUserId());
                return StatusCode(500, "An error occurred while adding the package.");
            }
        }

        [HttpPut("Update_Package/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdatePackage(int id, [FromBody] PackageDto dto)
        {
            try
            {
                var currentUserId = GetUserId();
                var existing = await _packageRepository.GetPackageByIdAsync(id);
                if (existing == null)
                    return NotFound("Package not found.");

                _mapper.Map(dto, existing);
                var updated = await _packageRepository.UpdatePackageAsync(existing);

                if (!updated)
                    return StatusCode(500, "Failed to update package.");

                await _logService.LogAsync("Info", $"Package updated (ID: {id})", null, currentUserId);
                return Ok(_mapper.Map<PackageDto>(existing));
            }
            catch (Exception ex)
            {
                await _logService.LogAsync("Error", $"Error updating package ID {id}", ex.Message, GetUserId());
                return StatusCode(500, "An error occurred while updating the package.");
            }
        }

        [HttpGet("View_Packages")]
        public async Task<IActionResult> GetAllPackages()
        {
            try
            {
                var packages = await _packageRepository.GetAllPackagesAsync();
                return Ok(_mapper.Map<IEnumerable<PackageDto>>(packages));
            }
            catch (Exception ex)
            {
                await _logService.LogAsync("Error", "Error retrieving packages", ex.Message, GetUserId());
                return StatusCode(500, "An error occurred while fetching packages.");
            }
        }

        [HttpDelete("Delete_Package/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePackage(int id)
        {
            try
            {
                var currentUserId = GetUserId();
                var deleted = await _packageRepository.DeletePackageAsync(id);

                if (!deleted)
                    return NotFound("Package not found or already deleted.");

                await _logService.LogAsync("Info", $"Package deleted (ID: {id})", null, currentUserId);
                return Ok("Package deleted successfully.");
            }
            catch (Exception ex)
            {
                await _logService.LogAsync("Error", $"Error deleting package ID {id}", ex.Message, GetUserId());
                return StatusCode(500, "An error occurred while deleting the package.");
            }
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdClaim, out int userId)
                ? userId
                : throw new UnauthorizedAccessException("Invalid token.");
        }
    }
}
