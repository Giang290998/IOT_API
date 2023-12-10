using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using dotenv.net.Utilities;

namespace IOT_API.Helper;

public static class JWT
{
    public static string CreateAccessToken(string user_id, string role, string level)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(EnvReader.GetStringValue("JWT_AT_KEY") ?? "JWT_SECRET_KEY");
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new(ClaimTypes.NameIdentifier, user_id),
                new(ClaimTypes.Role, role),
                new(ClaimTypes.SerialNumber, level)
            }),
            Expires = DateTime.UtcNow.AddMinutes(30),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);
        return jwtToken;
    }

    public static string CreateRefreshToken(string user_id, string role, string level)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(EnvReader.GetStringValue("JWT_RT_KEY") ?? "JWT_SECRET_KEY");
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new(ClaimTypes.NameIdentifier, user_id),
                new(ClaimTypes.Role, role),
                new(ClaimTypes.SerialNumber, level)
            }),
            Expires = DateTime.UtcNow.AddDays(3),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);
        return jwtToken;
    }

    public static string CreatePersistentToken(string user_id, string role, string level)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(EnvReader.GetStringValue("JWT_PT_KEY") ?? "JWT_SECRET_KEY");
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new(ClaimTypes.NameIdentifier, user_id),
                new(ClaimTypes.Role, role),
                new(ClaimTypes.SerialNumber, level)
            }),
            Expires = DateTime.UtcNow.AddDays(30),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);
        return jwtToken;
    }

    private static readonly string AT_Key = EnvReader.GetStringValue("JWT_AT_KEY") ?? "JWT_SECRET_KEY";
    private static readonly string RT_Key = EnvReader.GetStringValue("JWT_RT_KEY") ?? "JWT_SECRET_KEY";
    private static readonly string PT_Key = EnvReader.GetStringValue("JWT_PT_KEY") ?? "JWT_SECRET_KEY";

    public static bool ValidateAccessToken(string accessToken)
    {
        return ValidateToken(accessToken, AT_Key);
    }

    public static bool ValidateRefreshToken(string refreshToken)
    {
        return ValidateToken(refreshToken, RT_Key);
    }

    public static bool ValidatePersistentToken(string persistentToken)
    {
        return ValidateToken(persistentToken, PT_Key);
    }

    private static bool ValidateToken(string token, string keyString)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(keyString);

        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero // remove delay of token when expire
            }, out _);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static string GetIdFromPayloadToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();

        if (handler.ReadToken(token) is JwtSecurityToken jsonToken)
        {
            // In ra thông tin trong payload
            foreach (var claim in jsonToken.Payload)
            {
                if (claim.Key == "nameid")
                {
                    return claim.Value.ToString() ?? "";
                }
            }
        }
        return "";
    }

    public static string GetRoleFromPayloadToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();

        if (handler.ReadToken(token) is JwtSecurityToken jsonToken)
        {
            // In ra thông tin trong payload
            foreach (var claim in jsonToken.Payload)
            {
                if (claim.Key == "role")
                {
                    return claim.Value.ToString() ?? "";
                }
            }
        }
        return "";
    }
}