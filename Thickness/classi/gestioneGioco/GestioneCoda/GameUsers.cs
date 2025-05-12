using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thickness.classi.gestionePersonale.abbonamenti;

namespace Thickness.classi.gestioneGioco.GestioneCoda
{
    internal class GameUsers
    {
        private List<User> users;
        private List<Abbonamento> abbons;

        //Implementare che quando uno finisce l'abbonamento puo riabbonarsi
        public List<Abbonamento> abbonsFiniti;

        public GameUsers()
        {
            users = new List<User>();
            abbons = new List<Abbonamento>();
            abbonsFiniti = new List<Abbonamento>();
        }

        public void AddUser(User user)
        {

            users.Add(user);
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
            foreach (var u in abbons)
            {
                if (u.fine > now)
                {
                    list.Add(u.user);
                }
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

    }
}

