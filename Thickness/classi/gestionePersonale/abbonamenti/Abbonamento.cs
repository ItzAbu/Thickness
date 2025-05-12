using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thickness.classi.gestioneTempo;

namespace Thickness.classi.gestionePersonale.abbonamenti
{
    internal class Abbonamento
    {
        public User user;
        public DateTime inizio;
        public DateTime fine;
        public int prezzo;

        public Abbonamento(User user, DateTime inizio, DateTime fine, int prezzo)
        {
            this.user = user;
            this.inizio = inizio;
            this.fine = fine;
            this.prezzo = prezzo;
        }
    }
}
