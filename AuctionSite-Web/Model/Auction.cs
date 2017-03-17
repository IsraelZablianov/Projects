using System;
using System.Collections.Generic;

namespace Model
{
    public class Auction
    {
        public string ID{ get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double StartBid { get; set; }
        public bool IsItemNew { get; set; }
        public string UserID { get; set; }
        public string Picture1 { get; set; }
        public string Picture2 { get; set; }
        public string Picture3 { get; set; }
        public string Picture4 { get; set; }
        public int CategoryID { get; set; }

        public virtual ICollection<Bid> Bids { get; set; }
        public virtual Category Category { get; set; }
        public virtual User User { get; set; }

    }
}
