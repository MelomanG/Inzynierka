﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Functional.Maybe;
using Hexado.Db.Dtos;
using Hexado.Db.Entities;
using Hexado.Db.Repositories;
using Hexado.Db.Repositories.Specific;
using Microsoft.AspNetCore.Identity;

namespace Hexado.Core.Services.Specific
{
    public interface IHexadoUserService
    {
        Task<IdentityResult> CreateAsync(HexadoUser user, string password);
        Task<Maybe<HexadoUser>> GetSingleOrMaybeAsync(Expression<Func<HexadoUser, bool>> predicate);
        Maybe<IEnumerable<BoardGameDto>> GetLikedBoardGames(string userEmail);
        Maybe<IEnumerable<PubDto>> GetLikedPubs(string userEmail);
        Task UnLikeBoardGameAsync(string userEmail, string boardGameId);
        Task UnLikePubAsync(string userEmail, string pubId);
    }

    public class HexadoUserService : IHexadoUserService
    {
        private readonly IHexadoUserRepository _hexadoUserRepository;
        private readonly IRepository<LikedBoardGame> _likedBoardGameRepository;
        private readonly IRepository<LikedPub> _likedPubRepository;

        public HexadoUserService(
            IHexadoUserRepository hexadoUserRepository,
            IRepository<LikedBoardGame> likedBoardGameRepository,
            IRepository<LikedPub> likedPubRepository)
        {
            _hexadoUserRepository = hexadoUserRepository;
            _likedBoardGameRepository = likedBoardGameRepository;
            _likedPubRepository = likedPubRepository;
        }

        public async Task<IdentityResult> CreateAsync(HexadoUser user, string password)
        {
            return await _hexadoUserRepository.CreateAsync(user, password);
        }

        public async Task<Maybe<HexadoUser>> GetSingleOrMaybeAsync(Expression<Func<HexadoUser, bool>> predicate)
        {
            return await _hexadoUserRepository.GetSingleOrMaybeAsync(predicate, user => user.Account);
        }

        public Maybe<IEnumerable<BoardGameDto>> GetLikedBoardGames(string userEmail)
        {
            return _hexadoUserRepository.GetLikedBoardGames(userEmail);
        }

        public async Task UnLikeBoardGameAsync(string userEmail, string boardGameId)
        {
            var user = await _hexadoUserRepository.GetSingleOrMaybeAsync(
                u => u.Email == userEmail,
                u => u.LikedBoardGames);

            if (!user.HasValue)
                return;

            var toUnlike = user.Value.LikedBoardGames.FirstOrDefault(lbg => lbg.BoardGameId == boardGameId);
            if (toUnlike == null)
                return;
            await _likedBoardGameRepository.DeleteAsync(toUnlike);
        }

        public Maybe<IEnumerable<PubDto>> GetLikedPubs(string userEmail)
        {
            return _hexadoUserRepository.GetLikedPubs(userEmail);
        }

        public async Task UnLikePubAsync(string userEmail, string pubId)
        {
            var user = await _hexadoUserRepository.GetSingleOrMaybeAsync(
                u => u.Email == userEmail,
                u => u.LikedPubs);

            if (!user.HasValue)
                return;

            var toUnlike = user.Value.LikedPubs.FirstOrDefault(lbg => lbg.PubId == pubId);
            await _likedPubRepository.DeleteAsync(toUnlike);
        }
    }
}