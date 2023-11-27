using Newtonsoft.Json;


namespace API.Data
{
    public class Seed
{
    public static async Task SeedUsers(UserManager<AppUser> userManager)
    {
        if(await userManager.Users.AnyAsync()) return;
              var userData = await File.ReadAllTextAsync("Data/AppUser.json");
           var users = JsonConvert.DeserializeObject<List<AppUser>>(userData);
            if (users == null) return;
       
        foreach(var user in users){
            
            user.UserName=user.UserName.ToLower();
           
           await userManager.CreateAsync(user,"Pa$$w0rd");
        }
        // var photoData = await System.IO.File.ReadAllTextAsync("Data/Photo.json");
        // var photos = JsonConvert.DeserializeObject<List<Photo>>(photoData);
        // context.Photos.AddRange(photos);
         
    }
             
}


       
}