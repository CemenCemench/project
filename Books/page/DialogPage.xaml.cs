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

namespace Books.page
{
    /// <summary>
    /// Interaction logic for DialogPage.xaml
    /// </summary>
    public partial class DialogPage : Page
    {

        private Page Page;
        public Data.Books Books { get; set; }

        public Data.Copies copies { get; set; }
        public List<Data.Publishers> Publishers { get; set; }
        
        public DialogPage(Page page, Data.Books books, bool Copying)
        {
            InitializeComponent();
            this.Page = page;
            if (books == null) //создаём
            {
                labelNewBook.Content = "Новая книга";
                this.Books = new Data.Books();
                this.Books.BokId = Core.VOID;
                this.Books.PubId = Core.NONE;
            }
            else
            {
                if (Copying)//копируем
                {
                    labelNewBook.Content = "Новая книга на основе выбранной";
                    this.Books = Core.Databasse.Books.AsNoTracking().FirstOrDefault(B => B.BokId == books.BokId);
                    this.Books.BokId = Core.VOID;
                }
                else
                {
                    labelNewBook.Content = "Редактирование выбраной книги";
                    this.Books = books;


                }

            }
            LoadPublishers();
            DataContext = this;
        }


        private void LoadPublishers()
        {
            Publishers = new List<Data.Publishers>(Core.Databasse.Publishers.OrderBy(P => P.Name));
            PublishesComboBox.ItemsSource = Publishers;
            if(Publishers.Count > 0)
            {
                PublishesComboBox.SelectedItem = Core.Databasse.Publishers.SingleOrDefault(P => P.PubId == Books.PubId);
            }
        }
        private bool CheckInfo()
        {
            if(TextBoxname.Text == "")
            {
                MessageBox.Show("Не указано название книги", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.None);
                TextBoxname.Focus();
                return false;
            }
            if (TextBoxAnthors.Text == "")
            {
               if( MessageBox.Show("Не указан автор для книги.\n\n вы уверены, что  хотите оставить книгу без автора", "Поддтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.No)
                {
                    TextBoxname.Focus();
                    return false;
                }
            }
            if((PublishesComboBox.SelectedItem as Data.Publishers).PubId == Core.NONE)
            {
                MessageBox.Show("Невыбрано издание книги", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.None);
                PublishesComboBox.Focus();
                return false;
            }
            if (PublishYearTextBox.Text == "")
            {
                MessageBox.Show("Не указан год издания", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.None);
                PublishYearTextBox.Focus();
                return false;
            }

            String Name = TextBoxname.Text.ToLower();
            String Anthers = TextBoxAnthors.Text.ToLower();
            int Publishers = (PublishesComboBox.SelectedItem as Data.Publishers).PubId;
            int PublishYear;
            try { 
             PublishYear = int.Parse(PublishYearTextBox.Text);
                }
            catch
                {
                 PublishYear = 0;
                }
            Func<Data.Books, bool> Predicate; // тип данных для предиката есть водной и выходные параметры
            if(Books.BokId == Core.VOID)
            {
                Predicate = B => B.Name == Name && B.Anthors == Anthers && B.PubId == Publishers && B.PublishYear == PublishYear;
            }
            else
            {
                Predicate = B => B.Name == Name && B.Anthors == Anthers && B.PubId == Publishers && B.PublishYear == PublishYear && B.BokId != Books.BokId; 
            }

            int Count = Core.Databasse.Books.Where(Predicate).Count(); 
            if (Count > 0) 
            {
                MessageBox.Show("данная книга уже существует в списке", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.None);
                TextBoxname.Focus();
                return false;
            }

            return true;
        }

        private bool SaveInfo()
        {
            try 
            {
                if(Books.BokId == Core.VOID)
                {
                    Core.Databasse.Books.Add(Books);
                }
            Core.Databasse.SaveChanges();
            }
            catch
            {
                MessageBox.Show("не удалось сохранить изменения в базе данных", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.None);
                Core.CancelCheanges(Books);
                return false;
            }
            return true;
        }

        private void CloseDialog( bool NeedUpdet)
        {
            if (Page is BoolsPage )
            {
                BoolsPage BoolsPage = Page as BoolsPage;
                BoolsPage.HidenDialog();
                if (NeedUpdet)
                {
                    BoolsPage.UpdetetoBookDataGrid(Books);
                }
                else
                {
                    BoolsPage.BookDataGrid.Items.Refresh();
                }
                BoolsPage.BookDataGrid.Focus();
            }
            if (Page is CopiesPage)
            {
                CopiesPage CopiesPage = Page as CopiesPage;
                CopiesPage.HidenDialog();
                if (NeedUpdet)
                {
                    CopiesPage.UpdetetoBooksDataGrid(Books,copies);
                }
                else
                {
                    CopiesPage.BooksDataGrid.Items.Refresh();
                }
                CopiesPage.BooksDataGrid.Focus();
            }
        }

        private void ОКButton_Click(object sender, RoutedEventArgs e)
        {
            ОКButton.Focus();
            if (CheckInfo() && SaveInfo())
            {
                CloseDialog(true);
            }
        }

        
        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            cancelButton.Focus();
            if(Books.BokId != Core.VOID) 
            {
                Core.CancelCheanges(Books);
            }
            CloseDialog(false);

        }

        private void TextBoxname_LostFocus(object sender, RoutedEventArgs e)
        {
            (sender as TextBox).Text = TextBoxname.Text.Trim(' ', ',', '.', '!', '?');
        }
    }
}
