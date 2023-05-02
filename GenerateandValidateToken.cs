using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
public static class JwtTokenHelper
{
    private static readonly string secretKey = "doge_is_the_best_thing_ever_such_wow_many_amaze_so_security_much_crypto";
    private const int tokenExpiryInMinutes = 10;

    public static string CreateToken(string clientId)
    {


        var claims = new[]
        {
            new Claim("clientId", clientId)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


        var token = new JwtSecurityToken(
            issuer: "DogeIssuer",
            audience: "DogeAudience",
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(tokenExpiryInMinutes),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public static bool ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(secretKey);
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = "DogeIssuer",
                ValidateAudience = true,
                ValidAudience = "DogeAudience",
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);
        }
        catch
        {
            return false;
        }
        return true;
    }
}