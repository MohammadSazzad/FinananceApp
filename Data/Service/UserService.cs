using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FinanceApp.Models;
using FinanceApp.Models.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FinanceApp.Data.Service
{
    public class UserService : IUserService
    {
        private readonly FinanceAppContext _context;
        private readonly IConfiguration _config;
        private readonly PasswordHasher<User> _passwordHasher = new();

        public UserService(FinanceAppContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _context.Users
                .OrderByDescending(u => u.Id)
                .Select(u => new User {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    Role = u.Role,
                    CreatedAt = u.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<User?> GetUserById(int id)
        {
            return await _context.Users
                .Where(u => u.Id == id)
                .Select(u => new User {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    Role = u.Role,
                    CreatedAt = u.CreatedAt
                })
                .FirstOrDefaultAsync();
        }

        public async Task<LoginResponse> LoginUser(UserLoginDTO loginDTO)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == loginDTO.Username);

            if (user == null)
            {
                _passwordHasher.VerifyHashedPassword(new User(), "dummy-hash", "dummy-password");
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            var result = _passwordHasher.VerifyHashedPassword(
                user, 
                user.PasswordHash, 
                loginDTO.Password
            );

            if (result == PasswordVerificationResult.Failed)
                throw new UnauthorizedAccessException("Invalid credentials");

            var token = GenerateJwtToken(user);
            
            return new LoginResponse
            {
                Token = token,
                UserId = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role
            };
        }

        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["AppSettings:Token"]!)
            );
            
            var credentials = new SigningCredentials(
                securityKey, 
                SecurityAlgorithms.HmacSha512Signature
            );

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(30),
                SigningCredentials = credentials,
                Issuer = _config["AppSettings:Issuer"],
                Audience = _config["AppSettings:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task RegisterUser(User user, string password)
        {
            if (await _context.Users.AnyAsync(u => u.Username == user.Username))
                throw new ArgumentException("Username already exists");
            
            if (await _context.Users.AnyAsync(u => u.Email == user.Email))
                throw new ArgumentException("Email already exists");

            user.PasswordHash = _passwordHasher.HashPassword(user, password);
            user.CreatedAt = DateTime.UtcNow;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }


        public async Task UpdateUser(User user)
        {
            var existingUser = await _context.Users.FindAsync(user.Id);
            if (existingUser == null)
                throw new KeyNotFoundException("User not found");

            existingUser.Username = user.Username;
            existingUser.Email = user.Email;
            existingUser.Role = user.Role;

            await _context.SaveChangesAsync();
        }

        public async Task ChangePassword(int userId, string currentPassword, string newPassword)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("User not found");

            var result = _passwordHasher.VerifyHashedPassword(
                user, 
                user.PasswordHash, 
                currentPassword
            );

            if (result == PasswordVerificationResult.Failed)
                throw new UnauthorizedAccessException("Current password is invalid");

            user.PasswordHash = _passwordHasher.HashPassword(user, newPassword);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}