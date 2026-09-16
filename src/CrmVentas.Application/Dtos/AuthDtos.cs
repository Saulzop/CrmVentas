namespace CrmVentas.Application.Dtos;

public record LoginRequestDto(string NombreUsuario, string Password);

public record LoginResponseDto(string Token, string NombreUsuario, string Rol, DateTime ExpiraEn);
