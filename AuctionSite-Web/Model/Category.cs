using System.Collections.Generic;

namespace Model
{
    public class Category
    {
        public int ID { get; set; } 
        public string Name { get; set; }

        public virtual ICollection<Auction> Auctions { get; set; }

}
}
