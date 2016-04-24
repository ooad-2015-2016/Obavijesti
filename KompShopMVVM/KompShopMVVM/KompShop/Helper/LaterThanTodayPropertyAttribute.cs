using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KompShopMVVM.KompShop.Helper
{
    class LaterThanTodayPropertyAttribute : ValidationAttribute
    {
        public LaterThanTodayPropertyAttribute()
        {
        }

        //IsValid potrebna za validaciju
        protected override ValidationResult IsValid(object value, ValidationContext obj)
        {
            //value je vrijednost koja se validira
            if (value == null)
            {
                //u konstruktor ide sta ce se ispisati ako faila validacija. ErrorMessage je postavljen pri definiranju anotacije nad poljem
                return new ValidationResult(this.ErrorMessage);
            }

            //Sve sto je starije od sadasnjeg vremena nije validno
            var compare = DateTime.Now.CompareTo((DateTime)value);
            if (compare >= 0)
            {
                return new ValidationResult(this.ErrorMessage);
            }
            //Ako nema problema dosad sve je Ok
            return ValidationResult.Success;
        }
    }
}