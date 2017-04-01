using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgammonLogic
{
    public class Dice
    {
        private int _firstDie;
        private int _secondDie;
        private Random _diceValueGenerator = new Random();

        public int SecondDie
        {
            get
            {
                return _secondDie;
            }

            set
            {
                _secondDie = value;
            }
        }

        public int FirstDie
        {
            get
            {
                return _firstDie;
            }

            set
            {
                _firstDie = value;
            }
        }

        public void RollTheDice()
        {
            _firstDie = _diceValueGenerator.Next(1, 7);
            _secondDie = _diceValueGenerator.Next(1, 7);
        }

        public override string ToString()
        {
            return $"[{_firstDie}],[{_secondDie}]";
        }
    }
}
