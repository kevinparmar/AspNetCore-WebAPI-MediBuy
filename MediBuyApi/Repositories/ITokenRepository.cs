using Microsoft.AspNetCore.Identity;
using System.Runtime.InteropServices;

namespace MediBuyApi.Repositories
{
    public interface ITokenRepository
    {
        string CreateJwtToken(IdentityUser user, List<string> roles);
    }
}
