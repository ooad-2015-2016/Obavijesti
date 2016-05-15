using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ExterniUredjajKlijentApp.Uposlenici.Model
{
    [DataContract]
    class Dogadjaj
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public DateTime DatumVrijemeDogadjaja { get; set; }
        [DataMember]
        public int UposlenikId { get; set; }
        public virtual Uposlenik Uposlenik { get; set; }
    }
}
