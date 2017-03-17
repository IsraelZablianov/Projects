using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgammonLogic
{
    public class Player
    {
        public Player(Cell tool)
        {
            Tool = tool;
        }

        public int AmountOfWins
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public Cell Tool
        {
            get;
            set;
        }
    }
}
