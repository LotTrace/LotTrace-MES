using LotTrace_MES.src.Application.DTO.Request.Auth;
using LotTrace_MES.src.Application.DTO.Response.Auth;

namespace LotTrace_MES.src.Application.Interfaces
{
    public interface IAuthService
    {
        Task<ResponseAuthDTO?> LoginAsync(LoginRequestDTO loginRequestDTO);
        Task<ResponseAuthDTO?> RefreshAsync(RequestRefreshTokenDTO requestRefreshTokenDTO);
    }
}
