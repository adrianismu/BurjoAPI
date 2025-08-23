using Burjo.Core.DTOs;

namespace Burjo.Core.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto?> RegisterAsync(RegisterUserDto dto);
    Task<LoginResponseDto?> LoginAsync(LoginUserDto dto);
    Task<string> GenerateJwtTokenAsync(string email, Guid userId);
    Task<bool> ValidatePasswordAsync(string password, string hashedPassword);
    Task<string> HashPasswordAsync(string password);
}
