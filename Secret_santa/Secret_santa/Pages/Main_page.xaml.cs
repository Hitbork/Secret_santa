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

        private void Mix_all_players_button_Click(object sender, RoutedEventArgs e)
        {
            string[] strings = File.ReadAllLines(@"..\..\Players.txt");

            if (strings.Length == 0) {
                MessageBox.Show("Игроки не заполнены!");
                return;
            }

            if (strings.Length == 1) {
                MessageBox.Show("Игрок всего один!");
                return;
            }


            string[] playersFrom = strings.Select(x => x.ToString().Split('|')[0]).ToArray();

            while (true)
            {
                List<string> playersTo = playersFrom.ToList();

                File.WriteAllText(@"..\..\Pairs_of_players.txt", String.Empty);

                using (StreamWriter writer = new StreamWriter(@"..\..\Pairs_of_players.txt", true))
                {
                    string pair, playerTo;
                    Random rnd = new Random();

                    foreach (string player in playersFrom) {
                        pair = player + "-";

                        do
                        {
                            int i = rnd.Next(0, playersTo.Count());

                            playerTo = playersTo[i];

                            if (playerTo == player)
                                continue;

                            pair += playerTo;
                            playersTo.RemoveAt(i);
                        } while (playerTo == player);

                        writer.WriteAsync($"{pair}\n");
                    }
                }

                string lastString = File.ReadAllLines(@"..\..\Pairs_of_players.txt").Where(v => v.Trim().IndexOf($"{playersFrom[playersFrom.Length - 1]}-") != -1).ToArray()[0];

                if (lastString.Split('-')[0] == lastString.Split('-')[1])
                {
                    MessageBox.Show("Пары игроков не были сформированы, ещё одна попытка");
                    continue;
                }

                MessageBox.Show("Пары игроков были сформированы!");
                break;
            }
        }

        private void Send_to_all_button_Click(object sender, RoutedEventArgs e)
        {
            if (!DoEverybodyHaveAMail() || !AreEverybodyIncluded() || !AreEverybodyInPairs()) 
                return;

            string[] pairs = File.ReadAllLines(@"..\..\Pairs_of_players.txt");

            foreach (string pair in pairs) {
                SendAMail(pair.Split('-')[0], pair.Split('-')[1]);    
            }

            MessageBox.Show("Сообщения были отправлены!");
        }

        public bool DoEverybodyHaveAMail()
        {
            string playersThatDontHaveMail = "";
            string[] strings = File.ReadAllLines(@"..\..\Players.txt");

            foreach (string s in strings) {
                if (!s.Split('|')[1].Contains("@"))
                {
                    playersThatDontHaveMail += s.Split('|')[0] + ", ";
                }
            }

            if (!String.IsNullOrEmpty(playersThatDontHaveMail))
            {
                MessageBox.Show("У этих игроков нет почты или она неверна: " + playersThatDontHaveMail);
                return false;
            }

            return true;
        }

        public bool AreEverybodyIncluded()
        {
            string[] strings1 = File.ReadAllLines(@"..\..\Players.txt"),
                strings2 = File.ReadAllLines(@"..\..\Pairs_of_players.txt");

            if (strings1.Length != strings2.Length)
            {
                MessageBox.Show("Количество пар не равно количеству игроков!");
                return false;
            }

            return true;
        }

        public bool AreEverybodyInPairs()
        {
            string str = "";

            string[] strings = File.ReadAllLines(@"..\..\Players.txt");

            foreach (string s in strings)
            {
                if (File.ReadAllLines(@"..\..\Pairs_of_players.txt").Where(x => x.Trim().IndexOf($"{s.Split('|')[0]}-") != -1).ToArray().Length == 0)
                {
                    str += $"{s.Split('|')[0]} как отправитель,";
                }

                if (File.ReadAllLines(@"..\..\Pairs_of_players.txt").Where(x => x.Trim().IndexOf($"-{s.Split('|')[0]}") != -1).ToArray().Length == 0)
                {
                    str += $"{s.Split('|')[0]} как адресат,";
                }
            }
            
            if (!String.IsNullOrEmpty(str))
            {
                MessageBox.Show("Данных игроков нет в списке как: " + str);
                return false;
            }

            return true;
        }

        public void SendAMail(string sender, string receiver)
        {
            string mail = File.ReadAllLines(@"..\..\Players.txt").Where(v => v.Trim().IndexOf(sender) != -1).ToArray()[0].Split('|')[1];

            try
            {
                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
                message.IsBodyHtml = true; //тело сообщения в формате HTML
                message.From = new MailAddress("egord2004@gmail.com", "Тайный санта"); //отправитель сообщения
                message.To.Add(mail); //адресат сообщения
                message.Subject = "Твоя цель"; //тема сообщения
                message.Body = $"<div>Хо-хо-хо, привет {sender}!</div>";
                message.Body += "<div> </div>";
                message.Body += "<div>Это сообщение было сгенерировано специально для тебя, чтобы ты смог играть в тайного санту!</div>";
                message.Body += "<div> </div>";
                message.Body += "<h2 style=\"color: red;\">Твой тайный друг:</h2>";
                message.Body += $"<h1>{receiver}</h1>";
                
                using (System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient("smtp.gmail.com")) //используем сервера Google
                {
                    client.Credentials = new NetworkCredential("egord2004@gmail.com", "gqiz kmvw yasl gvtr"); //логин-пароль от аккаунта
                    client.Port = 587; //порт 587 либо 465
                    client.EnableSsl = true; //SSL обязательно

                    client.Send(message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetBaseException().Message);
            }
        }

        private void Change_player_mail_button_Click(object sender, RoutedEventArgs e)
        {
            ChangePlayerMail changePlayerMail= new ChangePlayerMail();

            changePlayerMail.ShowDialog();
        }

        private void Send_aim_for_button_Click(object sender, RoutedEventArgs e)
        {
            Send_aim_to_player send_Aim_To_Player = new Send_aim_to_player();

            send_Aim_To_Player.ShowDialog();
        }

        private void Check_mails_button_Click(object sender, RoutedEventArgs e)
        {
            CheckForMails checkForMails= new CheckForMails();

            checkForMails.ShowDialog();
        }

        private void Check_pairs_button_Click(object sender, RoutedEventArgs e)
        {
            CheckForPairs checkForPairs= new CheckForPairs();

            checkForPairs.ShowDialog();
        }

        private void Delete_player_button_Click(object sender, RoutedEventArgs e)
        {
            Delete_player delete_Player = new Delete_player();

            delete_Player.ShowDialog();
        }
    }
}
