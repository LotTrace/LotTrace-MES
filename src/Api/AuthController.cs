using LotTrace_MES.src.Application.DTO.Request.Auth;
using LotTrace_MES.src.Application.DTO.Response.Auth;
using LotTrace_MES.src.Application.Interfaces;
using LotTrace_MES.src.Domain.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace LotTrace_MES.src.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWorkerService _workerService;
        private static readonly Dictionary<string, Auth> _authDbMock = new Dictionary<string, Auth>();

        public AuthController(IConfiguration configuration, IWorkerService workerService)
        {
            _configuration = configuration;
            _workerService = workerService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<ResponseAuthDTO>> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            var worker = await _workerService.GetWorkerByEmployeeNumberAsync(loginRequestDTO.EmployeeNumber);

            if (worker == null)
            {
                return Unauthorized(new ResponseAuthDTO { Success = false, Message = "등록되지 않은 작업자 사번입니다." });
            }

            var accessToken = GenerateJwtToken(worker.Role, worker.EmployeeNumber);
            var refreshToekn = GenerateRefreshToken();

            var expiryMinutes = Convert.ToInt32(_configuration["Jwt:ExpireMinutes"]);
            var refreshDays = Convert.ToInt32(_configuration["Jwt:RefreshExpiryDays"]);
            DateTime accessExpiryTime = DateTime.UtcNow.AddMinutes(expiryMinutes);
            DateTime refreshExpiryTime = DateTime.UtcNow.AddDays(refreshDays);

            var authEntity = new Auth
            {
                Success = true,
                Token = accessToken,
                RefreshToken = refreshToekn,
                WorkerName = worker.WorkerName,
                Message = $"{worker.WorkerName}님 ({worker.Role})",
                AccessTimeExpiration = accessExpiryTime,
                RefreshTokenExpiration = refreshExpiryTime,
            };

            _authDbMock[worker.EmployeeNumber.ToString()] = authEntity;

            return Ok(new ResponseAuthDTO
            {
                Success = authEntity.Success,
                Token = authEntity.Token,
                RefreshToken = authEntity.RefreshToken,
                EmployeeNumber = worker.EmployeeNumber,
                Message = authEntity.Message,
                AccessTimeExpiration = authEntity.AccessTimeExpiration,
                RefreshTokenExpiration = refreshExpiryTime,
            });
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<ResponseAuthDTO>> Refresh([FromBody] RequestRefreshTokenDTO requestRefreshTokenDTO)
        {
            var principal = GetPrincipalFromExpiredToken(requestRefreshTokenDTO.ExpiredToken);
            if (principal == null)
            {
                return BadRequest(new { Message = "유효하지 않은 토큰입니다." });
            }

            var employeeNumber = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(employeeNumber))
            {
                return BadRequest(new { Message = "토큰 내부에 식별 가능한 사번 정보가 없습니다." });
            }

            var worker = await _workerService.GetWorkerByEmployeeNumberAsync(employeeNumber);

            if (worker == null || worker.EmployeeNumber != employeeNumber)
            {
                return Unauthorized(new ResponseAuthDTO { Success = false, Message = "작업자 정보가 사번과 일치하지 않거나 변조되었습니다." });
            }

            if(!_authDbMock.TryGetValue(employeeNumber, out var savedAuth) || savedAuth.RefreshToken != requestRefreshTokenDTO.RefreshToken)
            {
                return Unauthorized(new ResponseAuthDTO { Success = false, Message = "리프레시 토큰이 장부 정보와 일치하지 않습니다." });
            }

            if (savedAuth.RefreshTokenExpiration < DateTime.UtcNow)
            {
                return Unauthorized(new ResponseAuthDTO { Success = false, Message = "연장 가능 기한(7일)이 만료되었습니다. 다시 로그인 하세요." });
            }

            var newAccessToken = GenerateJwtToken(worker.Role, employeeNumber);
            var newExpiryMinutes = Convert.ToInt32(_configuration["Jwt:ExpireMinutes"]);
            DateTime accessExpiryTime = DateTime.UtcNow.AddMinutes(newExpiryMinutes);

            savedAuth.Token = newAccessToken;
            savedAuth.AccessTimeExpiration = accessExpiryTime;

            return Ok(new ResponseAuthDTO {
                Success = true,
                Token = savedAuth.Token,
                RefreshToken = savedAuth.RefreshToken,
                EmployeeNumber = worker.EmployeeNumber,
                Message = $"{savedAuth.WorkerName}님 세션 연장 성공",
                AccessTimeExpiration = savedAuth.AccessTimeExpiration
            });

        }
        private string GenerateJwtToken(string role, string employeeNumber)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var keyString = _configuration["Jwt:Key"];
            var expiryMinutes = Convert.ToInt32(_configuration["Jwt:ExpiryMinutes"]);
            var key = Encoding.ASCII.GetBytes(keyString);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Role, role),
                    new Claim(ClaimTypes.NameIdentifier, employeeNumber)
                }),
                Expires = DateTime.UtcNow.AddMinutes(expiryMinutes),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Jwt:Key"])),
                ValidateIssuer = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["Jwt:Audience"],
                ValidateLifetime = false 
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }

            return principal;
        }
    }
}
