using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProjectPipeline.Core.DTOs.Request;
using ProjectPipeline.Core.DTOs.Response;
using ProjectPipeline.Core.Entities;
using ProjectPipeline.Core.Interfaces.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProjectPipeline.Infrastructure.Services;

/// <summary>
/// Authentication service implementation
/// </summary>
public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IConfiguration _configuration;

    public AuthService(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null || !user.IsActive)
        {
            return new AuthResponse
            {
                IsSuccess = false,
                Message = "Invalid email or password"
            };
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!result.Succeeded)
        {
            return new AuthResponse
            {
                IsSuccess = false,
                Message = "Invalid email or password"
            };
        }

        var token = await GenerateJwtTokenAsync(user);
        user.LastLoginDate = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);

        return new AuthResponse
        {
            IsSuccess = true,
            Message = "Login successful",
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpiryInMinutes"])),
            User = new UserResponse
            {
                Id = user.Id,
                Email = user.Email!,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Department = user.Department,
                Designation = user.Designation,
                BusinessUnitId = user.BusinessUnitId
            }
        };
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
        {
            return new AuthResponse
            {
                IsSuccess = false,
                Message = "User with this email already exists"
            };
        }

        var user = new User
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Department = request.Department,
            Designation = request.Designation,
            BusinessUnitId = request.BusinessUnitId,
            IsActive = true
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            return new AuthResponse
            {
                IsSuccess = false,
                Message = string.Join(", ", result.Errors.Select(e => e.Description))
            };
        }

        // Add user to role
        await _userManager.AddToRoleAsync(user, request.Role);

        return new AuthResponse
        {
            IsSuccess = true,
            Message = "User registered successfully"
        };
    }

    public async Task<bool> LogoutAsync(string userId)
    {
        await _signInManager.SignOutAsync();
        return true;
    }

    public async Task<AuthResponse> RefreshTokenAsync(string refreshToken)
    {
        // Implement refresh token logic here
        await Task.CompletedTask;
        return new AuthResponse
        {
            IsSuccess = false,
            Message = "Refresh token not implemented yet"
        };
    }

    public async Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return false;

        var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        return result.Succeeded;
    }

    public async Task<bool> ResetPasswordAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            return false;

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        // Send email with reset token (implement email service)
        return true;
    }

    private async Task<string> GenerateJwtTokenAsync(User user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
            new Claim("BusinessUnitId", user.BusinessUnitId?.ToString() ?? "")
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpiryInMinutes"])),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
