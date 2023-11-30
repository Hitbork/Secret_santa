using System;
using System.IO;
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
using System.Windows.Shapes;

namespace Secret_santa.Pages
{
    /// <summary>
    /// Логика взаимодействия для CheckForMails.xaml
    /// </summary>
    public partial class CheckForMails : Window
    {
        public class Pair
        {
            public string name { get; set; }
            public string mail { get; set; }
        }


        public CheckForMails()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<Pair> pairs = new List<Pair>();

            string[] strings = File.ReadAllLines(@"..\..\Players.txt");

            foreach (string s in strings)
            {
                Pair pair = new Pair();

                pair.name = s.Split('|')[0];
                pair.mail = s.Split('|')[1];

                pairs.Add(pair);
            }

            PairsDataGrid.ItemsSource= pairs;
        }
    }
}
