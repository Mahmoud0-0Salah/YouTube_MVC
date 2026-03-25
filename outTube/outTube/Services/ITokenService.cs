using Microsoft.AspNetCore.Identity;
using outTube.Models;

namespace outTube.Services
{
    public interface ITokenService
    {
        Task<string> CreateToken(User user);
    }
}
