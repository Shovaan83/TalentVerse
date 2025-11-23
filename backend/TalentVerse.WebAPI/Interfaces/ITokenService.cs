using TalentVerse.WebAPI.Data.Entities;

namespace TalentVerse.WebAPI.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
