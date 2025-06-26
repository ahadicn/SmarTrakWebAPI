using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace SmarTrakWebAPI.Middleware
{
    public class JwtRoleMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<JwtRoleMiddleware> _logger;
        private readonly string _tenantId;
        private readonly string _authority;
        private readonly string _audience;

        public JwtRoleMiddleware(RequestDelegate next, ILogger<JwtRoleMiddleware> logger, IConfiguration configuration)
        {
            _next = next;
            _logger = logger;
            _tenantId = configuration["AzureAd:TenantId"] ?? throw new InvalidOperationException("TenantId configuration is not set.");
            _audience = configuration["AzureAd:Audience"] ?? throw new InvalidOperationException("Audience configuration is not set.");
            _authority = $"https://login.microsoftonline.com/{_tenantId}/v2.0";
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Extract token from Authorization header
                if (!context.Request.Headers.TryGetValue("Authorization", out var authHeaders) || !authHeaders.Any())
                {
                    _logger.LogError("No Authorization header provided");
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    await context.Response.WriteAsJsonAsync(new { error = "No token provided" });
                    return;
                }

                var authHeader = authHeaders.First();
                if (!authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    _logger.LogError("Invalid Authorization header format");
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    await context.Response.WriteAsJsonAsync(new { error = "Invalid token format" });
                    return;
                }

                var token = authHeader.Substring("Bearer ".Length).Trim();

                // Fetch OpenID Connect configuration
                var configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
                    $"{_authority}/.well-known/openid-configuration",
                    new OpenIdConnectConfigurationRetriever());
                var openIdConfig = await configurationManager.GetConfigurationAsync();

                // Configure token validation parameters
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = _authority,
                    ValidateAudience = true,
                    ValidAudience = _audience,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKeys = openIdConfig.SigningKeys,
                    ClockSkew = TimeSpan.FromMinutes(5)
                };

                // Validate token
                var handler = new JwtSecurityTokenHandler();
                var principal = handler.ValidateToken(token, validationParameters, out var validatedToken);

                // Set principal in HttpContext for downstream use
                context.Items["UserPrincipal"] = principal;

                // Set user identity for [Authorize] attribute
                context.User = principal;

                // Proceed to the next middleware
                await _next(context);
            }
            catch (SecurityTokenException ex)
            {
                _logger.LogError($"Token validation failed: {ex.Message}");
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsJsonAsync(new { error = $"Invalid token: {ex.Message}" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error: {ex.Message}");
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsJsonAsync(new { error = "Internal server error" });
            }
        }
    }

    public static class JwtRoleMiddlewareExtensions
    {
        public static IApplicationBuilder UseJwtRoleMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JwtRoleMiddleware>();
        }
    }
}