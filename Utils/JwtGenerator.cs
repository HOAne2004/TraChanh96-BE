// Utils/JwtGenerator.cs
using drinking_be.Interfaces.UserInterfaces;
using drinking_be.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace drinking_be.Utils
{
    public class JwtGenerator : IJwtGenerator
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;

        // Inject IConfiguration để lấy Secret Key từ appsettings
        public JwtGenerator(IConfiguration config)
        {
            _config = config;
            // Lấy Secret Key đã cấu hình trong appsettings.json
            var secret = _config["JwtSettings:Key"];

            if (string.IsNullOrEmpty(secret))
            {
                throw new ArgumentNullException("JwtSettings:Key không được cấu hình.");
            }
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        }

        public string CreateToken(User user)
        {
            // 1. Định nghĩa Claims (Thông tin User đưa vào Token)
            var claims = new List<Claim>
            {
                // Thêm ID công khai (PublicId) để dễ dàng xác định người dùng
                new Claim(JwtRegisteredClaimNames.Sub, user.PublicId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                
                // Thêm Role (Dựa trên role_id trong CSDL)
                new Claim(ClaimTypes.Role, user.RoleId switch
                {
                    2 => "Admin",
                    3 => "Manager",
                    _ => "User"
                }),
            };

            // 2. Tạo Credentials
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var issuer = _config["JwtSettings:Issuer"];
            var audience = _config["JwtSettings:Audience"];

            // 3. Định nghĩa Token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7), // Token có hiệu lực trong 7 ngày
                SigningCredentials = creds,
                Issuer = issuer,       // Tên dịch vụ phát hành (có thể lấy từ config)
                Audience = audience
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}