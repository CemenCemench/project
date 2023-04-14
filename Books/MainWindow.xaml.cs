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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Books
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private List<Page> ActivPage;

        private int CurentPageIndex;

        public MainWindow()
        {
            InitializeComponent();
            Core.AppmainWindow = this;
            ActivPage = new List<Page>();
            CurentPageIndex = -1;
        }

        public void SetMenuEnabled(bool DialogHidden)
        {
            ReadersButton.IsEnabled = false;
            BooksButton.IsEnabled = DialogHidden;
            CopiesButton.IsEnabled = DialogHidden;
            BooksButton.IsEnabled = DialogHidden;


            nextButton.IsEnabled = DialogHidden && CurentPageIndex < ActivPage.Count - 1;
            BackButton.IsEnabled = DialogHidden && CurentPageIndex > 0;
            clouseButton.IsEnabled = DialogHidden && ActivPage.Count > 0;
            clouseAllButton.IsEnabled = DialogHidden && ActivPage.Count > 0;
        }

        private int GetActivPageIndexByType(Type PageType) 
        {

            int index = ActivPage.Count -1;
            while (index >=0 && ActivPage[index].GetType() != PageType)  // начинаем с конца до начала и посиковый метод со 2 курса 
            {
                index--;
            }



            return index;
        }

        //требуеться для отобраежения окна
        private void ShowPage(Type PageType)
        {
            Page Page;
            if(PageType != null)
            {
                CurentPageIndex = GetActivPageIndexByType(PageType);
                if (CurentPageIndex < 0) //Проверка на существование страницы
                {
                    Page = (Page)Activator.CreateInstance(PageType);  // Создание страницы
                    ActivPage.Add(Page); // добавление в списов
                    CurentPageIndex = ActivPage.Count - 1;
                }
                else
                {
                    Page = ActivPage[CurentPageIndex];
                }     
                RootFrame.Navigate(Page);
            }
        }

        public void ClosePage()
        {
            if(ActivPage.Count > 0)
            {
                ActivPage.RemoveAt(CurentPageIndex);
                if(CurentPageIndex > 0)
                {
                    CurentPageIndex--;
                }
                else
                {
                    if(CurentPageIndex >= ActivPage.Count)
                    {
                        CurentPageIndex--;
                    }
                }
                if(CurentPageIndex >= 0)
                {
                    RootFrame.Navigate(ActivPage[CurentPageIndex]);
                }
                else
                {
                    RootFrame.Navigate(null);
                }
            }
        }

        private void ClouseAllPages()
        {
            ActivPage.RemoveAll(p => true);
            CurentPageIndex = -1;
            RootFrame.Navigate(null);
        }



        private void BooksButton_Click(object sender, RoutedEventArgs e)
        {
            ShowPage(typeof(page.BoolsPage));
        }

        private void CopiesButton_Click(object sender, RoutedEventArgs e)
        {
            ShowPage(typeof(page.CopiesPage));
        }

        private void RootFrame_LoadCompleted(object sender, NavigationEventArgs e)
        {
            
            SetMenuEnabled(true);
            RemovePageFromFrame();
        }

        public void RemovePageFromFrame()
        {
            while (RootFrame.CanGoBack)
            {
                RootFrame.RemoveBackEntry();
            }
        }

        private void clouseButton_Click(object sender, RoutedEventArgs e)
        {
            ClosePage();
        }

        private void clouseAllButton_Click(object sender, RoutedEventArgs e)
        {
            ClouseAllPages();
        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            if(CurentPageIndex < ActivPage.Count - 1)
            {
                CurentPageIndex++;
            }
            RootFrame.Navigate(ActivPage[CurentPageIndex]);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurentPageIndex > 0)
            {
                CurentPageIndex--;
            }
            RootFrame.Navigate(ActivPage[CurentPageIndex]);
        }
        private void SetMenuEnabled(object sender, RoutedEventArgs e)
        {
            SetMenuEnabled(true);
        }

        private void PublishersButton_Click(object sender, RoutedEventArgs e)
        {
            ShowPage(typeof(page.Publishers));
        }
    }
}
