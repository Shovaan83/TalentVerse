using TalentVerse.WebAPI.Data.Entities;

namespace TalentVerse.WebAPI.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);
    }
}
