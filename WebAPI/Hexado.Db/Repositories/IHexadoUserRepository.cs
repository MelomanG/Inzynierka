using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Functional.Maybe;
using Hexado.Db.Entities;
using Microsoft.AspNetCore.Identity;

namespace Hexado.Db.Repositories
{
    public interface IHexadoUserRepository: IRepository<HexadoUser>
    {
        Task<IdentityResult> CreateAsync(HexadoUser user, string password);
        Task<Maybe<HexadoUser>> GetUserIncludeTokensAsync(Expression<Func<HexadoUser, bool>> expression);
    }
}