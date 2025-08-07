using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using radio_backend.Auth;

namespace radio_backend.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class Auth : ControllerBase
{
    private readonly PersonRepository _personRepository;

    public Auth()
    {
        _personRepository = new PersonRepository();
    }

    [HttpPost]
    public IResult Login([FromBody]Person loginData)
    {
        // находим пользователя 
        Person? person = _personRepository.GetPeople().FirstOrDefault(p =>
            p.Email == loginData.Email && p.Password == loginData.Password);
        
        // если пользователь не найден, отправляем статусный код 401
        if (person is null) return Results.Unauthorized();

        var claims = new List<Claim> { new(ClaimTypes.Name, person.Email) };
        // создаем JWT-токен
        var jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            claims: claims,
            expires: DateTime.MaxValue,
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256));
        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

        // формируем ответ
        var response = new
        {
            access_token = encodedJwt,
            username = person.Email
        };

        return Results.Json(response);
    }
}