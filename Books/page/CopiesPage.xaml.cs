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
    /// Interaction logic for CopiesPage.xaml
    /// </summary>
    public partial class CopiesPage : Page
    {
        public CopiesPage()
        {
            this.DataContext = this;
            InitializeComponent();
            BooksViewModel = new CollectionViewSource();
            BooksViewModel.Filter += FilterBooks;
            CopiesViewModel = new CollectionViewSource();
            CopiesViewModel.Filter += FilterCopies;
            UpdetetoBooksDataGrid(null, null);
        }

        private void FilterBooks(object sender, FilterEventArgs e)
        {
            Data.Books Book = e.Item as Data.Books;
            String Text = FilterBooksTextBox.Text.ToLower();
            e.Accepted = Book.Name.ToLower().Contains(Text) ||
                Book.Anthors.ToLower().Contains(Text) ||
                Book.Publishers.Name.ToLower().Contains(Text) ||
                Book.PublishYear.ToString().Contains(Text);
        }
        private void FilterCopies(object sender, FilterEventArgs e)
        {
            Data.Copies Copy = e.Item as Data.Copies;
            String Text = FilterBooksTextBoxSpliter.Text.ToLower();
            e.Accepted = Copy.Books.Name.ToLower().Contains(Text) ||
                Copy.InvNum.ToString().Contains(Text) ||
                Copy.PurchaseDate.ToString().Contains(Text) ||
                Copy.DecommDate.ToString().Contains(Text);
        }


        private void SetControlEnable()
        {
            bool DialogHidden = BookScrollViewer.Visibility == Visibility.Hidden;

            bool ItemSelected = BooksDataGrid.SelectedIndex >= 0;
            AddBookButton.IsEnabled = DialogHidden;
            CopyBookButton.IsEnabled = DialogHidden && ItemSelected;
            EditBookButton.IsEnabled = DialogHidden && ItemSelected;
            DeleteBookButton.IsEnabled = DialogHidden && ItemSelected;
            FindBooksButton.IsEnabled = DialogHidden;

            ItemSelected = CopisDataGridSpliter.SelectedIndex >= 0;
            AddBookButtonSpliter.IsEnabled = DialogHidden;
            CopyBookButtonSpliter.IsEnabled = DialogHidden && ItemSelected;
            EditBookButtonSpliter.IsEnabled = DialogHidden && ItemSelected;
            DeleteBookButtonSpliter.IsEnabled = DialogHidden && ItemSelected;
            FindBooksButtonSpliter.IsEnabled = DialogHidden;

            CloseButton.IsEnabled = DialogHidden;
            BooksDataGrid.IsEnabled = DialogHidden;
            CopisDataGridSpliter.IsEnabled = DialogHidden;
            Core.AppmainWindow.SetMenuEnabled(DialogHidden);
        }

        private CollectionViewSource BooksViewModel { get; set; }

        private CollectionViewSource CopiesViewModel { get; set; }



        public void UpdetetoCopiesDataGrid(Data.Copies copies)
        {
            if (copies == null && CopisDataGridSpliter.SelectedItem != null) // конструкция указвает там где была закончена работа со списком 
            {
                copies = CopisDataGridSpliter.SelectedItem as Data.Copies;
            }
            Data.Books Book = BooksDataGrid.SelectedItem as Data.Books;
            ObservableCollection<Data.Copies> Copies;
            if (Book != null)
            {
                Copies = new ObservableCollection<Data.Copies>(Core.Databasse.Copies.Where(C => C.BokId == Book.BokId));
            }
            else
            {
                Copies = new ObservableCollection<Data.Copies>();
            }
            CopiesViewModel.Source = Copies;
            CopisDataGridSpliter.ItemsSource = CopiesViewModel.View;

            if (Copies.Count > 0)
            {
                CopisDataGridSpliter.SelectedItem = copies;
                if (CopisDataGridSpliter.SelectedIndex < 0)
                {
                    CopisDataGridSpliter.SelectedIndex = 0;
                }
            }
            SetControlEnable();
        }

        public void UpdetetoBooksDataGrid(Data.Books Book, Data.Copies copies)
        {
            if (Book == null && BooksDataGrid.SelectedItem != null) // конструкция указвает там где была закончена работа со списком 
            {
                Book = BooksDataGrid.SelectedItem as Data.Books;
            }
            ObservableCollection<Data.Books> Books = new ObservableCollection<Data.Books>(Core.Databasse.Books.Where(B => B.BokId >= 0));
            BooksViewModel.Source = Books;
            BooksDataGrid.ItemsSource = BooksViewModel.View;

            if (Books.Count > 0)
            {
                BooksDataGrid.SelectedItem = Book;
                if (BooksDataGrid.SelectedIndex < 0)
                {
                    BooksDataGrid.SelectedIndex = 0;
                }
            }
            UpdetetoCopiesDataGrid(copies);
        }

        private void ShowDialog(Page page)
        {
            Grid.SetColumnSpan(BooksDataGrid, 1);
            BookScrollViewer.Visibility = Visibility.Visible;
            BookGridSplitter.Visibility = Visibility.Visible;
            BookFrame.Navigate(page);
            SetControlEnable();
        }

        public void HidenDialog()
        {
            Grid.SetColumnSpan(BooksDataGrid, 3);
            BookScrollViewer.Visibility = Visibility.Hidden;
            BookGridSplitter.Visibility = Visibility.Hidden;
            BookFrame.Navigate(null);
            while (BookFrame.CanGoBack)
                BookFrame.RemoveBackEntry();
            SetControlEnable();
        }
        private void ShowDialogCopies(Page page)
        {
            Grid.SetColumnSpan(CopisDataGridSpliter, 1);
            BookScrollViewer.Visibility = Visibility.Visible;
            BookGridSplitter.Visibility = Visibility.Visible;
            BookFrame.Navigate(page);
            SetControlEnable();
        }
        public void HidenDialogCopies()
        {
            Grid.SetColumnSpan(CopisDataGridSpliter, 3);
            BookScrollViewer.Visibility = Visibility.Hidden;
            BookGridSplitter.Visibility = Visibility.Hidden;
            BookFrame.Navigate(null);
            while (BookFrame.CanGoBack)
                BookFrame.RemoveBackEntry();
            SetControlEnable();
        }

        private void FindBooksButton_Click(object sender, RoutedEventArgs e)
        {
            if (FilterBooksDockPanel.Visibility == Visibility.Collapsed)
            {
                FilterBooksDockPanel.Visibility = Visibility.Visible;
                FindBooksButton.Content = "Закрыть";
            }
            else
            {
                FilterBooksDockPanel.Visibility = Visibility.Collapsed;
                FindBooksButton.Content = "Поиск";
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

        private void AddBookButton_Click(object sender, RoutedEventArgs e)
        {
            ShowDialog(new DialogPage(this, null, false));
        }

        private void CopyBookButton_Click(object sender, RoutedEventArgs e)
        {
            Data.Books books = BooksDataGrid.SelectedItem as Data.Books;
            ShowDialog(new DialogPage(this, books, true));
        }

        private void EditBookButton_Click(object sender, RoutedEventArgs e)
        {
            if (BooksDataGrid.SelectedIndex >= 0)//обязательно 
            {
                Data.Books books = BooksDataGrid.SelectedItem as Data.Books;
                ShowDialog(new DialogPage(this, books, false));
            }
        }

        private void DeleteBookButton_Click(object sender, RoutedEventArgs e)
        {
            Data.Books DeletingBook = BooksDataGrid.SelectedItem as Data.Books;
            int Count = Core.Databasse.Copies.Where(C => C.BokId == DeletingBook.BokId).Count();
            if(Count > 0)
            {
                MessageBox.Show("Не удалось удалить выбранную книгу так как не существует экеплров", "Информация", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None);
                return;
            }

            if (MessageBox.Show("Вы уверены что хотите удалить выбранную книгу", "поддтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.No)
            {
                return;
            }
          
            if (BooksDataGrid.SelectedIndex < BooksDataGrid.SelectedItems.Count - 1)
            {
                BooksDataGrid.SelectedIndex++;
            }
            else
            {
                if (BooksDataGrid.SelectedIndex > 0)
                {
                    BooksDataGrid.SelectedIndex--;
                }
            }
            Data.Books SelectingBooks = BooksDataGrid.SelectedItem as Data.Books;
            try
            {
                Core.Databasse.Books.Remove(DeletingBook);
                Core.Databasse.SaveChanges();
                UpdetetoBooksDataGrid(SelectingBooks, null);
            }
            catch
            {
                MessageBox.Show("Не удалось удалить выбранную книгу", "поддтверждение", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.No);
                Core.CancelAllCanges();
                UpdetetoBooksDataGrid(DeletingBook, null);

            }
            BooksDataGrid.Focus();
        }

        private void BooksDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdetetoCopiesDataGrid(null);
        }

        private void FilterBooksTextBoxSpliter_TextChanged(object sender, TextChangedEventArgs e)
        {
            CopiesViewModel.View.Refresh();
        }

        private void FindBooksButtonSpliter_Click(object sender, RoutedEventArgs e)
        {
            if (FilterBooksDockPanelSpliter.Visibility == Visibility.Collapsed)
            {
                FilterBooksDockPanelSpliter.Visibility = Visibility.Visible;
                FindBooksButtonSpliter.Content = "Закрыть";
            }
            else
            {
                FilterBooksDockPanelSpliter.Visibility = Visibility.Collapsed;
                FindBooksButtonSpliter.Content = "Поиск";
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
           Style Style = Core.GetStyle("activePageButtonSyle");
            if (Style != null) 
            {
                Core.AppmainWindow.CopiesButton.Style = Style;
            }
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            Style Style = Core.GetStyle("InactivePageButtonSyle");
            if (Style != null)
            {
                Core.AppmainWindow.CopiesButton.Style = Style;
            }
        }

        private void DeleteBookButtonSpliter_Click(object sender, RoutedEventArgs e)
        {

            Data.Copies DeletingCopies = CopisDataGridSpliter.SelectedItem as Data.Copies;
            int Count = Core.Databasse.Copies.Where(C => C.CopId == DeletingCopies.CopId).Count();
            if (Count > 0)
            {
                MessageBox.Show("Не удалось удалить выбранную книгу так как не существует экеплров", "Информация", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None);
                return;
            }

            if (MessageBox.Show("Вы уверены что хотите удалить выбранную книгу", "поддтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.No)
            {
                return;
            }

            if (CopisDataGridSpliter.SelectedIndex < CopisDataGridSpliter.SelectedItems.Count - 1)
            {
                CopisDataGridSpliter.SelectedIndex++;
            }
            else
            {
                if (CopisDataGridSpliter.SelectedIndex > 0)
                {
                    CopisDataGridSpliter.SelectedIndex--;
                }
            }
            Data.Copies SelectingCopies = CopisDataGridSpliter.SelectedItem as Data.Copies;
            try
            {
                Core.Databasse.Copies.Remove(DeletingCopies);
                Core.Databasse.SaveChanges();
                UpdetetoCopiesDataGrid(SelectingCopies);
            }
            catch
            {
                MessageBox.Show("Не удалось удалить выбранную книгу", "поддтверждение", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.No);
                Core.CancelAllCanges();
                UpdetetoCopiesDataGrid(DeletingCopies);

            }
            BooksDataGrid.Focus();
        }

        private void AddBookButtonSpliter_Click(object sender, RoutedEventArgs e)
        {
            ShowDialogCopies(new DialogPage(this, null, false));
        }
    }
}
