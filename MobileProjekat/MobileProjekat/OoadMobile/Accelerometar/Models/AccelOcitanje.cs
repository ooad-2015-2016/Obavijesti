using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileProjekat.OoadMobile.Accelerometar.Models
{
    public class AccelOcitanje
    {
        //trenutak vremena u kojem je ocitano
        public int Milisec { get; set; }
        //vrijednost od -1 do 1
        public double Vrijednost { get; set; }
    }
}
