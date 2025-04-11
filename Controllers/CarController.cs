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
    public class CarController : ControllerBase
    {
        private readonly ICarRepository _carRepository;
        private readonly UserManager<User> _userManager;
        private readonly ILogService _logService;
        private readonly IMapper _mapper;

        public CarController(
            ICarRepository carRepository,
            UserManager<User> userManager,
            ILogService logService,
            IMapper mapper)
        {
            _carRepository = carRepository;
            _userManager = userManager;
            _logService = logService;
            _mapper = mapper;
        }

        [HttpPost("Add_Car")]
        public async Task<IActionResult> AddCar([FromBody] CarDto dto)
        {
            try
            {
                var currentUserId = GetUserId();
                var currentUser = await _userManager.FindByIdAsync(currentUserId.ToString());

                if (currentUser == null)
                    return Unauthorized("User not found.");

                var isAdmin = await _userManager.IsInRoleAsync(currentUser, "Admin");

                if (!isAdmin && currentUser.Id != dto.CustomerId)
                    return Forbid("You can only add cars for your own account.");

                var customer = await _userManager.FindByIdAsync(dto.CustomerId.ToString());
                if (customer == null || !(await _userManager.IsInRoleAsync(customer, "Customer")))
                    return BadRequest("Invalid CustomerId.");

                var car = _mapper.Map<Car>(dto);
                car.UserId = dto.CustomerId;

                var addedCar = await _carRepository.AddCarAsync(car);

                await _logService.LogAsync("Info", $"Car added for user {dto.CustomerId}", null, currentUserId);

                return Ok(_mapper.Map<CarDto>(addedCar));
            }
            catch (Exception ex)
            {
                await _logService.LogAsync("Error", "Error adding car", ex.Message, GetUserId());
                return StatusCode(500, "An error occurred while adding the car.");
            }
        }

        [HttpGet("Get_Car/{id}")]
        public async Task<IActionResult> GetCarById(int id)
        {
            try
            {
                var currentUserId = GetUserId();
                var car = await _carRepository.GetCarByIdAsync(id);
                if (car == null)
                    return NotFound("Car not found.");

                var currentUser = await _userManager.FindByIdAsync(currentUserId.ToString());
                var isAdmin = await _userManager.IsInRoleAsync(currentUser, "Admin");

                if (!isAdmin && car.UserId != currentUserId)
                    return Forbid("You can only view your own car.");

                return Ok(_mapper.Map<CarDto>(car));
            }
            catch (Exception ex)
            {
                await _logService.LogAsync("Error", $"Error getting car with ID {id}", ex.Message, GetUserId());
                return StatusCode(500, "An error occurred while fetching the car.");
            }
        }

        [HttpPut("Update_Car/{id}")]
        public async Task<IActionResult> UpdateCarById(int id, [FromBody] CarDto dto)
        {
            try
            {
                var currentUserId = GetUserId();
                var car = await _carRepository.GetCarByIdAsync(id);
                if (car == null)
                    return NotFound("Car not found.");

                var currentUser = await _userManager.FindByIdAsync(currentUserId.ToString());
                var isAdmin = await _userManager.IsInRoleAsync(currentUser, "Admin");

                if (!isAdmin && car.UserId != currentUserId)
                    return Forbid("You can only update your own car.");

                var customer = await _userManager.FindByIdAsync(dto.CustomerId.ToString());
                if (customer == null || !(await _userManager.IsInRoleAsync(customer, "Customer")))
                    return BadRequest("Invalid CustomerId.");

                _mapper.Map(dto, car);
                car.UserId = dto.CustomerId;

                var updated = await _carRepository.UpdateCarAsync(car);
                if (!updated)
                    return StatusCode(500, "Failed to update car.");

                await _logService.LogAsync("Info", $"Car updated (ID: {id})", null, currentUserId);
                
                return Ok("Car Added");
            }
            catch (Exception ex)
            {
                await _logService.LogAsync("Error", $"Error updating car ID {id}", ex.Message, GetUserId());
                return StatusCode(500, "An error occurred while updating the car.");
            }
        }

        [HttpDelete("Delete_Car/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCarById(int id)
        {
            try
            {
                var currentUserId = GetUserId();
                var deleted = await _carRepository.DeleteCarAsync(id);
                if (!deleted)
                    return NotFound("Car not found or already deleted.");

                await _logService.LogAsync("Info", $"Car deleted (ID: {id})", null, currentUserId);
                return Ok("Car deleted successfully.");
            }
            catch (Exception ex)
            {
                await _logService.LogAsync("Error", $"Error deleting car ID {id}", ex.Message, GetUserId());
                return StatusCode(500, "An error occurred while deleting the car.");
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
