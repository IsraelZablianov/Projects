using AuctionSiteServer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace AuctionSiteServer.serverController
{
    public static class Mapper
    {

        public static User ToUser(UserRegistrationDTO userRegistrationDto)
        {
            return new User()
            {
                ID = userRegistrationDto.ID,
                Name = userRegistrationDto.Name,
                Email = userRegistrationDto.Email,
                Password = userRegistrationDto.Password,
                CreatedOn = DateTime.Now,
                LastLoginTime = DateTime.Now
            };
        }

        public static UserDTO ToUserDTO(User user)
        {
            return new UserDTO()
            {
                ID = user.ID,
                Name = user.Name,
                Email = user.Email,
                CreatedOn = user.CreatedOn,
                LastLoginTime = user.LastLoginTime,
            };
        }

        public static Bid ToBid(HighestBidDTO highestBidDto, string auctionId, string userId)
        {
            return new Bid()
            {
                AuctionID = auctionId,
                UserID = userId,
                BidTime = highestBidDto.BidTime,
                Offer = highestBidDto.Bid,
            };
        }

        public static HighestBidDTO ToHighestBidDTO(Bid bid)
        {
            return new HighestBidDTO()
            {
                Bid = bid.Offer,
                BidTime = bid.BidTime
            };
        }

        public static Auction ToAuction(AuctionAddingDTO auctionAddingDto, string userId)
        {
            DBManager manager = new DBManager();
            return new Auction()
            {
               
                CategoryID = auctionAddingDto.CategoryId,
                Description = auctionAddingDto.Description,
                EndTime = auctionAddingDto.EndTime,
                StartTime = auctionAddingDto.StartTime,
                IsItemNew = auctionAddingDto.IsItemNew,
                Picture1 = auctionAddingDto.Picture1,
                Picture2 = auctionAddingDto.Picture2,
                Picture3 = auctionAddingDto.Picture3,
                Picture4 = auctionAddingDto.Picture4,
                StartBid = auctionAddingDto.StartBid,
                Title = auctionAddingDto.Title,
                UserID = userId,
                
            };
        }

        public static Category ToCategory(CategoryDTO categoryDto)
        {
            return new Category()
            {
                ID = categoryDto.ID,
                Name = categoryDto.Name
            };
        }

        public static CategoryDTO ToCategoryDTO(Category category)
        {
            return new CategoryDTO()
            {
                ID = category.ID,
                Name = category.Name
            };
        }

        public static AuctionDTO ToAuctionDTO(Auction auction)
        {
            return new AuctionDTO()
            {
                BidCount = auction.Bids.Count,
                Category = ToCategoryDTO(auction.Category),
                Description = auction.Description,
                EndTime = auction.EndTime,
                StartTime = auction.StartTime,
                IsItemNew = auction.IsItemNew,
                Picture1 = auction.Picture1,
                Picture2 = auction.Picture2,
                Picture3 = auction.Picture3,
                Picture4 = auction.Picture4,
                StartBid = auction.StartBid,
                Title = auction.Title,
                HighestBid = auction.Bids.Count != 0 ? ToHighestBidDTO(auction.Bids.Last()) : null,
                User = ToUserDTO(auction.User),
                ID = auction.ID

            };
        }
    }
}