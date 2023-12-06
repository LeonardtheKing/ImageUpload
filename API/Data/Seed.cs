using Newtonsoft.Json;


namespace API.Data
{
    public class Seed
{
    public static async Task SeedUsers(UserManager<AppUser> userManager,
    RoleManager<AppRole> roleManager)
    {
        if(await userManager.Users.AnyAsync()) return;
              var userData = await File.ReadAllTextAsync("Data/AppUser.json");
           var users = JsonConvert.DeserializeObject<List<AppUser>>(userData);
            if (users == null) return;
            var roles = new List<AppRole>
            {
                new AppRole{Name="Member", ConcurrencyStamp = Guid.NewGuid().ToString()},
                new AppRole{Name="Admin", ConcurrencyStamp = Guid.NewGuid().ToString()},
                new AppRole{Name="Moderator", ConcurrencyStamp = Guid.NewGuid().ToString()},

            };
          foreach(var role in roles){
            await roleManager.CreateAsync(role);
          }
        foreach(var user in users){
            
            user.UserName=user.UserName.ToLower();   
           await userManager.CreateAsync(user,"Pa$$w0rd");
           await userManager.AddToRoleAsync(user,"Member");
        }

        var admin = new AppUser
        {
            UserName="admin"
        };
        await userManager.CreateAsync(admin,"Pa$$w0rd");
        await userManager.AddToRolesAsync(admin,new[]{"Admin","Moderator"});
        // var photoData = await System.IO.File.ReadAllTextAsync("Data/Photo.json");
        // var photos = JsonConvert.DeserializeObject<List<Photo>>(photoData);
        // context.Photos.AddRange(photos);
         
    }
             
}


       
}
