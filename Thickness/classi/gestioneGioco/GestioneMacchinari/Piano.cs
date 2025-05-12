using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thickness.classi.gestioneGioco.GestioneMacchinari
{
    internal class Piano
    {
        public List<Macchinario> Macchinari;

        public Piano()
        {
            Macchinari = new List<Macchinario> { new Macchinario() };
        }

        public Piano(List<Macchinario> macchinari)
        {
            Macchinari = macchinari;
        }

        public bool AddUtente()
        {
            foreach (var i in Macchinari)
            {
                if (i.addUtente())
                {
                    return true;
                }
            }
            return false;
        }

        public bool AddMacchinario()
        {
            if (Macchinari.Count < 30)
            {
                Macchinari.Add(new Macchinario());
                return true;
            }
            else
            {
                return false;
            }
        }

        public int getUser()
        {
            int utenti = 0;
            foreach (var i in Macchinari)
            {
                utenti += i.postiOccupati;
            }
            return utenti;

        }

        public bool removeUtente()
        {
            foreach (var i in Macchinari)
            {
                if (i.removeUtente())
                {
                    return true;
                }
            }
            return false;
        }

        public void removeAll()
        {
            foreach (var i in Macchinari)
            {
                i.removeAll();
            }
        }
    }
}
