using ExterniUredjajKlijentApp.Uposlenici.Model;
using ExterniUredjajKlijentApp.Uposlenici.Model.Servis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExterniUredjajKlijentApp.Uposlenici.ViewModel
{
    class UposleniciViewModel
    {
        public Uposlenik CreateUposlenik { get; set; }
        ExterniServis eksterniServis;

        public UposleniciViewModel()
        {
            eksterniServis = new ExterniServis();
            CreateUposlenik = new Uposlenik();
        }

        public void DodajUposlenika()
        {
            eksterniServis.dodajKorisnika(CreateUposlenik);
        }

    }
}
