using LotTrace_MES.src.Application.DTO.Request.Auth;
using LotTrace_MES.src.Application.DTO.Response.Auth;
using LotTrace_MES.src.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LotTrace_MES.src.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<ResponseAuthDTO>> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            var result = await _authService.LoginAsync(loginRequestDTO);

            if (result == null)
            {
                return Unauthorized(new ResponseAuthDTO { Success = false, Message = "사번 또는 비밀번호가 일치하지 않습니다." });
            }

            return Ok(result);
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<ResponseAuthDTO>> Refresh([FromBody] RequestRefreshTokenDTO requestRefreshTokenDTO)
        {
            var result = await _authService.RefreshAsync(requestRefreshTokenDTO);

            if (result == null)
            {
                return BadRequest(new { Message = "유효하지 않은 토큰이거나 정보가 일치하지 않습니다." });
            }

            if (!result.Success)
            {
                return Unauthorized(result);
            }

            return Ok(result);
        }
    }
}
