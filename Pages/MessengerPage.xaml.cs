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
    /// Логика взаимодействия для MessengerPage.xaml
    /// </summary>
    public partial class MessengerPage : Page
    {
        public ObservableCollection<User> listUsers;
        public ObservableCollection<Student> listStudents;
        public ObservableCollection<Teacher> listTeachers;
        public User selectedUser = null;
        public ObservableCollection<Message> listMessages;

        public MessengerPage()
        {
            InitializeComponent();
            listUsers = new ObservableCollection<User>();
            listMessages = new ObservableCollection<Message>();
            lvMessages.ItemsSource = listMessages;
            lvUsers.ItemsSource = listUsers;
            GetUsers();
        }

        public void Update() 
        {
            GetMessages();
            
        }
        public void GetUsers() 
        {
            listUsers.Clear();
            if (MainWindow.currentUser.TypeId == 1) 
                foreach (var item in (GetTeachers()))
                {
                    listUsers.Add(item);
                }
            else if (MainWindow.currentUser.TypeId == 2)
                foreach (var item in (GetStudents()))
                {
                    listUsers.Add(item);
                }
            else if (MainWindow.currentUser.TypeId == 3)
            {
                foreach (var item in (GetTeachers()))
                {
                    listUsers.Add(item);
                }
                foreach (var item in (GetStudents())) 
                {
                    listUsers.Add(item);
                }
            }
        }

        public void GetMessages() 
        {
            listMessages.Clear();
            var list = new ObservableCollection<Message> (Entities.GetContext().Message
                            .Where(p => p.SenderId == MainWindow.currentUser.Id && p.RecieverId == selectedUser.Id 
                            || p.SenderId == selectedUser.Id && p.RecieverId == MainWindow.currentUser.Id)
                            .OrderBy(p => p.Date).ToList());
            foreach(var item in list) 
            {
                listMessages.Add(item);
            }
        }

        public ObservableCollection<User> GetStudents() 
        {
            return new ObservableCollection<User>( Entities.GetContext().User.Where( p => p.UserType.Id == 1).ToList());
        }

        public ObservableCollection<User> GetTeachers()
        {
            return new ObservableCollection<User>(Entities.GetContext().User.Where(p => p.UserType.Id == 2).ToList());
        }

        private void Click_btnSend(object sender, RoutedEventArgs e)
        {
            try
            {
                Message msg = new Message();

                msg.Text = tbMessage.Text;
                msg.Date = DateTime.Now;
                msg.SenderId = MainWindow.currentUser.Id;
                msg.IsReaded = false;
                msg.RecieverId = ((User)lvUsers.SelectedItem).Id;

                Entities.GetContext().Message.Add(msg);
                Entities.GetContext().SaveChanges();

                Update();
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SelectionChanged_lvUsers(object sender, SelectionChangedEventArgs e)
        {
            btnSend.IsEnabled = true;
            selectedUser = ((User)lvUsers.SelectedItem);
            Update();
        }
    }
}
