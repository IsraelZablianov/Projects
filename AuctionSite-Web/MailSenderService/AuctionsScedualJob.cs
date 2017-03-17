using System;
using System.Linq;
using System.Text;
using AuctionSiteServer;
using MailSender;

namespace MailSenderService
{
    public class AuctionsScedualJob
    {
        public void DeleteEndedAuctions()
        {
            var deleteAuctionMailSender = new AuctionsSiteMailSender();
            var manager = new DBManager();
            var endedAuctions = manager.GetEndedAuctions();

            for (int i = 0; i < endedAuctions.Length; i++)
            {
                manager.DeleteAuction(endedAuctions[i].ID,false);
            }

        }
 
    }
}
