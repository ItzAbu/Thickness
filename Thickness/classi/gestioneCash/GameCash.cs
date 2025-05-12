using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thickness.classi.gestioneCash
{
    internal class GameCash
    {
        double cash;

        public GameCash()
        {
            this.cash = 0;
        }

        public bool addCash(double amount)
        {
            if(amount < 0)
            {
                return false; // Invalid amount to add
            }
            this.cash += amount;
            return true;
        }
        
        public double getCash()
        {
            return this.cash;
        }

        public bool remCash(double amount)
        {
            if(amount < 0)
            {
                return false; // Invalid amount to remove
            }
            this.cash -= amount;
            return true;
        }
    }
}
