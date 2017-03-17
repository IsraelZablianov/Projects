using System;


namespace Model
{
    public class Bid
    {
        public long ID { get; set; }
        public string AuctionID { get; set; }
        public string UserID { get; set; }
        public double Offer{ get; set; }
        public DateTime BidTime { get; set; }
        
        public virtual Auction Auction { get; set; }
        public virtual User User { get; set; }
    }
}
