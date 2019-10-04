﻿using System.Threading.Tasks;
using Hexado.Db.Entities;
using Hexado.Db.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Hexado.Core.Services
{
    public class HexadoUserService : IHexadoUserService
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
    }
}