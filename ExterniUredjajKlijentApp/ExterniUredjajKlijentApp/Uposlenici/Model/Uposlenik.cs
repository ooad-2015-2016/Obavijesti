﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ExterniUredjajKlijentApp.Uposlenici.Model
{
    //DataContract i DataMember su za Json Serijalizaciju 
    [DataContract]
    class Uposlenik : INotifyPropertyChanged
    {
        //DataMember kaze: Ovaj property ce se serijalizirati
        //a ko nema DataMember on nece
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Ime { get; set; }
        [DataMember]
        public string Prezime { get; set; }
        [DataMember]
        public string Pozicija { get; set; }
        private string rfidKartica;//Rfid string procitan sa kartice
        [DataMember]//Regex da ocisti simbole koji bi mogli prouzrokovati probleme poput enter znaka i slicno
        //mora prozvati property changed jer je na njega vezano polje koje se treba promijeniti kad se povuce kartica, bez ovog, textbox prazan
        public string RfidKartica { get { return rfidKartica; } set { rfidKartica = Regex.Replace(value, "[^0-9a-zA-Z]+", ""); OnNotifyPropertyChanged("RfidKartica"); } }
        [DataMember]
        public string KreditnaKartica { get; set; }
        public byte[] Slika { get; set; }
        public virtual ICollection<Dogadjaj> Dogadjaji { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnNotifyPropertyChanged([CallerMemberName] string memberName = "")
        {
            //? je skracena verzija ako nije null
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
        }
    }
}
