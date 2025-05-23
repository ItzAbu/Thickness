using System.Collections.Generic;
using Thickness.classi.gestioneGioco.GestioneMacchinari;

namespace Thickness.classi.gestioneCash
{
    internal class Spese
    {

        //Sistemare Ufficio

        //Trama
        //Provieni da una famiglia milionaria e tuo padre ti da del fallito, e te per ridimostrarti ti metti nei debiti 
        //e adesso il tuo obbiettivo è diventare la palestra più rinomata del paese e diventare tu stesso milionario

        List<Piano> piani;
        long AffittoMensile;
        public long spesetot { get; set; }

        public Spese()
        {
            piani = new List<Piano> { new Piano() };
            for (int i = 0; i < 5; i++)
            {
                piani[0].AddMacchinario();
            }
            AffittoMensile = (52) * 30; // Convertito in mensile  
            AffittoMensile += GetMacchinari() * 3 * 30; // Convertito in mensile
            spesetot = 0;
        }

        public Spese(List<Piano> piani, int affittoMensile, long spesetot)
        {
            AffittoMensile = affittoMensile;
            this.piani = piani;
            this.spesetot = spesetot;
        }

        public int GetMacchinari()
        {
            int macchinari = 0;
            foreach (var i in piani)
            {
                macchinari += i.Macchinari.Count;
            }
            return macchinari;
        }

        public long GetAffittoMensile()
        {
            return AffittoMensile;
        }

        public bool addMacchinario()
        {
            foreach (var i in piani)
            {
                if (i.AddMacchinario())
                {
                    AffittoMensile += 5 * 30; // Convertito in mensile  
                    spesetot += 2500;
                    return true;
                }
            }



            return false;
        }

        public bool addUtente()
        {
            foreach (var i in piani)
            {
                if (i.AddUtente())
                {
                    return true;
                }
            }
            return false;
        }

        public int GetUtenti()
        {
            int count = 0;
            foreach (var i in piani)
            {
                count += i.getUser();
            }
            return count;
        }

        public bool removeUtente()
        {
            foreach (var i in piani)
            {
                if (i.removeUtente())
                {
                    return true;
                }
            }
            return false;
        }

        public void resetUtente()
        {
            foreach (var i in piani)
            {
                i.removeAll();
            }
        }

        public bool setUser(int u)
        {
            foreach (var i in piani)
            {
                i.removeAll();
            }

            for (int i = 0; i < u; i++)
            {
                if (!addUtente())
                {
                    return false;
                }
            }
            return true;
        }

        public bool addPiano()
        {
            if (piani.Count < 30)
            {
                piani.Add(new Piano());
                AffittoMensile += 10 * 30; // Convertito in mensile  
                spesetot += 25000;
                return true;
            }
            else
            {
                return false;
            }
        }

        public long pagamentoMensile()
        {
            return AffittoMensile;

        }

        public int getPiani()
        {
            return piani.Count;
        }

        public int getMaxUsers()
        {
            int utenti = 0;
            foreach (var i in piani)
            {
                utenti += i.getUser();
            }
            return utenti;
        }
    }
}

