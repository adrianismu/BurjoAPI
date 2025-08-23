using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Burjo.Core.DTOs;
using Burjo.Core.Entities;
using Burjo.Core.Interfaces;
using Burjo.Core.Models;
using Microsoft.IdentityModel.Tokens;

namespace Burjo.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly JwtSettings _jwtSettings;
    private readonly IUserRepository _userRepository;

    public AuthService(JwtSettings jwtSettings, IUserRepository userRepository)
    {
        _jwtSettings = jwtSettings;
        _userRepository = userRepository;
    }

    public async Task<LoginResponseDto?> RegisterAsync(RegisterUserDto dto)
    {
        // Validate email uniqueness
        if (await _userRepository.EmailExistsAsync(dto.Email))
        {
            return null; // Email already exists
        }

        // Hash password using BCrypt
        var hashedPassword = await HashPasswordAsync(dto.Password);

        // Create user entity
        var user = new User
        {
            Email = dto.Email.ToLower(),
            PasswordHash = hashedPassword
        };

        // Save user
        var createdUser = await _userRepository.CreateAsync(user);

        // Generate JWT token
        var token = await GenerateJwtTokenAsync(createdUser.Email, createdUser.Id);

        // Return response
        return new LoginResponseDto
        {
            Token = token,
            UserId = createdUser.Id,
            Email = createdUser.Email,
            ExpiresAt = DateTime.UtcNow.AddHours(_jwtSettings.ExpiryHours),
            Profile = null // No profile yet, user needs to create one
        };
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginUserDto dto)
    {
        // Find user by email with profile
        var user = await _userRepository.GetByEmailWithProfileAsync(dto.Email);
        if (user == null)
        {
            return null; // User not found
        }

        // Verify password
        var isValidPassword = await ValidatePasswordAsync(dto.Password, user.PasswordHash);
        if (!isValidPassword)
        {
            return null; // Invalid password
        }

        // Generate JWT token
        var token = await GenerateJwtTokenAsync(user.Email, user.Id);

        // Map profile to DTO if exists
        UserProfileDto? profileDto = null;
        if (user.Profile != null)
        {
            profileDto = new UserProfileDto
            {
                FullName = user.Profile.FullName,
                Age = user.Profile.Age,
                Gender = user.Profile.Gender,
                HeightCm = user.Profile.HeightCm,
                WeightKg = user.Profile.WeightKg,
                MedicalHistory = user.Profile.MedicalHistory,
                FitnessLevel = user.Profile.FitnessLevel,
                Bmi = user.Profile.Bmi
            };
        }

        // Return response
        return new LoginResponseDto
        {
            Token = token,
            UserId = user.Id,
            Email = user.Email,
            ExpiresAt = DateTime.UtcNow.AddHours(_jwtSettings.ExpiryHours),
            Profile = profileDto
        };
    }

    public async Task<string> GenerateJwtTokenAsync(string email, Guid userId)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtSettings.Secret);
        
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, email),
            new(ClaimTypes.NameIdentifier, userId.ToString()),
            new(ClaimTypes.Email, email),
            new("userId", userId.ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(_jwtSettings.ExpiryHours),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key), 
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return await Task.FromResult(tokenHandler.WriteToken(token));
    }

    public async Task<bool> ValidatePasswordAsync(string password, string hashedPassword)
    {
        return await Task.FromResult(BCrypt.Net.BCrypt.Verify(password, hashedPassword));
    }

    public async Task<string> HashPasswordAsync(string password)
    {
        return await Task.FromResult(BCrypt.Net.BCrypt.HashPassword(password));
    }
}
