using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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

namespace Books.page
{
    /// <summary>
    /// Interaction logic for RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {

        private string Capcha;
        private int CapchaLength = 5;
        public RegistrationWindow()
        {
            InitializeComponent();
            CreateCaptcha();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            AuthWindow window = new AuthWindow();
            window.Show();
            Close();
        }

        private void ButtonPassword_Click(object sender, RoutedEventArgs e)
        {
            bool PasswordVisible = PasswordBoxPassword.Visibility == Visibility.Hidden;
            if (PasswordVisible)
            {
                PasswordBoxPassword.Visibility = Visibility.Visible;
                PasswordBoxPassword.Width = PasswordTextBox.Width;
                PasswordBoxPassword.Password = PasswordTextBox.Text;
                PasswordTextBox.Visibility = Visibility.Hidden;
                PasswordTextBox.Width = 0;
                PasswordButton.Content = "Показать";
            }
            else
            {
                PasswordTextBox.Visibility = Visibility.Visible;
                PasswordTextBox.Width = PasswordBoxPassword.ActualWidth;
                PasswordTextBox.Text = PasswordBoxPassword.Password;
                PasswordBoxPassword.Visibility = Visibility.Hidden;
                PasswordBoxPassword.Width = 0;
                PasswordButton.Content = "Скрыть";
            }
        }

        private bool CaphaInfo()
        {
            if (CaptchaTextBox.Text != Capcha)
            {
                MessageBox.Show("Не правильно введена каптча", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                CreateCaptcha();
                return false;
            }
            return true;
        }

        private bool UniqueLogin(String Login)
        {
            Core.CurrentUsers = Core.Databasse.Users.SingleOrDefault(U => U.login == Login);
            if (Core.CurrentUsers == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool ValidatePassword(String Password)
        {
            const int MIN_LENGTH = 8;
            const int MAX_LENGTH = 15;

            if (Password == null) throw new ArgumentNullException();

            bool meetsLENGTHRequirements = Password.Length >= MIN_LENGTH && Password.Length <= MAX_LENGTH;
            bool hasUpperCaseLetter = false;
            bool hasLowerCaseLetter = false;
            bool hasDecimalDigit = false;

            if (meetsLENGTHRequirements)
            {
                foreach (char c in Password)
                {
                    if (char.IsUpper(c)) hasUpperCaseLetter = true;
                    else if (char.IsLower(c)) hasLowerCaseLetter = true;
                    else if (char.IsDigit(c)) hasDecimalDigit = true;
                }
            }
            bool isValid = meetsLENGTHRequirements && hasUpperCaseLetter && hasLowerCaseLetter && hasDecimalDigit;
            return isValid;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            // проыверитьь 4 вещи 
            if (!CaphaInfo()) return;
            try
            {
                if (ValidatePassword(PasswordBoxPassword.Password))
                {
                    if (UniqueLogin(LoginTextBox.Text) == true)
                    {
                        Data.Users users = new Data.Users();
                        users.UseId = Core.VOID;
                        users.login = LoginTextBox.Text;
                        users.password = PasswordBoxPassword.IsVisible ? PasswordBoxPassword.Password : PasswordTextBox.Text;
                        users.email = "";
                        users.RolId = 2;
                        Core.Databasse.Users.Add(users);
                        Core.Databasse.SaveChanges();
                        Close();
                    }
                    else
                    {
                        MessageBox.Show(
                       "Логин не является уникальным",
                       "Предупреждение",
                       MessageBoxButton.OK,
                       MessageBoxImage.Warning,
                       MessageBoxResult.OK
                       );
                    }
                }
                else
                {
                    MessageBox.Show(
                      "В пороле не достаёт 1-заглавной буквы 2 цифры 3-маленькой буквы 4-не менее 8 символов 5- не более 15 символов",
                      "Предупреждение",
                      MessageBoxButton.OK,
                      MessageBoxImage.Warning,
                      MessageBoxResult.OK
                      );
                }
            }
            catch
            {
                MessageBox.Show(
                   "не удалось сохранить информацию о пользователе",
                   "Предупреждение",
                   MessageBoxButton.OK,
                   MessageBoxImage.Warning,
                   MessageBoxResult.OK
                   );
            }


            AuthWindow window = new AuthWindow();
            window.Show();
            Close();
        }
        private void CreateCaptcha() //генерация каптчи
        {
            Random Generator = new Random();
            string Alphabet = "1234567890QWERTYUIOPASDFGHJKLZXCVBNM";
            Capcha = "";
            for (int i = 0; i < CapchaLength; i++)
            {
                Capcha += Alphabet[Generator.Next(Alphabet.Length)];
            }
            int CaptchaWidth = 200;
            int CaptchaHeight = 100;

            Bitmap Bitmap = new Bitmap(CaptchaWidth, CaptchaHeight);
            Graphics Graphics = Graphics.FromImage(Bitmap);
            Graphics.Clear(System.Drawing.Color.Beige);
            System.Drawing.Color[] Colors = { System.Drawing.Color.Black, System.Drawing.Color.Brown, System.Drawing.Color.Crimson, System.Drawing.Color.DarkBlue, System.Drawing.Color.DarkGoldenrod };
            int X = Generator.Next(CaptchaWidth / 5);
            int Y = Generator.Next(CaptchaHeight / 3);
            Graphics.DrawString(Capcha, new Font("Arial", 32), new SolidBrush(Colors[Generator.Next(Colors.Length)]), new System.Drawing.Point(X, Y));
            for (X = 0; X < CaptchaWidth; X++)
            {
                for (Y = 0; Y < CaptchaHeight; Y++)
                {
                    if (Generator.Next() % 5 == 0)
                    {
                        Bitmap.SetPixel(X, Y, Colors[Generator.Next(Colors.Length)]);
                    }
                }
            }
            for (int i = 0; i < 20; i++)
            {
                Graphics.DrawLine(new System.Drawing.Pen(Colors[Generator.Next(Colors.Length)]), Generator.Next(CaptchaWidth), Generator.Next(CaptchaHeight),
                                                                                                 Generator.Next(CaptchaWidth), Generator.Next(CaptchaHeight));
            }

            CaptchaImage.Source = BitmapToBitmapImage(Bitmap);
        }

        private BitmapImage BitmapToBitmapImage(Bitmap Bitmap)
        {
            MemoryStream Stream = new MemoryStream();
            Bitmap.Save(Stream, ImageFormat.Png);
            Stream.Position = 0;
            BitmapImage Image = new BitmapImage();
            Image.BeginInit();
            Image.StreamSource = Stream;
            Image.CacheOption = BitmapCacheOption.OnLoad;
            Image.EndInit();
            Image.Freeze();
            return Image;
        }

        private void ChangeCaptchaButton_Click(object sender, RoutedEventArgs e)
        {
            CreateCaptcha();
        }

        private void PronounceCaptchaButton_Click(object sender, RoutedEventArgs e)
        {
            System.Speech.Synthesis.SpeechSynthesizer speechSynthesizer = new System.Speech.Synthesis.SpeechSynthesizer();
            speechSynthesizer.Volume = 100;
            foreach (char c in Capcha)
            {
                speechSynthesizer.Speak(c.ToString());
            }
        }
    }
}
