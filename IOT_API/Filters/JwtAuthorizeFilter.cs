using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;

namespace IOT_API.Filters;

public class JwtAuthorizeFilter : IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token == null)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("JWT_AT_KEY") ?? "");
        try
        {
            var claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;

            if (jwtToken.ValidTo < DateTime.UtcNow)
            {
                // expired token
                context.Result = new UnauthorizedResult();
                return;
            }

            var user_id = int.Parse(jwtToken.Claims.FirstOrDefault(claim => claim.Type == "nameid")?.Value ?? "0");
            var role = int.Parse(jwtToken.Claims.FirstOrDefault(claim => claim.Type == "role")?.Value ?? "");

            if (user_id == 0)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            context.HttpContext.Items["user_id"] = user_id;
            context.HttpContext.Items["role"] = role;
        }
        catch (Exception)
        {
            context.Result = new UnauthorizedResult();
            return;
        }
    }
}