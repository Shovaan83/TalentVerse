using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TalentVerse.WebAPI.Data.Entities;

namespace TalentVerse.WebAPI.Data
{
    public class Seed
    {
        public static async Task SeedUsers(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, ILogger logger)
        {
            try
            {
                if (await userManager.Users.AnyAsync()) return;

                var roles = new List<IdentityRole>
            {
                new IdentityRole{Name = "Member", NormalizedName = "MEMBER"},
                new IdentityRole{Name = "Admin", NormalizedName = "ADMIN"},
                new IdentityRole{Name = "Business", NormalizedName = "BUSINESS" }
            };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role.Name))
                    {
                        await roleManager.CreateAsync(role);
                    }
                }

                var members = new List<AppUser>
                {
                    new AppUser
                    {
                        UserName = "shovan",
                        Email = "shovan@talentverse.com",
                        Bio = "I am Shovan, the creator of TalentVerse.",
                        ProfilePictureURL = "https://instagram.fbir1-1.fna.fbcdn.net/v/t51.2885-19/586687629_18355552537207369_677735766952371488_n.jpg?stp=dst-jpg_s150x150_tt6&efg=eyJ2ZW5jb2RlX3RhZyI6InByb2ZpbGVfcGljLmRqYW5nby4xMDgwLmMyIn0&_nc_ht=instagram.fbir1-1.fna.fbcdn.net&_nc_cat=101&_nc_oc=Q6cZ2QFMRPJC7p3xVXY1TGd36CVEGTVaPjfDMqyZ2idz-ZUv6jgJSTOjuMSCFA70BqJ-pO3Pk-LbPS-3sCyyReZZN2AR&_nc_ohc=bb8RXCg_X70Q7kNvwHFecCC&_nc_gid=q5RCd0S6jGdeNvJKUOjECA&edm=AP4sbd4BAAAA&ccb=7-5&oh=00_AfjitFTvsqPbXvLSrj9DCE-PzcDGZPpyIHF5GZMQ-_we2A&oe=692A2E55&_nc_sid=7a9f4b"
                    }
                };

                foreach (var member in members)
                {
                    var result = await userManager.CreateAsync(member, "Shovaan345@#");

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(member, "Member");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            logger.LogError($"Error while creating user {member.UserName}: {error.Description}");
                        }
                    }
                }

                var admin = new AppUser
                {
                    UserName = "admin",
                    Email = "admin@talentverse.com",
                    Bio = "I am the admin of TalentVerse."
                };

                var adminResult = await userManager.CreateAsync(admin, "adminofTalentVerse@123");

                if (adminResult.Succeeded)
                {
                    await userManager.AddToRolesAsync(admin, new[] { "Admin", "Member" });
                    logger.LogInformation("Seeding completed successfully.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"An error occurred while seeding users: {ex.Message}");
                throw;
            }

        }
    }
}
