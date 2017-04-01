using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace AuctionSiteClient
{
    [HubName("auction")]
    public class AuctionHub : Hub
    {
        public void SendMessage(string message)
        {
            Clients.All.hello();
        }
    }
}