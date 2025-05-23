using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thickness.classi.gestioneGioco.GestioneMacchinari
{
    internal class Macchinario
    {
        public int postiLiberi { get; set; }
        public int postiTotali { get; }

        public Macchinario()
        {
            postiTotali = 5;
            postiLiberi = 5;
        }

        public Macchinario(int postiLiberi)
        {
            this.postiLiberi = postiLiberi;
            postiTotali = 5;
        }

        public bool addUtente()
        {
            if (postiLiberi > 0)
            {
                postiLiberi--;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool removeUtente()
        {
            if (postiLiberi < postiTotali)
            {
                postiLiberi++;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void removeAll()
        {
            postiLiberi = 5 ;
        }
    }
}
