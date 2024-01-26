using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Web;

namespace AngularProject.API.Jwt
{
    public class TokenValidationHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string token = null;

            if (request.Headers.Authorization != null && request.Headers.Authorization.Scheme == "Bearer")
            {
                token = request.Headers.Authorization.Parameter;
            }

            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    var secretKey = "163ac74a99b2a87cf3636d47e5965d4f";
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                    var tokenHandler = new JwtSecurityTokenHandler();

                    tokenHandler.ValidateToken(token, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = key,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    }, out SecurityToken validatedToken);

                    var jwtToken = (JwtSecurityToken)validatedToken;
                    var identity = new ClaimsIdentity(jwtToken.Claims, "Jwt");
                    HttpContext.Current.User = new ClaimsPrincipal(identity);
                }
                catch
                {
                    // Token geçersiz ise burada işlem yapın
                }
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}