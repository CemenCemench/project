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
    /// Interaction logic for Publishers.xaml
    /// </summary>
    public partial class Publishers : Page
    {
        public Publishers()
        {
            this.DataContext = this;
            InitializeComponent();
            PublishersViewModel = new CollectionViewSource();
            PublishersViewModel.Filter += FilterPublishers;
            UpdetetoBookDataGrid(null);
        }

        private CollectionViewSource PublishersViewModel { get; set; }

        private void FilterPublishers(object sender, FilterEventArgs e)
        {
            Data.Publishers Publisher = e.Item as Data.Publishers;
            String Text = FilterTextBoxPublichers.Text.ToLower();
            e.Accepted = Publisher.Name.ToLower().Contains(Text) ||
                Publisher.Addres.ToLower().Contains(Text);
        }

        private void SetControlEnable()
        {
            bool ItemSelected = PublichersDataGrid.SelectedIndex >= 0;
            AddButtonPublichers.IsEnabled = true;
            CopyButtonPublichers.IsEnabled = ItemSelected;
            CheangeButtonPublichers.IsEnabled = ItemSelected;
            DeleteButtonPublichers.IsEnabled = ItemSelected;
            SearchButtonPublichers.IsEnabled = true;
        }

        private void UpdetetoBookDataGrid(Data.Publishers Publisher)
        {
            if (Publisher == null && PublichersDataGrid.SelectedItem != null) // конструкция указвает там где была закончена работа со списком 
            {
                Publisher = PublichersDataGrid.SelectedItem as Data.Publishers;
            }
            ObservableCollection<Data.Publishers> Publishers = new ObservableCollection<Data.Publishers>(Core.Databasse.Publishers.Where(p => p.PubId >= 0));
            PublishersViewModel.Source = Publishers;
            PublichersDataGrid.ItemsSource = PublishersViewModel.View;

            if (Publishers.Count > 0)
            {
                PublichersDataGrid.SelectedItem = Publisher;
                if (PublichersDataGrid.SelectedIndex < 0)
                {
                    PublichersDataGrid.SelectedIndex = 0;
                }
            }
            SetControlEnable();
        }

        private void AddButtonPublichers_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (SearchDockPanelPublichers.Visibility == Visibility.Collapsed)
            {
                SearchDockPanelPublichers.Visibility = Visibility.Visible;
                SearchButtonPublichers.Content = "Закрыть";
            }
            else
            {
                SearchDockPanelPublichers.Visibility = Visibility.Collapsed;
                SearchButtonPublichers.Content = "Поиск";
            }
        }

        private void FilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            PublishersViewModel.View.Refresh();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Core.AppmainWindow.ClosePage();
        }

        private void DeleteButtonPublichers_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
