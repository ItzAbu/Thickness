using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Thickness.classi
{
    internal class Admin
    {
        public string Nome {  get; set; }
        public string code { get; set; }

        public Admin(string nome, string code)
        {
            Nome = nome;
            this.code = code;
        }
    }
}
