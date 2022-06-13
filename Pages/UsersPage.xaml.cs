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
    /// Логика взаимодействия для UsersPage.xaml
    /// </summary>
    public partial class UsersPage : Page
    {
        public ObservableCollection<User> usersList = new ObservableCollection<User>();
        public UsersPage()
        {
            InitializeComponent();
            lvUsers.ItemsSource = usersList;
            GetUsers();
        }

        public void GetUsers() 
        {
            usersList.Clear();

            var usersGet = Entities.GetContext().User.ToList();

            foreach(var user in usersGet) 
            {
                usersList.Add(user);
            }
        }

        private void addUserBTN_Click(object sender, RoutedEventArgs e)
        {
            if (lvUsers.SelectedItem == null) return;
            AddUserWindow addUserWindow = new AddUserWindow();
            addUserWindow.Show();
        }

        private void editUserBTN_Click(object sender, RoutedEventArgs e)
        {
            if (lvUsers.SelectedItem == null) return;
            AddUserWindow addUserWindow = new AddUserWindow(lvUsers.SelectedItem as User);
            addUserWindow.Show();
        }
    }
}
