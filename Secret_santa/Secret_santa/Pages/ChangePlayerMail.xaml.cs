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
    /// Логика взаимодействия для ChangePlayerMail.xaml
    /// </summary>
    public partial class ChangePlayerMail : Window
    {
        public ChangePlayerMail()
        {
            InitializeComponent();
        }

        private void ChangePlayerMailButton_Click(object sender, RoutedEventArgs e)
        {
            string path = @"..\..\PLayers.txt";

            if (MailTextBox.Text == String.Empty || NameTextBox.Text == String.Empty)
            {
                MessageBox.Show("Не все поля заполнены!");
                return;
            }

            if (File.ReadAllLines(path).Where(v => v.Trim().IndexOf(NameTextBox.Text) == -1).ToArray()[0] == String.Empty)
            {
                MessageBox.Show("Такого имени нет в игроках!");
                return;
            }

            if (!MailTextBox.Text.Contains("@"))
            {
                MessageBox.Show("Почта заполнена неверно!");
                return;
            }

            File.WriteAllLines(path, File.ReadAllLines(path).Where(v => v.Trim().IndexOf(NameTextBox.Text) == -1).ToArray());

            using (StreamWriter writer= new StreamWriter(path, true))
            {
                writer.WriteAsync($"{NameTextBox.Text}|{MailTextBox.Text}");
            }

            MessageBox.Show($"Почта у игрока {NameTextBox.Text} была успешно изменена!");
            
            this.Close();
        }
    }
}
