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
        public string searchString;
        public UserType filterType;
        public UsersPage()
        {
            InitializeComponent();
            lvUsers.ItemsSource = usersList;
            var filters = new ObservableCollection<UserType>(Entities.GetContext().UserType.ToList());
            var type = new UserType();
            type.Id = 0;
            type.Title = "Любая роль";
            filters = new ObservableCollection<UserType>( filters.Prepend(type).ToList());
            FilterCB.ItemsSource = filters;
            GetUsers();
        }

        public void GetUsers() 
        {
            usersList.Clear();

            var usersGet = Entities.GetContext().User.ToList();

            if (!String.IsNullOrEmpty(searchString)) 
            {
                usersGet = usersGet.Where(p => 
                           p.FirstName.ToLowerInvariant().Contains(searchString.ToLowerInvariant())
                        || p.LastName.ToLowerInvariant().Contains(searchString.ToLowerInvariant())
                        || p.Patronymic.ToLowerInvariant().Contains(searchString.ToLowerInvariant())).ToList();
            }

            if (FilterCB.SelectedIndex != 0) 
            {
                usersGet = usersGet.Where( p => p.UserType.Id == ((UserType)FilterCB.SelectedItem).Id).ToList();
            }

            foreach(var user in usersGet) 
            {
                usersList.Add(user);
            }
        }

        private void addUserBTN_Click(object sender, RoutedEventArgs e)
        {
            AddUserWindow addUserWindow = new AddUserWindow();
            if (addUserWindow.ShowDialog() == true) GetUsers();
        }

        private void editUserBTN_Click(object sender, RoutedEventArgs e)
        {
            if (lvUsers.SelectedItem == null) return;
            AddUserWindow addUserWindow = new AddUserWindow(lvUsers.SelectedItem as User);
            if (addUserWindow.ShowDialog() == true) GetUsers();
        }

        private void SearchTB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            searchString = ((TextBox)sender).Text;
            GetUsers();

        }

        private void FilterCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GetUsers();
        }
    }
}
