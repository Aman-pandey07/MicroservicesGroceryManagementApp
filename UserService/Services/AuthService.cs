using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;
using UserService.Data;
using UserService.Dto;
using UserService.Models;

namespace UserService.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserDbContext _context;
        private readonly IConfiguration _config;

        public AuthService(UserDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        public async Task<UserDto> Login(UserLoginDto dto)
        {

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == dto.Email.ToLower());
            if (user == null) return null;

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password));
            if (!computedHash.SequenceEqual(user.PasswordHash)) return null;

            return new UserDto
            {
                Name = user.Name,
                Email = user.Email,
                Token = CreateToken(user)
            };

        }

        public async Task<UserDto> Register(UserregistrationDto dto)
        {

            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                Name = dto.Name,
                Email = dto.Email.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password)),
                PasswordSalt = hmac.Key,
                Role = "User"
            };

            _context.Add(user);
            await _context.SaveChangesAsync();

            return new UserDto
            {
                Name = dto.Name,
                Email = dto.Email,
                Token = CreateToken(user)
            };

           
        }

        public async Task<bool> UserExists(string email)
        {
            return await _context.Users.AnyAsync(x => x.Email == email.ToLower());
        }



        public string CreateToken(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role , user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key , SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
