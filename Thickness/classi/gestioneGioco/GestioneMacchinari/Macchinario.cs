using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thickness.classi.gestioneGioco.GestioneMacchinari
{
    internal class Macchinario
    {
        public int postiOccupati { get; set; }
        public int postiTotali { get; }

        public Macchinario()
        {
            postiTotali = 5;
            postiOccupati = 0;
        }

        public Macchinario(int postiOccupati)
        {
            this.postiOccupati = postiOccupati;
            postiTotali = 5;
        }

        public bool addUtente()
        {
            if (postiOccupati < postiTotali)
            {
                postiOccupati++;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool removeUtente()
        {
            if (postiOccupati > 0)
            {
                postiOccupati--;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void removeAll()
        {
            postiOccupati = 0;
        }
    }
}
