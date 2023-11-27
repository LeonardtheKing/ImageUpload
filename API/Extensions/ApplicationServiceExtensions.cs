using System.Security.Claims;
using System.Text;
using API.Helpers;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace API.Extensions
{

    public static class ApplicationServiceExtensions
    {

        public static IServiceCollection RegisterServices(this IServiceCollection Services, IConfiguration config)
        {


            //             return Services;
            Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();
            Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
                });
            });
            Services.Configure<IdentityOptions>(options =>
                options.ClaimsIdentity.UserIdClaimType = ClaimTypes.NameIdentifier);
            Services.AddScoped<ITokenService, TokenService>();
            Services.AddScoped<IUserRepository, UserRepository>();
            Services.AddScoped<IPhotoService, PhotoService>();
            Services.AddScoped<ILikesRepository, LikesRepository>();
             Services.AddIdentity<AppUser, AppRole>()
        .AddEntityFrameworkStores<DataContext>()
        .AddDefaultTokenProviders();
            
           

            Services.AddScoped<LogUserActivity>();
            Services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            Services.AddAutoMapper(typeof(AutoMapperProfiles));
            Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                 .AddJwtBearer(options =>
                 {
                     options.TokenValidationParameters = new TokenValidationParameters
                     {
                         ValidateIssuerSigningKey = true,
                         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("TokenKey").Value)),
                         ValidateIssuer = false,
                         ValidateAudience = false
                     };
                 });
            Services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });
            return Services;
        }

    }
}