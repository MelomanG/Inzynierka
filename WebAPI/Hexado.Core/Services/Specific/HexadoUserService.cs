using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Functional.Maybe;
using Hexado.Db.Entities;
using Hexado.Db.Repositories.Specific;
using Microsoft.AspNetCore.Identity;

namespace Hexado.Core.Services.Specific
{
    public interface IHexadoUserService
    {
        Task<IdentityResult> CreateAsync(HexadoUser user, string password);
        Task<Maybe<HexadoUser>> GetSingleOrMaybeAsync(Expression<Func<HexadoUser, bool>> predicate);
    }

    public class HexadoUserService: IHexadoUserService
    {
        private readonly IHexadoUserRepository _hexadoUserRepository;

        public HexadoUserService(
            IHexadoUserRepository hexadoUserRepository)
        {
            _hexadoUserRepository = hexadoUserRepository;
        }

        public async Task<IdentityResult> CreateAsync(HexadoUser user, string password)
        {
            return await _hexadoUserRepository.CreateAsync(user, password);
        }

        public async Task<Maybe<HexadoUser>> GetSingleOrMaybeAsync(Expression<Func<HexadoUser, bool>> predicate)
        {
            return await _hexadoUserRepository.GetSingleOrMaybeAsync(predicate);
        }
    }
}