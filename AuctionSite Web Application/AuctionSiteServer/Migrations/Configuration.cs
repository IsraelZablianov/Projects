using System.Collections.Generic;
using Model;

namespace AuctionSiteServer.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<AuctionSiteServer.DatabaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(AuctionSiteServer.DatabaseContext context)
        {
            var users = new List<User>
            {
            new User{ID="10001", Name="orel eliyahu",Email="orele1995@gmail.com",Password="123456", LastLoginTime= DateTime.UtcNow,CreatedOn= DateTime.UtcNow },
            new User{ID="10002", Name="israel",Email="orell@codevalue.net",Password="123456", LastLoginTime= DateTime.UtcNow,CreatedOn= DateTime.UtcNow },
            new User{ID="10003", Name="lidor ismach-moshe",Email="lidorm@codevalue.net",Password="123456", LastLoginTime= DateTime.UtcNow,CreatedOn= DateTime.UtcNow },
            new User{ID="10004", Name="Laura",Email="Laura@abc.com",Password="123456", LastLoginTime= DateTime.UtcNow,CreatedOn= DateTime.UtcNow },
            new User{ID="10005", Name="Nino",Email="Nino@abc.com",Password="123456", LastLoginTime= DateTime.UtcNow,CreatedOn= DateTime.UtcNow }
            };

            users.ForEach(u => context.Users.Add(u));
            context.SaveChanges();

            var categories = new List<Category>
            {
            new Category{ID=1, Name="Electronics"},
            new Category{ID=2, Name="Fashion"},
            new Category{ID=3, Name="Home"},
            new Category{ID=4, Name="Books"},
            new Category{ID=5, Name="Children"},
            new Category{ID=6, Name="Misc"}
            };

            categories.ForEach(c => context.Categories.Add(c));
            context.SaveChanges();

            var auctions = new List<Auction>();
            Random rnd = new Random();
            for (int i = 0; i < 100; i++)
            {
                auctions.Add(new Auction
                {
                    ID = Guid.NewGuid().ToString("N"),
                    Title = "Item "+i,
                    Description = "this is the description for item "+i,
                    StartTime = DateTime.UtcNow,
                    EndTime = new DateTime(2016, 12, 1).ToUniversalTime(),
                    StartBid = i,
                    IsItemNew = true,
                    Picture1 = @"http://www.funny-animalpictures.com/media/content/items/images/funnycats0041_O.jpg",
                    Picture2 = null,
                    Picture3 = null,
                    Picture4 = null,
                    UserID = rnd.Next(10001, 10006).ToString(),
                    CategoryID = i%6+1
                });
            }
            context.Auctions.AddRange(auctions);
            context.SaveChanges();

        }
    }
}
