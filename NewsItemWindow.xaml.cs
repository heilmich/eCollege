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

namespace eCollege
{
    /// <summary>
    /// Логика взаимодействия для NewsItemWindow.xaml
    /// </summary>
    public partial class NewsItemWindow : Window
    {
        public News newsItem;
        public bool isNewsCreated = false;
        public NewsItemWindow()
        {
            InitializeComponent();
            newsItem = new News();
            SetDataContext();
        }

        public NewsItemWindow(News newsItem)
        {
            InitializeComponent();
            this.newsItem = newsItem;
            SetDataContext();
            this.Title = "Редактировать новость";
            titleTB.Text = "Редактировать новость";
            isNewsCreated = true;
        }

        public void SetDataContext() 
        {
            NewsItemGrid.DataContext = newsItem;
        }

        private void SaveBTN_Click(object sender, RoutedEventArgs e)
        {
            if (isNewsCreated == false) 
            {
                newsItem.AuthorId = MainWindow.currentUser.Id;
                newsItem.Date = DateTime.Now;
                Entities.GetContext().News.Add(newsItem);
            }
            else 
            {
                newsItem.Date = DateTime.Now;
            }
            Entities.GetContext().SaveChangesAsync();
        }
    }
}
