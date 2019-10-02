using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hexado.Db.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Hexado.Core.Services
{
    public interface IHexadoUserService
    {
        Task<ValidationResult> CreateAsync(HexadoUser user, string password);
    }

    public class HexadoUserService : IHexadoUserService
    {
        private readonly UserManager<HexadoUser> _userManager;

        public HexadoUserService(UserManager<HexadoUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ValidationResult> CreateAsync(HexadoUser user, string password)
        {
            var foundedUser = await _userManager.FindByEmailAsync(user.Email);
            if (foundedUser != null)
                new ValidationResult();

            var result = await _userManager.CreateAsync(foundedUser, password);
        }
    }
}