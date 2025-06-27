using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SmarTrakWebAPI.Filters;
using Microsoft.Identity.Web;
using System.Security.Claims;
using System.Xml.Xsl;
using SmarTrakWebAPI.DBEntities;
//using SmarTrakWebAPI.Middleware;


var builder = WebApplication.CreateBuilder(args);


//// Add authentication services for [Authorize] attribute

// Add Azure AD authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(
        jwtOptions =>
        {

            jwtOptions.TokenValidationParameters.ValidAudiences = new[]
            {
                builder.Configuration["AzureAd:Audience"],
                $"api://{builder.Configuration["AzureAd:Audience"]}"
            };
            // Map roles and name claims
            jwtOptions.TokenValidationParameters.RoleClaimType = "roles";
            jwtOptions.TokenValidationParameters.NameClaimType = "name";

            // Disable automatic claim mapping to preserve original claim types
            jwtOptions.MapInboundClaims = false;

            // Transform namespaced claims to short names
            jwtOptions.Events = new JwtBearerEvents
            {
                OnTokenValidated = context =>
                {
                    if (context.Principal != null)
                    {
                        var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
                        if (claimsIdentity != null)
                        {
                            // Map role claim
                            var roleClaims = claimsIdentity.FindAll("http://schemas.microsoft.com/ws/2008/06/identity/claims/role").ToList();
                            foreach (var claim in roleClaims)
                            {
                                claimsIdentity.AddClaim(new Claim("roles", claim.Value));
                            }

                            // Map oid claim
                            var oidClaim = claimsIdentity.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
                            if (oidClaim != null)
                            {
                                claimsIdentity.AddClaim(new Claim("oid", oidClaim.Value));
                            }

                            // Map sub claim (if needed)
                            var subClaim = claimsIdentity.FindFirst("sub");
                            if (subClaim != null)
                            {
                                claimsIdentity.AddClaim(new Claim("sub", subClaim.Value));
                            }
                        }
                    }
                    return Task.CompletedTask;
                }
            };
        },
        identityOptions =>
        {
            // Bind AzureAd configuration
            builder.Configuration.GetSection("AzureAd").Bind(identityOptions);
        });




// Add authorization
builder.Services.AddAuthorization();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddLogging();


// Register DbContext with retry logic
builder.Services.AddDbContext<STContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure()
    ));



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SmarTrakAzure", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by your access token",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    c.OperationFilter<FileUploadOperationFilter>(); // Only keep if handling file uploads
});


builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 200_000_000; // 200 MB
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "SmartrakAPI V1");
        c.RoutePrefix = string.Empty; // Makes Swagger UI accessible at root
    });
}

app.UseHttpsRedirection();
app.UseRouting();

// Apply JWT middleware (no role requirements, handled by [Authorize])
//app.UseJwtRoleMiddleware();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();