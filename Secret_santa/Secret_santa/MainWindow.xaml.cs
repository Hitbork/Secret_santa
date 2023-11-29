using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Secret_santa
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string connectionString;

        public MainWindow()
        {
            InitializeComponent();
            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.IsEnabled = true;
            timer.Tick += (o, t) => { CurrentTime.Content = DateTime.Now.ToString(); };
            timer.Start();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы точно хотите закрыть?", "Exit window", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if (!(e.Content is Page page)) return;
        }
    }
}
