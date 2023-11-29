using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для Add_player.xaml
    /// </summary>
    public partial class Add_player : Window
    {
        public Add_player()
        {
            InitializeComponent();
        }

        private void AddPlayerButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(NameTextBox.Text))
            {
                MessageBox.Show("Имя игрока обязательно!");
                return;
            }

            using (StreamWriter writer = new StreamWriter(@"..\..\players.txt", true))
            {
                writer.WriteAsync($"{NameTextBox.Text}|");
                writer.WriteAsync($"{MailTextBox.Text}\n");
            }

            if (String.IsNullOrEmpty(MailTextBox.Text))
                MessageBox.Show($"Игрок {NameTextBox.Text} был добавлен без почты!");
            else
                MessageBox.Show($"Игрок {NameTextBox.Text} был добавлен");

            this.Close();
        }
    }
}
