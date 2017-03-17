using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace AuctionSiteClient
{
    [HubName("auctionAdd")]
    public class AuctionAdd : Hub
    {
        public void NotifyAuctionWasAdded()
        {
            Clients.All.OnAuctionAdded();
        }
    }
}