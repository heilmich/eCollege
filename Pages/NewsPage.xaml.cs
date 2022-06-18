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

namespace eCollege
{
    /// <summary>
    /// Логика взаимодействия для NewsPage.xaml
    /// </summary>
    public partial class NewsPage : Page
    {
        public ObservableCollection<News> newsList;
        public NewsPage()
        {
            InitializeComponent();
            GetNews();
            NewsLB.ItemsSource = newsList;
            if (MainWindow.currentUser.UserType.Id == 3) 
            {
                AddNewsBTN.Visibility = Visibility.Visible;
                EditNewsBTN.Visibility = Visibility.Visible;
            }
        }


        public ObservableCollection<News> GetNews() 
        {
            newsList = new ObservableCollection<News>(Entities.GetContext().News.OrderByDescending(p => p.Id).ToList());
            return newsList;
        }

        private void AddNews_Click(object sender, RoutedEventArgs e)
        {
            NewsItemWindow newsItemWindow = new NewsItemWindow();
            if (newsItemWindow.ShowDialog() == true) GetNews();
        }

        private void EditNews_Click(object sender, RoutedEventArgs e)
        {
            if (NewsLB.SelectedItem == null) return;
            NewsItemWindow newsItemWindow = new NewsItemWindow(NewsLB.SelectedItem as News);
            if (newsItemWindow.ShowDialog() == true) GetNews();
        }
    }
}
