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
    /// Логика взаимодействия для CheckForPairs.xaml
    /// </summary>
    public partial class CheckForPairs : Window
    {
        public class Pair
        {
            public string sender { get; set; }
            public string receiver { get; set; }
        }

        public CheckForPairs()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<Pair> pairs = new List<Pair>();

            string[] strings = File.ReadAllLines(@"..\..\Pairs_of_players.txt");

            foreach (string s in strings)
            {
                Pair pair = new Pair();

                pair.sender = s.Split('-')[0];
                pair.receiver = s.Split('-')[1];

                pairs.Add(pair);
            }

            PairsDataGrid.ItemsSource= pairs;
        }
    }
}
