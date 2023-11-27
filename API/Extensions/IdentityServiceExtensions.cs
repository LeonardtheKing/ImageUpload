using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions
{
  public static class IdentityServiceExtensions
  {
    public static IServiceCollection AddIdentityServices(this IServiceCollection services,
    IConfiguration config)
    {
      // services.AddIdentityCore<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
      //           .AddEntityFrameworkStores<DataContext>();
      services.AddIdentityCore<AppUser>(opt =>
      {
        opt.Password.RequireNonAlphanumeric = false;

      })
       .AddDefaultTokenProviders()
       .AddRoles<AppRole>()
       .AddRoleManager<RoleManager<AppRole>>()
       .AddSignInManager<SignInManager<AppUser>>()
       .AddPasswordValidator<RoleValidator<AppRole>>()
       .AddUserManager<UserManager<AppUser>>()
       .AddEntityFrameworkStores<DataContext>();
      services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
          options.TokenValidationParameters = new TokenValidationParameters
          {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"])),
            ValidateIssuer = false,
            ValidateAudience = false
          };
        });
      return services;
    }

  }
}