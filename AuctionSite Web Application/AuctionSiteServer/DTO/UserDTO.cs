using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSiteServer.DTO
{
    public class UserDTO
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime LastLoginTime { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
