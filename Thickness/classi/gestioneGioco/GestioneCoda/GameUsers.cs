using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thickness.classi.gestioneCash;
using Thickness.classi.gestionePersonale.abbonamenti;

namespace Thickness.classi.gestioneGioco.GestioneCoda
{
    internal class GameUsers
    {
        private List<User> users;
        private List<Abbonamento> abbons;
        public Spese spese;

        public List<Abbonamento> abbonsFiniti;

        public GameUsers()
        {
            users = new List<User>();
            abbons = new List<Abbonamento>();
            abbonsFiniti = new List<Abbonamento>();
            spese = new Spese();

            
        }

        public bool AddUser(User user)
        {
            if(!spese.addUtente())
            {
                return false;
            }
            users.Add(user);
            return true;
        }

        public bool AlreadyExists(User user)
        {
            foreach (User u in users)
            {
                if (u.Equals(user))
                {
                    return true;
                }
            }
            return false;
        }

        public User findUser(string nome, string cognome)
        {
            foreach (User u in users)
            {
                if(u.nome.Equals(nome) && u.cognome.Equals(cognome))
                {
                    return u;
                }
            }
            return null;
        }

        public int Count()
        {
            return users.Count;
        }

        public void addabbon(Abbonamento abbon)
        {
            abbons.Add(abbon);
        }

        public void addabbon(User user, DateTime inizio, DateTime fine, int prezzo)
        {
            abbons.Add(new Abbonamento(user, inizio, fine, prezzo));
            if (!users.Contains(user))
            {
                this.AddUser(user);
            }
        }

        public bool isAlreadyAbb(User s)
        {
            foreach (var u in abbons)
            {
                if (u.user.Equals(s))
                {
                    return true;
                }
            }

            return false;
        }

        public List<User> abbAttivi(DateTime now)
        {
            var list = new List<User>();
            var listAbbFiniti = new List<Abbonamento>();
            var listAbbAttivi = new List<Abbonamento>();
            foreach (var u in abbons)
            {
                if (u.fine > now)
                {
                    list.Add(u.user);
                    listAbbAttivi.Add(u);
                }
                else
                {
                    listAbbFiniti.Add(u);
                }
            }

            this.abbons = listAbbAttivi;
            this.abbonsFiniti = listAbbFiniti;

            for (int i=0; i< listAbbFiniti.Count; i++)
            {
                spese.removeUtente();
            }

            return list;
        }

        public List<Abbonamento> getActiveAbb()
        {
            return this.abbons;
        }

        public List<Abbonamento> getEndedAbb()
        {
            return this.abbonsFiniti;
        }

        public long getSpesa()
        {
            return spese.pagamentoMensile();
        }

        public bool addMacchinario()
        {
            return spese.addMacchinario();
        }

        public int getMacchinari()
        {
            return spese.GetMacchinari();
        }

        public bool addPiano()
        {
            return spese.addPiano();
        }

        public int getPiani()
        {
            return spese.getPiani();
        }

        public long getSpeseTot()
        {
            return spese.spesetot;
        }

        public int getMaxUsers()
        {
            return spese.getMaxUsers();
        }
    }
}

