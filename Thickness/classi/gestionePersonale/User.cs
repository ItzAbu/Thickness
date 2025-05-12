using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thickness.classi
{
    internal class User
    {

        public string nome { get; set; }
        public string cognome { get; set; }
        public string email { get; set; }
        public string CodF { get; set; }
        public string numeroTelefono { get; set; }
        public string gender { get; set; }
        public DateTime dataNascita { get; set; }
        public string residenza { get; set; }

        public User(string nome, string cognome, string email, string codF,
               string numeroTelefono, string gender, DateTime dataNascita, string residenza)
        {
            this.nome = nome;
            this.cognome = cognome;
            this.email = email;
            this.CodF = codF;
            this.numeroTelefono = numeroTelefono;
            this.gender = gender;
            this.dataNascita = dataNascita;
            this.residenza = residenza;
        }
    }
}
