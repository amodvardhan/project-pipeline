using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProjectPipeline.Shared.Helpers;

/// <summary>
/// Helper class for JWT token operations
/// </summary>
public static class JwtHelper
{
    /// <summary>
    /// Generates a JWT token
    /// </summary>
    /// <param name="claims">Claims to include in the token</param>
    /// <param name="configuration">Configuration containing JWT settings</param>
    /// <returns>JWT token string</returns>
    public static string GenerateToken(IEnumerable<Claim> claims, IConfiguration configuration)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(configuration["Jwt:ExpiryInMinutes"])),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    /// <summary>
    /// Validates a JWT token
    /// </summary>
    /// <param name="token">Token to validate</param>
    /// <param name="configuration">Configuration containing JWT settings</param>
    /// <returns>ClaimsPrincipal if valid, null otherwise</returns>
    public static ClaimsPrincipal? ValidateToken(string token, IConfiguration configuration)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            return principal;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Extracts claims from a JWT token without validation
    /// </summary>
    /// <param name="token">JWT token</param>
    /// <returns>Claims from the token</returns>
    public static IEnumerable<Claim> GetClaimsFromToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            return jwtToken.Claims;
        }
        catch
        {
            return Enumerable.Empty<Claim>();
        }
    }

    /// <summary>
    /// Checks if a token is expired
    /// </summary>
    /// <param name="token">JWT token</param>
    /// <returns>True if expired</returns>
    public static bool IsTokenExpired(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            return jwtToken.ValidTo < DateTime.UtcNow;
        }
        catch
        {
            return true;
        }
    }
}
