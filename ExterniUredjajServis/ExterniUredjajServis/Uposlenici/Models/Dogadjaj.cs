using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExterniUredjajServis.Uposlenici.Models
{
    public class Dogadjaj
    {
        public int Id { get; set; }
        public DateTime DatumVrijemeDogadjaja { get; set; }
        public int UposlenikId { get; set; }
        public virtual Uposlenik Uposlenik { get; set; }
    }
}