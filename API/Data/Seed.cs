using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using API.Entities;
using Newtonsoft.Json;

namespace API.Data
{
  public class Seed
{
    public static async Task SeedUsers(DataContext context)
    {
        if(await context.AppUsers.AnyAsync()) return;
        if(await context.Photos.AnyAsync()) return;

        var userData = await System.IO.File.ReadAllTextAsync("Data/AppUser.json");
        // var users = JsonSerializer.Deserialize<List<AppUser>>(userData);
       var users = JsonConvert.DeserializeObject<List<AppUser>>(userData);
        foreach(var user in users){
            
            
            using var hmac = new HMACSHA512();
            user.UserName=user.UserName.ToLower();
            user.PasswordHash=hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
            user.PasswordSalt=hmac.Key;

            context.AppUsers.Add(user);
        }
        // var photoData = await System.IO.File.ReadAllTextAsync("Data/Photo.json");
        // var photos = JsonConvert.DeserializeObject<List<Photo>>(photoData);
        // context.Photos.AddRange(photos);
         await context.SaveChangesAsync();
    }
             
}


       
}