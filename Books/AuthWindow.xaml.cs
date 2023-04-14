using Books.page;
using System;
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
using System.Windows.Threading;

namespace Books
{
    /// <summary>
    /// Interaction logic for AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        private static int MaxAtemps = 3;

        private int _Attempts = MaxAtemps;

        public int Attempts
        {
            get
            {
                return _Attempts;
            }
            set
            {
                if (_Attempts != value)
                {
                    _Attempts = value;
                }
                AttemptsLable.Content = _Attempts;
                if (_Attempts > 0)
                {
                    AttemptsDockPanel.Visibility = Visibility.Visible;
                }
                else
                {
                    AttemptsDockPanel.Visibility = Visibility.Hidden;
                }
            }

        }
        public AuthWindow()
        {
            InitializeComponent();
            Timer = new DispatcherTimer();
            Timer.Interval = TimeSpan.FromSeconds(1);
            Timer.Tick += BlockingTimerTick;
        }

        private void BlockingTimerTick(object sender, EventArgs e)
        {
            BlockingTime = BlockingTime.Subtract(new TimeSpan(0, 0, 1));
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            string Login = LoginTextBox.Text;
            string Password = PasswordBoxPassword.Visibility == Visibility.Visible ?
                PasswordBoxPassword.Password : PasswordTextBox.Text;
            try { 
            Core.CurrentUsers = Core.Databasse.Users.SingleOrDefault
                (U => U.login == Login && U.password == Password );
            if(Core.CurrentUsers != null)
            {
                MainWindow window = new MainWindow();
                window.Show();
                Close();
            }
            else
            {
                MessageBox.Show(
                    "Введён неверный логин и/или пороль", 
                    "Предупреждение", 
                    MessageBoxButton.OK,
                    MessageBoxImage.Information, 
                    MessageBoxResult.OK
                    );
                    Attempts--;
                    if (Attempts == 0)
                    {
                        //Close();
                        BlockingTime = StandartBlockingTime;
                        Properties.Settings.Default.startsBlockingTime = DateTime.Now;
                        Properties.Settings.Default.Save();

                    }
                }
            }
            catch
            {
                MessageBox.Show(
                    "Не удалось подключиться к серверам баз данных",
                    "Предупреждение",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information,
                    MessageBoxResult.OK
                    );
            }
        }

        private DispatcherTimer Timer;

        private TimeSpan _BlockingTime;

        private static TimeSpan StandartBlockingTime = new TimeSpan(0, 1, 0);
        
        private TimeSpan BlockingTime
        {
            get
            {
                return _BlockingTime;
            }
            set
            {
                if(_BlockingTime != value)
                {
                    if(value.TotalSeconds < 0)
                    {
                        value = new TimeSpan(0, 0, 0);
                    }
                    _BlockingTime = value;
                    if(_BlockingTime.TotalSeconds > 0)
                    {
                        ShowBlockingTimeMessage();
                    }
                    else
                    {
                        HideBlockingTimeMessage();
                    }
                    TimerLabel.Content = _BlockingTime.ToString("hh':'mm':'ss");
                }
            }
        }

        private void ShowBlockingTimeMessage()
        {
            TimerDockPanel.Visibility = Visibility.Visible;
            Height = 200;
            OkButton.IsEnabled = false;
            Timer.Start();

        }

        private void HideBlockingTimeMessage()
        {
            TimerDockPanel.Visibility = Visibility.Hidden;
            Height = 180;
            OkButton.IsEnabled = true;
            Timer.Stop();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void PasswordButton_Click(object sender, RoutedEventArgs e)
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

        private class DispesherTimer
        {
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BlockingTime = StandartBlockingTime.Subtract(
            BlockingTime = DateTime.Now - Properties.Settings.Default.startsBlockingTime);

        }

        private void RegistationButton_Click(object sender, RoutedEventArgs e)
        {
            RegistrationWindow window = new RegistrationWindow();
                window.Show();
                Close();
        }
    }
}
