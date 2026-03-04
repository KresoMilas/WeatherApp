using WeatherApp.API.Data;
using WeatherApp.API.DTOs.AuthDtos;
using WeatherApp.API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;

namespace WeatherApp.API.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _appDbContext;

    private readonly IConfiguration _configuration;


    public AuthService(AppDbContext appDbContext, IConfiguration configuration)
    {
        _appDbContext = appDbContext;
        _configuration = configuration;
    }

    public async Task<AuthResponseDto> RegisterAsync(AuthRequestDto registerRequest)
    {
        var existingUser = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Email == registerRequest.Email);
        if (existingUser == null)
        {
            var newUser = new User()
            {
                Email = registerRequest.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password)
            };
            _appDbContext.Users.Add(newUser);
            await _appDbContext.SaveChangesAsync();
            return new AuthResponseDto { Email = newUser.Email, Token = GenerateToken(newUser) };
        }
        else
        {
            throw new InvalidOperationException("User with this email already exists.");
        }
    }

    public async Task<AuthResponseDto> LoginAsync(AuthRequestDto loginRequest)
    {
        var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Email == loginRequest.Email);
        if (user != null && BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.PasswordHash))
        {
            var token = GenerateToken(user);
            return new AuthResponseDto { Email = user.Email, Token = token };
        }
        else
        {
            throw new InvalidOperationException("Invalid email or password.");
        }
    }

    private string GenerateToken(User user)
    {
        var key = _configuration["Jwt:Key"]!;
        var issuer = _configuration["Jwt:Issuer"]!;
        var audience = _configuration["Jwt:Audience"]!;
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}   