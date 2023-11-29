using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
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
using System.Runtime.InteropServices;

namespace Secret_santa.Pages
{
    /// <summary>
    /// Логика взаимодействия для Main_page.xaml
    /// </summary>
    public partial class Main_page : Page
    {
        public Main_page()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateListOfPLayers();
        }

        public void UpdateListOfPLayers()
        {
            string[] strings = File.ReadAllLines(@"..\..\Players.txt").ToArray();

            string players = "Игроки: ";

            foreach (string str in strings)
            {
                players += str.Split('|')[0];
                players += " ";
            }

            List_of_players.Text = players;
        }

        private void Edit_players_button_Click(object sender, RoutedEventArgs e)
        {
            Add_player add_Player = new Add_player();

            add_Player.ShowDialog();
        }

        private void Update_players_button_Click(object sender, RoutedEventArgs e)
        {
            UpdateListOfPLayers();
        }

        private void Send_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
                message.IsBodyHtml = true; //тело сообщения в формате HTML
                message.From = new MailAddress("egord2004@gmail.com", "Секретный санта"); //отправитель сообщения
                message.To.Add("hitbork@mail.ru"); //адресат сообщения
                message.Subject = "Твоя цель"; //тема сообщения
                message.Body = File.ReadAllText(@"..\..\Mail_basis.txt"); //тело сообщения

                using (System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient("smtp.gmail.com")) //используем сервера Google
                {
                    client.Credentials = new NetworkCredential("egord2004@gmail.com", "gqiz kmvw yasl gvtr"); //логин-пароль от аккаунта
                    client.Port = 587; //порт 587 либо 465
                    client.EnableSsl = true; //SSL обязательно

                    client.Send(message);
                    MessageBox.Show("Сообщение отправлено успешно!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetBaseException().Message);
            }
        }

        private void Delete_all_players_button_Click(object sender, RoutedEventArgs e)
        {
            File.WriteAllText(@"..\..\PLayers.txt", String.Empty);
            File.WriteAllText(@"..\..\Pairs_of_players.txt", String.Empty);

            MessageBox.Show("Все игроки были удаляны!");

            UpdateListOfPLayers();
        }
    }
}
