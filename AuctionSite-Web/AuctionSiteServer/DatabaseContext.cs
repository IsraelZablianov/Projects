using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;


namespace AuctionSiteServer
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
            : base("AuctionDatabase")
        {
            //Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Auction> Auctions { get; set; }
        public DbSet<Bid> Bids { get; set; }
        public DbSet<Category> Categories { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bid>()
               .HasRequired(b => b.Auction)
               .WithMany(a => a.Bids)
               .HasForeignKey(b => b.AuctionID)
               .WillCascadeOnDelete(true);

            base.OnModelCreating(modelBuilder);
        }
    }
    
}
