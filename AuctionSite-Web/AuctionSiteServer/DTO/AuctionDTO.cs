﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSiteServer.DTO
{
    public class AuctionDTO
    { 
        public string ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double StartBid { get; set; }
        public bool IsItemNew { get; set; }     
        public string Picture1 { get; set; }
        public string Picture2 { get; set; }
        public string Picture3 { get; set; }
        public string Picture4 { get; set; }
        public int BidCount { get; set; }
        public UserDTO User { get; set; }
        public CategoryDTO Category { get; set; }
        public HighestBidDTO HighestBid { get; set; }

    }
}