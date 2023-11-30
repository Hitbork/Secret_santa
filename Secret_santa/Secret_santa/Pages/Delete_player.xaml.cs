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
    /// Логика взаимодействия для Delete_player.xaml
    /// </summary>
    public partial class Delete_player : Window
    {
        public Delete_player()
        {
            InitializeComponent();
        }

        private void DeletePlayerButton_Click(object sender, RoutedEventArgs e)
        {
            string path = @"..\..\Players.txt";

            if (NameTextBox.Text == String.Empty)
            {
                MessageBox.Show("Необходимо заполнить поле имя!");
                return;
            }

            if (File.ReadAllLines(path).Where(x => x.Trim().IndexOf(NameTextBox.Text) != -1).ToArray()[0] == String.Empty)
            {
                MessageBox.Show("Такого игрока не было найдено!");
                return;
            }

            File.WriteAllLines(path, File.ReadAllLines(path).Where(v => v.Trim().IndexOf(NameTextBox.Text) == -1).ToArray());

            MessageBox.Show($"Игрок {NameTextBox.Text} был удалён из списка!");

            File.WriteAllText(@"..\..\Pairs_of_players.txt", String.Empty);

            MessageBox.Show("Список пар был очищен! Обновите список для правильной работы программы");

            this.Close();
        }
    }
}
