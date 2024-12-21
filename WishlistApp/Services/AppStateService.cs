using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

public class JwtDecoder
{
    public static ClaimsPrincipal DecodeJwtToken(string token, string secretKey, string validIssuer, string validAudience)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            // Валидация токена: настройка параметров
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,

                ValidIssuer = validIssuer,
                ValidAudience = validAudience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
            };

            // Валидация и расшифровка токена
            var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

            // Проверка типа токена (например, должен быть JwtSecurityToken)
            if (validatedToken is not JwtSecurityToken jwtToken ||
                !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Недействительный токен.");
            }

            return principal;
        }
        catch (Exception ex)
        {
            // Обработка ошибок
            Console.WriteLine($"Ошибка расшифровки токена: {ex.Message}");
            return null;
        }
    }
}