using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExterniUredjajServis.Uposlenici.Models
{
    public class Uposlenik
    {
        public int Id { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Pozicija { get; set; }
        public string RfidKartica { get; set; }
        public string KreditnaKartica { get; set; }
        public byte[] Slika { get; set; }
        public virtual ICollection<Dogadjaj> Dogadjaji { get; set; }
    }
}