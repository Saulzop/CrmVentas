using CrmVentas.Application.Dtos;
using CrmVentas.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CrmVentas.Api.Controllers;

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
    public async Task<ActionResult<LoginResponseDto>> Login(LoginRequestDto dto)
    {
        var resultado = await _authService.LoginAsync(dto);
        return Ok(resultado);
    }
}
