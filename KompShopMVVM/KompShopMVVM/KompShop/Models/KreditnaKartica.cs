using KompShopMVVM.KompShop.Helper;
using Prism.Windows.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KompShopMVVM.KompShop.Models
{
    class KreditnaKartica : ValidatableBindableBase
    {
        private string brojKartice;
        //required polje validacija, potreban je SetProperty
        [Required(ErrorMessage = "Niste unijeli broj kartice")]
        public string BrojKartice {
            get { return brojKartice; }
            set { SetProperty(ref brojKartice, value); }
        }
        private DateTime datumIsteka;
        //Custom definirana Validacija uklasi LaterThanTodayPropertyAttribute
        [LaterThanTodayPropertyAttribute(ErrorMessage = "Datum mora biti u buducnosti")]
        public DateTime DatumIsteka
        {
            get { return datumIsteka; }
            set { SetProperty(ref datumIsteka, value); }
        }
        //primjer regular expression validacije
        private string ccv;
        [Required(ErrorMessage = "Niste unijeli Ccv"), RegularExpression(@"\d{4}", ErrorMessage = "Ccv je 4 cifre!")]
        public string Ccv
        {
            get { return ccv; }
            set { SetProperty(ref ccv, value); }
        }
    }
}
