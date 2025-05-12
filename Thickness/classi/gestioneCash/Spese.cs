namespace Thickness.classi.gestioneCash
{
    internal class Spese
    {

        //Sistemare Ufficio, in base ai prezzi le utenze aumentano/diminuiscono
        //I macchinari massimo 30 per piano, quindi sistemare anche setup piani
        //10k ogni macchinario - 100k ogni piano - +5 al giorno per ogni piano - +3 al giorno per ogni macchinario
        //Sponsor a fine mese restituisce 25% per ogni 50k spesi *Fine mese*
        //per ogni macchinario massimo 5 iscritti
        //500k debito iniziale per aprire
        //300 euro ogni mese di base


        //Trama
        //Provieni da una famiglia milionaria e tuo padre ti da del fallito, e te per ridimostrarti ti metti nei debiti 
        //e adesso il tuo obbiettivo è diventare la palestra più rinomata del paese e diventare tu stesso milionario

        int Macchinari;
        int AffittoGionaliero;
        int maxMacchinari;
        int utenti;


        public Spese()
        {
            Macchinari = 12;
            AffittoGionaliero = 50 + 10;
            maxMacchinari = 30;
            utenti = 0;
        }

        public Spese(int macchinari, int affittoGionaliero, int maxMacchinari, int utenti)
        {
            Macchinari = macchinari;
            AffittoGionaliero = affittoGionaliero;
            this.maxMacchinari = maxMacchinari;
            this.utenti = utenti;
        }

        public int GetMacchinari()
        {
            return Macchinari;
        }

        public int GetAffittoGionaliero()
        {
            return AffittoGionaliero;
        }

        public bool addMacchinario()
        {
            if (Macchinari == maxMacchinari)
            {
                return false;
            }
            Macchinari++;
            AffittoGionaliero += 5;
            return true;

        }

        public bool addUtente()
        {
            if(Macchinari * 3 <= utenti)
            {

                return false; ;
            }
            utenti++;
            if (utenti % 5 == 0)
            {
                AffittoGionaliero += 5;
            }
            return true;
        }

        public int GetUtenti()
        {
            return utenti;
        }

        public void removeUtente()
        {
            utenti--;
            if (utenti % 5 == 0)
            {
                AffittoGionaliero -= 5;
            }
        }

        public void resetUtente()
        {
            utenti = 0;
            AffittoGionaliero = 50 + 10;
        }



    }
}
