using System.Threading.Tasks;
using Hexado.Db.Entities;
using Microsoft.AspNetCore.Identity;

namespace Hexado.Core.Services
{
    public interface IHexadoUserService
    {
        Task<IdentityResult> CreateAsync(HexadoUser user, string password);
    }
}