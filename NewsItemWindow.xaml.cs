using Microsoft.Win32;
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
        public NewsPhoto newsPhoto;
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
            PhotoLV.ItemsSource = newsItem.NewsPhoto;
            
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
            MessageBox.Show("Новость сохранена");
            this.Close();
        }

        private void AddPhotoBTN_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string picstr = Image.SerializeFromDialog();
                if (picstr == null) return;
                else 
                {
                    NewsPhoto newsPhoto = new NewsPhoto();
                    newsPhoto.Photo = picstr;
                    newsPhoto.NewsId = newsItem.Id;
                    Entities.GetContext().NewsPhoto.Add(newsPhoto);
                    Entities.GetContext().SaveChanges();

                    MessageBox.Show("Изображение изменено");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка! \nКод ошибки: " + ex.Message);
            }
        }

        private void NewsPhoto_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = PhotoLV.SelectedItem as NewsPhoto;
            if (item == null) return;
            if (!newsItem.NewsPhoto.Remove(item)) return;
            Entities.GetContext().SaveChanges();
            MessageBox.Show("Фото удалено");
                
            
        }

        private void RemovePhotoBTN_Click(object sender, RoutedEventArgs e)
        {
            var item = PhotoLV.SelectedItem as NewsPhoto;
            if (item == null) return;
            Entities.GetContext().NewsPhoto.Remove(item);
            Entities.GetContext().SaveChanges();
            //PhotoLV.Items.Remove(PhotoLV.SelectedItem);
            MessageBox.Show("Фото удалено");
        }
    }
}
