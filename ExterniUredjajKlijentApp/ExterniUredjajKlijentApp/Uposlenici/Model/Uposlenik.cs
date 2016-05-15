using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ExterniUredjajKlijentApp.Uposlenici.Model
{
    [DataContract]
    class Uposlenik
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Ime { get; set; }
        [DataMember]
        public string Prezime { get; set; }
        [DataMember]
        public string Pozicija { get; set; }
        [DataMember]
        public string RfidKartica { get; set; }
        [DataMember]
        public string KreditnaKartica { get; set; }
        [DataMember]
        public byte[] Slika { get; set; }
        public virtual ICollection<Dogadjaj> Dogadjaji { get; set; }
    }
}
