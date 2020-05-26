using Hexado.Db.Entities;
using Hexado.Web.Models;
using Hexado.Web.Models.Responses;

namespace Hexado.Web.Extensions.Models
{
    public static class RateExtension
    {
        public static BoardGameRate ToBoardGameRateEntity(this RateModel model, string userId, string gameBoardId)
        {
            return model.ToBoardGameRateEntity(userId, gameBoardId, default);
        }

        public static BoardGameRate ToBoardGameRateEntity(this RateModel model, string userId, string gameBoardId, string? rateId)
        {
            return new BoardGameRate
            {
                Id = rateId,
                HexadoUserId = userId,
                BoardGameId = gameBoardId,
                UserRate = model.UserRate,
                Comment = model.Comment
            };
        }
        public static PubRate ToPubRateEntity(this RateModel model, string userId, string pubId)
        {
            return model.ToPubRateEntity(userId, pubId, default);
        }

        public static PubRate ToPubRateEntity(this RateModel model, string userId, string pubId, string? rateId)
        {
            return new PubRate
            {
                Id = rateId,
                HexadoUserId = userId,
                PubId = pubId,
                UserRate = model.UserRate,
                Comment = model.Comment
            };
        }

        public static RateResponse ToRateResponse(this BoardGameRate entity)
        {
            return new RateResponse
            {
                Id = entity.Id,
                UserRate = entity.UserRate,
                Comment = entity.Comment,
                UserName = entity?.HexadoUser?.UserName
            };
        }

        public static RateResponse ToRateResponse(this PubRate entity)
        {
            return new RateResponse
            {
                Id = entity.Id,
                UserRate = entity.UserRate,
                Comment = entity.Comment,
                UserName = entity?.HexadoUser?.UserName
            };
        }
    }
}
