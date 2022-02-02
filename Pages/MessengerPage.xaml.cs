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
        public ObservableCollection<Student> listStudents;
        public ObservableCollection<Teacher> listTeachers;
        public User selectedUser = null;

        public MessengerPage()
        {
            InitializeComponent();
            GetUsers();
        }

        public void Update() 
        {
            lvMessages.ItemsSource = GetMessages(); 
            
        }
        public void GetUsers() 
        {
            if (MainWindow.currentUser.TypeId == 1) lvUsers.ItemsSource = GetTeachers();
            else                                    lvUsers.ItemsSource = GetStudents();
        }

        public List<Message> GetMessages() 
        {
            return (Entities.GetContext().Message
                            .Where(p => p.SenderId == MainWindow.currentUser.Id && p.RecieverId == selectedUser.Id 
                            || p.SenderId == selectedUser.Id && p.RecieverId == MainWindow.currentUser.Id)
                            .OrderBy(p => p.Date).ToList());
        }

        public ObservableCollection<Student> GetStudents() 
        {
            return new ObservableCollection<Student>( Entities.GetContext().Student);
        }

        public ObservableCollection<Teacher> GetTeachers()
        {
            return new ObservableCollection<Teacher>(Entities.GetContext().Teacher);
        }

        private void Click_btnSend(object sender, RoutedEventArgs e)
        {

            Message msg = new Message();

            msg.Text = tbMessage.Text;
            msg.Date = DateTime.Now;
            msg.SenderId = MainWindow.currentUser.Id;
            msg.IsReaded = false;
            if (MainWindow.currentUser.TypeId == 1) msg.RecieverId = ((Teacher)lvUsers.SelectedItem).User.Id;
            else                                    msg.RecieverId = ((Student)lvUsers.SelectedItem).User.Id;

            Entities.GetContext().Message.Add(msg);
            Entities.GetContext().SaveChanges();

            Update();
                
        }

        private void SelectionChanged_lvUsers(object sender, SelectionChangedEventArgs e)
        {
            if (MainWindow.currentUser.TypeId == 1)
            selectedUser = ((Teacher)lvUsers.SelectedItem).User;
            else selectedUser = ((Student)lvUsers.SelectedItem).User;
            Update();
        }
    }
}
