using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Thickness.classi.gestioneGioco.GestioneCoda
{
    internal class UsersGetter
    {

        public UsersGetter() { }

        private string path = @"../../script/utenti.json";

        public User GetRandomUser()
        {
            string json = File.ReadAllText(path);
            List<User> users = JsonConvert.DeserializeObject<List<User>>(json);
            return users[new Random().Next(users.Count)];
        }
    }

    
}
