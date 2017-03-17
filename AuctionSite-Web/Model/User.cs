using System;
using System.Collections.Generic;

namespace Model
{
    public class User
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime LastLoginTime { get; set; }
        public DateTime CreatedOn { get; set; }//Date for example2015-01-01T00:00:00

        public virtual ICollection<Auction> Auctions { get; set; }
        public virtual ICollection<Bid> Bids { get; set; }


    }
}
