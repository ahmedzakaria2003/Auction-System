using AuctionSystem.Application.Contracts;
using AuctionSystem.Application.Services.Contracts;
using AuctionSystem.Domain.Entities;
using AuctionSystem.Domain.Exceptions;  
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuctionSystem.Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly SigningCredentials _signingCredentials;
        private readonly IUnitOfWork _unitOfWork;

        public TokenService(IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;

            var key = Encoding.UTF8.GetBytes(_configuration["JWTOptions:SecretKey"]);

            if (key.Length == 0)
                throw new ArgumentNullException("JWT SecretKey is missing in the configuration.");

            _signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256
            );
        }

        public async Task<string> GenerateTokenAsync(ApplicationUser user, IEnumerable<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = new JwtSecurityToken(
                issuer: _configuration["JWTOptions:Issuer"],
                audience: _configuration["JWTOptions:Audience"],
                expires: DateTime.UtcNow.AddHours(6),
                claims: claims,
                signingCredentials: _signingCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            var refreshToken = new RefreshToken
            {
                UserId = user.Id,
                Token = tokenString,
                ExpirationDate = DateTime.UtcNow.AddDays(7)
            };

            await _unitOfWork.refreshToken.AddAsync(refreshToken);
            await _unitOfWork.SaveChangesAsync();

            return tokenString;
        }

        public async Task<ApplicationUser> ValidateRefreshTokenAsync(string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
                throw new UnauthorizedException("Refresh token is invalid or expired.");

            var tokenDb = await _unitOfWork.refreshToken.GetByTokenAsync(refreshToken);

            if (tokenDb == null || tokenDb.ExpirationDate < DateTime.UtcNow)
                throw new UnauthorizedException("Invalid or expired refresh token.");

            var user = await _unitOfWork.Users.GetByIdAsync(tokenDb.UserId);
            if (user == null)
                throw new NotFoundException("User not found.");

            return user;
        }
    }
}
