using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

namespace Secret_santa.Pages
{
    /// <summary>
    /// Логика взаимодействия для Send_aim_to_player.xaml
    /// </summary>
    public partial class Send_aim_to_player : Window
    {
        public Send_aim_to_player()
        {
            InitializeComponent();
        }

        private void SendAimToPlayerButton_Click(object sender, RoutedEventArgs e)
        {
            if (NameTextBox.Text == String.Empty)
            {
                MessageBox.Show("Необходимо заполнить поле имя!");
                return;
            }

            if (File.ReadAllLines(@"..\..\Players.txt").Where(x => x.Trim().IndexOf(NameTextBox.Text) != -1).ToArray()[0] == String.Empty)
            {
                MessageBox.Show("Такого игрока не было найдено!");
                return;
            }

            string pair = File.ReadAllLines(@"..\..\Pairs_of_players.txt").Where(x => x.Trim().IndexOf($"{NameTextBox.Text}-") != -1).ToArray()[0];

            SendAMail(pair.Split('-')[0], pair.Split('-')[1]);

            MessageBox.Show($"Сообщение для игрока {NameTextBox.Text} было отправлено!");
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
                //message.Body = "<div style=\"color: red;\">Сообщение от System.Net.Mail</div>"; //тело сообщения
                message.Body = $"<div>Хо-хо-хо, привет {sender}!</div>";
                message.Body += "<div> </div>";
                message.Body += "<div>Это сообщение было сгенерировано специально для тебя, чтобы ты смог играть в тайного санту!</div>";
                message.Body += "<div> </div>";
                message.Body += "<h2 style=\"color: red;\">Твоя тайный друг:</h2>";
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

            this.Close();
        }
    }
}
