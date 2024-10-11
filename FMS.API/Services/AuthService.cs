using AutoMapper;
using FMS.API.Entities;
using FMS.API.Middleware;
using FMS.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FMS.API.Services;

public interface IAuthService
{
    Task RegisterUser(UserSignupDto dto);
    Task<string> LoginUser(UserLoginDto dto);
    Task<UserDto> Account(Guid id);
}

public class AuthService : IAuthService
{
    private readonly FMSDbContext _context;
    private readonly AuthenticationSettings _authenticationSettings;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IMapper _mapper;

    public AuthService(FMSDbContext context, AuthenticationSettings authenticationSettings, IPasswordHasher<User> passwordHasher, IMapper mapper)
    {
        _context = context;
        _authenticationSettings = authenticationSettings;
        _passwordHasher = passwordHasher;
        _mapper = mapper;
    }

    public async Task RegisterUser(UserSignupDto dto)
    {
        if (await getUserByEmail(dto.Email) is not null) throw new BadRequestException("Email already in use.");

        if (dto.Password != dto.ConfirmPassword)
        {
            throw new BadRequestException("Passwords are different");
        }

        var newUser = new User()
        {
            Email = dto.Email,
            FullName = dto.FullName,
        };

        var hashedPasword = _passwordHasher.HashPassword(newUser, dto.Password);

        newUser.PasswordHash = hashedPasword;

        await _context.Users.AddAsync(newUser);
        await _context.SaveChangesAsync();
    }

    public async Task<string> LoginUser(UserLoginDto dto)
    {
        var user = await getUserByEmail(dto.Email);

        if (user is null)
        {
            throw new BadRequestException("Invalid email or password");
        }

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
        if (result == PasswordVerificationResult.Failed)
        {
            throw new BadRequestException("Invalid email or password");
        }

        return getToken(user);
    }

    private string getToken(User user)
    {
        var userId = user.Id.ToString();
        var userEmail = user.Email.ToString();
        var userFullName = user.FullName.ToString();

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_authenticationSettings.JwtKey);  // UTF8?

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Email, userEmail),
                new Claim("FullName", userFullName),
            }),
            Expires = DateTime.UtcNow.AddDays(_authenticationSettings.JwtExpiredays),
            Issuer = _authenticationSettings.JwtIssuer,
            Audience = _authenticationSettings.JwtIssuer,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public async Task<UserDto> Account(Guid id)
    {
        var user = await _context
            .Users
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user is null)
        {
            throw new BadRequestException("Invalid id.");
        }

        var result = _mapper.Map<UserDto>(user);

        return result;
    }

    private async Task<User> getUserByEmail(string email)
    {
        var user = await _context
            .Users
            .FirstOrDefaultAsync(u => u.Email == email);

        if (user is null) return null;
        return user;
    }
}