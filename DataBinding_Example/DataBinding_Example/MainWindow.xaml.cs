using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


//Hozzáadott függőségek
using System.ComponentModel; // Ez tartalmazza az INotification osztályt 
using System.Runtime.CompilerServices; // Valós idejű adatkötésért felelős szolgáltatás

using System.Windows.Threading;

namespace DataBinding_Example
{
    //public partial class MainWindow : Window <-ez volt az eredeti származtatás
    public partial class MainWindow : INotifyPropertyChanged // <- INotifyPropertyChanged egy interface, amin keresztül értesülünk a változásról. 
    {
        public MainWindow()
        {
            /* A DataContext segítségével megadható, hol dolgozunk, például egy adatbázisban.
             * Innetől a rendszer képes objektumokként kezelni az adatbázis rekordjait.
             * Képesek vagyunk LINQ val manipulálni az adatokat SQL helyett.
             * Jelnleg helyi objektumokkal akarunk dolgozni, íhy ez az ablak lesz a DataContextünk
             */
            DataContext = this; 
            InitializeComponent();
        }

        /* Gombok eseményei */

        /// <summary>
        /// A reset gomb által kiváltott esemény
        /// </summary>
        /// <param name="sender">A kiváltó objektum</param>
        /// <param name="e">A hozzá kapcsolódó esemény argumentumai</param>
        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            OsszekotoSzam = 0;
        }


        /* Mezők és a hozzájuk tartozó getter és setter függvények */

        /// <summary>
        /// Ezt a számot fogjuk módosítani, a UI segítségével
        /// </summary>
        private int osszekotoSzam = 0;

        /// <summary>
        /// Getter és setter segítségével hozzáférhetővé tesszük az osszekotoSzam-ot.
        /// A Setter megnézi, hogy változott-e az érték és csak akkor írja felül  ameglévőt, ha az különbözik az új értéktől, ezzel a felesleges changeket kihagyjuk a folyamatból.
        /// </summary>
        public int OsszekotoSzam
        {
            get { return osszekotoSzam; }
            set
            {
                if( osszekotoSzam != value)
                {
                    osszekotoSzam = value;
                    //  Meghívjuk az OnPropertyChamged függvényt, hogy változtassa meg az ablak tulajdonságait
                    OnPropertyChanged();
                }
            }
        }



        /* Segéd kód, az események kiváltásához */

        /// <summary>
        /// Létrehozunk egy példányt a tulajdonság változás esemény kezeléséből.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged; 

        /// <summary>
        /// Ezt a függvényt hívjuk meg, amikor a setter érzékeli a változást
        /// </summary>
        /// <param name="propertyName">A CallerMemberName segítségével az eseményt kiváltó nevét kapjuk meg, amit a propertyName stringbe tárolunk, az egyenlőség jelel null be állítjuk, ha nincs neki neve. </param>
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            
            // Változás esetén az Invoke segítségével beállítja:
            //      - Ezt az ablakot, mint az esemény kiváltó objektuma
            //      - Az esemény kiváltás argumentumának beállítja az eseményt meghívó nevét.   
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

   

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DispatcherTimer Idozito = new DispatcherTimer();
            Idozito.Interval = TimeSpan.FromSeconds(1);
            Idozito.Tick += IdoLepes;
            Idozito.Start();
        }

        private int ido = 0;
        private void IdoLepes(object sender, EventArgs e)
        {
            ido++;

            IdoText.Content = ido.ToString();
        }
    }
}
