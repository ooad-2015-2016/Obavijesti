using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace ProjekatOoad.Helper
{
    //ITriggerValue Trazi Active i IsActive Changed
    public class BoolAndMinWidthStateTrigger : StateTriggerBase, ITriggerValue
    {

        public BoolAndMinWidthStateTrigger()
        {
            //staviti svoj event u da se izvrsi pri mijenjanju velicine prozora
            Window.Current.SizeChanged += Current_SizeChanged;
        }

        //ova metoda se poziva kad god je potrebno uraditi update triggera
        private void UpdateTrigger()
        {
            //postavi jel ili nije aktivan trigger
            //uslove u triggerima treba namjesititi tako da samo jedan ispunjava uslove u datom trenutku
            //ako jedan iskljucuje drugi
            IsActive = (BoolAndMinWidthStateTrigger.SkontajJelAktivanTrigger(VrijednostKojaSePrati, ZeljenaVrijednost, MinSirina, MaxSirina));
        }

        //ovo je vrijednost koja ima Binding
        public bool VrijednostKojaSePrati
        {
            get { return (bool)GetValue(VrijednostKojaSePratiProperty); }
            set { SetValue(VrijednostKojaSePratiProperty, value); }
        }

        //Ako se ovo definira onda ce se pojaviti mogucnost postavljanja atributa VrijednostKojaSePrati u xaml kontroli BoolAndMinWidthStateTrigger 
        public static readonly DependencyProperty VrijednostKojaSePratiProperty =
        //ovo ce povezati ono sto je pod navodnicima "AtributUXaml" sa VrijednostKojaSePrati
            DependencyProperty.Register("VrijednostKojaSePrati", typeof(bool), typeof(BoolAndMinWidthStateTrigger),
            new PropertyMetadata(null, OnValuePropertyChanged));

        //vrijednost koja bi trebala da bude jednaka Binding vrijednosti
        //klik na button search postavi VrijednostKojaSePrati na true i ako je zeljena vrijednost true i MinSirina ok aktivirati ce se state trigger
        public bool ZeljenaVrijednost
        {
            get { return (bool)GetValue(ZeljenaVrijednostProperty); }
            set { SetValue(ZeljenaVrijednostProperty, value); }
        }

        public static readonly DependencyProperty ZeljenaVrijednostProperty =
                    DependencyProperty.Register("ZeljenaVrijednost", typeof(bool), typeof(BoolAndMinWidthStateTrigger), new PropertyMetadata(null, OnValuePropertyChanged));

        //Ostala dva property na isti nacin i dobice se 4 atributa WindowWidth treba da bude < MinSirina a > MaxSirina
        public int MinSirina
        {
            get { return (int)GetValue(MinSirinaProperty); }
            set { SetValue(MinSirinaProperty, value); }
        }

        public static readonly DependencyProperty MinSirinaProperty =
                    DependencyProperty.Register("MinSirina", typeof(int), typeof(BoolAndMinWidthStateTrigger), new PropertyMetadata(null, OnValuePropertyChanged));

        public int MaxSirina
        {
            get { return (int)GetValue(MaxSirinaProperty); }
            set { SetValue(MaxSirinaProperty, value); }
        }

        public static readonly DependencyProperty MaxSirinaProperty =
                    DependencyProperty.Register("MaxSirina", typeof(int), typeof(BoolAndMinWidthStateTrigger), new PropertyMetadata(null, OnValuePropertyChanged));


        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (BoolAndMinWidthStateTrigger)d;
            obj.UpdateTrigger();
        }

        //Event koji je bio povezan sa promjenom prozora, kad se prozor promijeni provjeri state trigger jel se sta promijenilo
        private void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            UpdateTrigger();
        }

        //uslov koji odredjuje da li je aktivan trenutni state trigger
        internal static bool SkontajJelAktivanTrigger(bool vKojaSePrati, bool vZeljena, int minSirina, int maxSirina)
        {
            return (vKojaSePrati.Equals(vZeljena) && Window.Current.Bounds.Width > (int)minSirina && Window.Current.Bounds.Width < (int)maxSirina);
        }

        //Implementacija Interface
        //Promjena IsActive utice da dodje do izmjene state trigera i stila u trenutnom frame (uvijek ista implementacija)
        private bool m_IsActive;
        public bool IsActive
        {
            get { return m_IsActive; }
            private set
            {
                if (m_IsActive != value)
                {
                    m_IsActive = value;
                    base.SetActive(value);
                    if (IsActiveChanged != null)
                        IsActiveChanged(this, EventArgs.Empty);
                }
            }
        }
        public event EventHandler IsActiveChanged;
    }
}
