using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace EcommerceApi.Helpers
{
    public class TokenValidator
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<TokenValidator> _logger;

        public TokenValidator(IConfiguration configuration, ILogger<TokenValidator> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public ClaimsPrincipal? ValidateToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT key not configured"));

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    RequireExpirationTime = true
                };

                _logger.LogInformation("Attempting to validate token");

                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

                if (validatedToken is JwtSecurityToken jwtToken)
                {
                    // Log token details
                    _logger.LogInformation("Token validated successfully:");
                    _logger.LogInformation($"Issuer: {jwtToken.Issuer}");
                    _logger.LogInformation($"Valid from: {jwtToken.ValidFrom}");
                    _logger.LogInformation($"Valid to: {jwtToken.ValidTo}");
                    _logger.LogInformation("Claims:");
                    foreach (var claim in principal.Claims)
                    {
                        _logger.LogInformation($"- {claim.Type}: {claim.Value}");
                    }

                    // Verify required claims are present
                    var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    var email = principal.FindFirst(ClaimTypes.Email)?.Value;

                    if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(email))
                    {
                        _logger.LogWarning("Token missing required claims");
                        return null;
                    }

                    return principal;
                }

                _logger.LogWarning("Token is not a valid JWT token");
                return null;
            }
            catch (SecurityTokenExpiredException ex)
            {
                _logger.LogWarning(ex, "Token has expired");
                throw;
            }
            catch (SecurityTokenInvalidSignatureException ex)
            {
                _logger.LogWarning(ex, "Token has invalid signature");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Token validation failed");
                throw;
            }
        }
    }
}
