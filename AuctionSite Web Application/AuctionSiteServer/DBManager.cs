using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using MailSender;
using Model;

namespace AuctionSiteServer
{
    public class DBManager
    {
        private const int PartRange = 20;

        public DBManager(){ }

        public void AddUser(User user)
        {
            using (DatabaseContext databaseContext = new DatabaseContext())
            {
                if (Users(databaseContext).FirstOrDefault(u => u.ID == user.ID) == null)
                {
                    databaseContext.Users.Add(user);
                    databaseContext.SaveChanges();
                    var auctionsSiteMailSender = new AuctionsSiteMailSender();
                    auctionsSiteMailSender.SendNewUserMail(user);
                }
            }
        }

        public bool Login(string userName, string password)
        {
            return IsUserExists(userName, password);
        }

        public bool IsUserExists(string userName, string password)
        {
            using (DatabaseContext databaseContext = new DatabaseContext())
            {
                User user = Users(databaseContext).FirstOrDefault(u => u.ID == userName && u.Password == password);

                return user != null;
            }
        }

        public Category [] GetCategories()
        {
            using (DatabaseContext databaseContext = new DatabaseContext())
            {
                var allCategories = Categories(databaseContext);

                return allCategories.ToArray();
            }
        }

        public Auction GetAuction(string id)
        {
            using (DatabaseContext databaseContext = new DatabaseContext())
            {
                Auction auction = Auctions(databaseContext).FirstOrDefault(a => a.ID == id);

                return auction;
            }
        }

        public Auction[] GetAuctions()
        {
            using (DatabaseContext databaseContext = new DatabaseContext())
            {
                var allAuctions = Auctions(databaseContext);

                return allAuctions.ToArray();
            }
          
        }

        public Auction[] GetAuctions(int partNumber)
        {
            using (var databaseContext = new DatabaseContext())
            {
                if (partNumber*PartRange > databaseContext.Auctions.Count()) return null;
                return Auctions(databaseContext).Skip(partNumber*PartRange).Take(PartRange).ToArray();
            }
        }

        public Auction[] GetEndedAuctions()
        {
            using (DatabaseContext databaseContext = new DatabaseContext())
            {

                var allAuctions = Auctions(databaseContext).Where(a=>a.EndTime < DateTime.UtcNow);

                return allAuctions.ToArray();
            }

        }

        public void AddAuction(Auction auction)
        {
            using (DatabaseContext databaseContext = new DatabaseContext())
            {
                auction.ID = Guid.NewGuid().ToString("N");
                databaseContext.Auctions.Add(auction);
                databaseContext.SaveChanges();
            }
        }

        public bool AddBid(Bid bid)
        {
            using (DatabaseContext databaseContext = new DatabaseContext())
            {
                var auction = Auctions(databaseContext).FirstOrDefault(a => a.ID == bid.AuctionID);
                var lastBid = auction?.Bids.LastOrDefault();
                if (lastBid != null && bid.Offer <= lastBid.Offer) return false;               
                databaseContext.Bids.Add(bid);
                databaseContext.SaveChanges();
                if (lastBid != null)
                {
                    SendMailToHeigerBidder(databaseContext, lastBid, auction);
                }
                return true;
            }
        }

        private void SendMailToHeigerBidder(DatabaseContext databaseContext, Bid lastBid, Auction auction)
        {
            var mailSender = new AuctionsSiteMailSender();
            var lastBidder = Users(databaseContext).FirstOrDefault(u => u.ID == lastBid.UserID);
            mailSender.SendHeigherBidMail(auction, lastBidder);
        }

        public void DeleteAuction(string auctionId, bool initiated)
        {
            using (DatabaseContext databaseContext = new DatabaseContext())
            {
                var deleteAuctionMailSender = new AuctionsSiteMailSender();
                var auction = Auctions(databaseContext).FirstOrDefault(a => a.ID == auctionId);
                var buyer = auction.Bids.Count > 0
                    ? Users(databaseContext).FirstOrDefault(u => u.ID == auction.Bids.Last().UserID)
                    : null;
                if (initiated)
                    deleteAuctionMailSender.SendInitiatedDeleteMail(auction, buyer);
                else
                    deleteAuctionMailSender.SendNotInitiatedDeleteMail(auction, buyer);

                databaseContext.Auctions.Remove(auction);
                databaseContext.SaveChanges();
            }
        }

        private IEnumerable<Auction> Auctions(DatabaseContext databaseContext)
        {
                return databaseContext.Auctions
                    .Include(a => a.User)
                    .Include(a => a.Category)
                    .Include(a => a.Bids).ToList();
        }

        private IEnumerable<Bid> Bids(DatabaseContext databaseContext)
        {
                return databaseContext.Bids
                    .Include(b => b.Auction)
                    .Include(b => b.User);
        }

        private IEnumerable<Category> Categories(DatabaseContext databaseContext)
        {           
                return databaseContext.Categories
                    .Include(c => c.Auctions);
        }

        private IEnumerable<User> Users(DatabaseContext databaseContext)
        {
            return databaseContext.Users
                .Include(u => u.Auctions)
                .Include(u => u.Bids);

        }


    }
}    

