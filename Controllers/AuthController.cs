using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using CarWash.Models;
using CarWash.DTO;
using CarWash.Interfaces;
using AutoMapper;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CarWash.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly ILogService _logService;

        public AuthController(
            UserManager<User> userManager,
            RoleManager<IdentityRole<int>> roleManager,
            SignInManager<User> signInManager,
            IMapper mapper,
            IConfiguration config,
            ILogService logService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _config = config;
            _logService = logService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            try
            {
                if (await _userManager.FindByEmailAsync(dto.Email) != null)
                {
                    await _logService.LogAsync("Warning", $"Registration failed - Email '{dto.Email}' is already taken.");
                    return BadRequest("Email is already in use.");
                }

                var user = _mapper.Map<User>(dto);
                user.IsActive = true;
                user.CreatedAt = DateTime.UtcNow;

                var result = await _userManager.CreateAsync(user, dto.Password);
                if (!result.Succeeded)
                    return BadRequest(result.Errors);

                if (!await _roleManager.RoleExistsAsync(dto.Role))
                    await _roleManager.CreateAsync(new IdentityRole<int>(dto.Role));

                await _userManager.AddToRoleAsync(user, dto.Role);

                await _logService.LogAsync("Info", $"User registered: {user.Email}", userId: user.Id);
                return Ok("Registration successful.");
            }
            catch (Exception ex)
            {
                await _logService.LogAsync("Error", "Exception during registration", ex.ToString());
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(dto.Email);
                if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
                    return Unauthorized("Invalid email or password.");

                var roles = await _userManager.GetRolesAsync(user);
                var token = GenerateJwtToken(user, roles);

                await _logService.LogAsync("Info", "Login successful", userId: user.Id);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                await _logService.LogAsync("Error", "Exception during login", ex.ToString());
                return StatusCode(500, "An error occurred while logging in.");
            }
        }

        [Authorize]
        [HttpGet("Get_Profile")]
        public async Task<IActionResult> GetCurrentUser()
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var user = await _userManager.FindByIdAsync(userId.ToString());
                var roles = await _userManager.GetRolesAsync(user);

                return Ok(new
                {
                    user.Id,
                    user.Email,
                    user.Name,
                    user.PhoneNumber,
                    user.Location,
                    Role = roles.FirstOrDefault()
                });
            }
            catch (Exception ex)
            {
                await _logService.LogAsync("Error", "Exception during profile fetch", ex.ToString(), GetUserIdFromClaims());
                return StatusCode(500, "An error occurred while fetching user profile.");
            }
        }

        [Authorize]
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null) return Unauthorized("User not found");

                if (dto.NewPassword != dto.VerifyPassword)
                    return BadRequest("New password and verification password do not match");

                var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);

                if (!result.Succeeded)
                    return BadRequest(result.Errors.Select(e => e.Description));

                return Ok("Password changed successfully");
            }
            catch (Exception ex)
            {
                await _logService.LogAsync("Error", "Exception during password reset", ex.ToString(), GetUserIdFromClaims());
                return StatusCode(500, "An error occurred while resetting password.");
            }
        }

        [Authorize]
        [HttpPut("UpdateProfile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto dto)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null) return Unauthorized("User not found");

                _mapper.Map(dto, user);
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                    return BadRequest(result.Errors.Select(e => e.Description));

                return Ok("Profile updated successfully");
            }
            catch (Exception ex)
            {
                await _logService.LogAsync("Error", "Exception during profile update", ex.ToString(), GetUserIdFromClaims());
                return StatusCode(500, "An error occurred while updating profile.");
            }
        }

        private string GenerateJwtToken(User user, IList<string> roles)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("name", user.Name ?? "")
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private int? GetUserIdFromClaims()
        {
            var idClaim = User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(idClaim, out var id) ? id : (int?)null;
        }
    }
}
