using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Books.page
{
    /// <summary>
    /// Interaction logic for BoolsPage.xaml
    /// </summary>
    public partial class BoolsPage : Page
    {
        public BoolsPage()
        {
            this.DataContext = this;
            InitializeComponent();
            BooksViewModel = new CollectionViewSource();
            BooksViewModel.Filter += FilterBooks;
            UpdetetoBookDataGrid(null);
        }



        private void FilterBooks(object sender, FilterEventArgs e)
        {
            Data.Books Book = e.Item as Data.Books;
            String Text = FilterTextBox.Text.ToLower();
            e.Accepted = Book.Name.ToLower().Contains(Text) ||
                Book.Anthors.ToLower().Contains(Text)||
                Book.Publishers.Name.ToLower().Contains(Text)||
                Book.PublishYear.ToString().Contains(Text); 
        }

      

        private void SetControlEnable()
        {
            bool DialogHidden = BookScrollViewer.Visibility == Visibility.Hidden;
            bool ItemSelected = BookDataGrid.SelectedIndex >= 0;
            AddButton.IsEnabled = DialogHidden;
            CopyButton.IsEnabled = DialogHidden && ItemSelected;
            CheangeButton.IsEnabled = DialogHidden && ItemSelected;
            DeleteButton.IsEnabled = DialogHidden && ItemSelected;
            SearchButton.IsEnabled = DialogHidden;
            CloseButton.IsEnabled = DialogHidden;
            BookDataGrid.IsEnabled = DialogHidden;

            Core.AppmainWindow.SetMenuEnabled(DialogHidden);
        }

        private CollectionViewSource BooksViewModel { get; set; }

        private void SetConbtrolsEnable()
        {

        }

        public void UpdetetoBookDataGrid(Data.Books Book)
        {
            if (Book == null && BookDataGrid.SelectedItem != null) // конструкция указвает там где была закончена работа со списком 
            {
                Book = BookDataGrid.SelectedItem as Data.Books;
            }
            ObservableCollection<Data.Books> Books = new ObservableCollection<Data.Books>(Core.Databasse.Books.Where(B => B.BokId >= 0));
            BooksViewModel.Source = Books;
            BookDataGrid.ItemsSource = BooksViewModel.View;

            if (Books.Count > 0)
            {
                BookDataGrid.SelectedItem = Book;
                if (BookDataGrid.SelectedIndex < 0)
                {
                    BookDataGrid.SelectedIndex = 0;
                }
            }
            SetControlEnable();
        }

        private void ShowDialog(Page page)
        {
            Grid.SetColumnSpan(BookDataGrid, 1);
            BookScrollViewer.Visibility = Visibility.Visible;
            BookGridSplitter.Visibility = Visibility.Visible;
            BookFrame.Navigate(page);
            SetConbtrolsEnable();
        }

        public void HidenDialog()
        {
            Grid.SetColumnSpan(BookDataGrid, 3);
            BookScrollViewer.Visibility = Visibility.Hidden;
            BookGridSplitter.Visibility = Visibility.Hidden;
            BookFrame.Navigate(null);
            while (BookFrame.CanGoBack)
                BookFrame.RemoveBackEntry();
            SetConbtrolsEnable();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (SearchDockPanel.Visibility == Visibility.Collapsed)
            {
                SearchDockPanel.Visibility = Visibility.Visible;
                SearchButton.Content = "Закрыть";
            }
            else
            {
                SearchDockPanel.Visibility = Visibility.Collapsed;
                SearchButton.Content = "Поиск";
            }
        }

        private void FilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            BooksViewModel.View.Refresh();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Core.AppmainWindow.ClosePage();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            ShowDialog(new DialogPage(this, null, false));
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            Data.Books books = BookDataGrid.SelectedItem as Data.Books;
            ShowDialog(new DialogPage(this, books, true));
        }

        private void CheangeButton_Click(object sender, RoutedEventArgs e)
        {
            if(BookDataGrid.SelectedIndex >= 0)//обязательно 
            {
                Data.Books books = BookDataGrid.SelectedItem as Data.Books;
                ShowDialog(new DialogPage(this, books, false));
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены что хотите удалить выбранную книгу", "поддтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) ==MessageBoxResult.No)
            {
                return ;
            }
            Data.Books DeletingBook = BookDataGrid.SelectedItem as Data.Books;
            if(BookDataGrid.SelectedIndex < BookDataGrid.SelectedItems.Count -1)
            {
                BookDataGrid.SelectedIndex++;
            }
            else
            {
                if(BookDataGrid.SelectedIndex > 0)
                {
                    BookDataGrid.SelectedIndex--;
                }
            }
            Data.Books SelectingBooks = BookDataGrid.SelectedItem as Data.Books;
            try { 
            Core.Databasse.Books.Remove(DeletingBook);
            Core.Databasse.SaveChanges();
                UpdetetoBookDataGrid(SelectingBooks);
            }
            catch
            {
                MessageBox.Show("Не удалось удалить выбранную книгу", "поддтверждение", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.No);
                Core.CancelAllCanges();
                UpdetetoBookDataGrid(DeletingBook);

            }
            BookDataGrid.Focus();
        }
    }
}
