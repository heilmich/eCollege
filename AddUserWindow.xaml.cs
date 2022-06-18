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
using System.Windows.Shapes;

namespace eCollege
{
    /// <summary>
    /// Логика взаимодействия для AddUserWindow.xaml
    /// </summary>
    public partial class AddUserWindow : Window
    {
        public User currentUser;
        public bool isUserCreated = false;
        public AddUserWindow()
        {
            InitializeComponent();
            SetCB();
            currentUser = new User();
            cbUserType.SelectedIndex = 0;
            cbGroup.SelectedIndex = 0;
            userGrid.DataContext = currentUser;
        }

        public AddUserWindow(User user)
        {
            InitializeComponent();
            SetCB();
            cbUserType.SelectedItem = user.UserType;
            if (user.UserType.Id == 1) 
            {
                cbGroup.SelectedItem = user.Student.Single().Group;
            }
            if (user.UserType.Id == 2)
            {
                cbGroup.SelectedItem = user.Teacher.Single().Group;
            }
            isUserCreated = true;
            currentUser = user;
            userGrid.DataContext = currentUser;
            this.Title = "Редактировать пользователя";
        }

        public void SetCB() 
        {
            cbUserType.ItemsSource = Entities.GetContext().UserType.ToList();
            cbGroup.ItemsSource = Entities.GetContext().Group.ToList();
        }



        private void SaveBTN_Click(object sender, RoutedEventArgs e)
        {
            if (Entities.GetContext().User.Where(p => p.Login == currentUser.Login && p.Id != currentUser.Id).Count() != 0)
            {
                MessageBox.Show("Пользователь с данным логином уже зарегистрирован");
                return;
            }

            var gender = cbGender.SelectedIndex;
            if (gender == 0) currentUser.Gender = "М";
            else if (gender == 1) currentUser.Gender = "Ж";

            currentUser.TypeId = (cbUserType.SelectedItem as UserType).Id;
            currentUser.Login = tbLogin.Text;
            currentUser.Password = tbPassword.Text;
            currentUser.LastName = tbLastName.Text;
            currentUser.FirstName = tbFirstName.Text;
            currentUser.Patronymic = tbPatronymic.Text;

            if (!isUserCreated)
            {
                currentUser.RegDate = DateTime.Now;
                currentUser.LoginDate = DateTime.Now;
                if (currentUser.TypeId == 1)
                {
                    Student newStudent = new Student();
                    newStudent.UserId = currentUser.Id;
                    newStudent.GroupId = (cbGroup.SelectedItem as Group).Id;
                    Entities.GetContext().Student.Add(newStudent);
                }
                else if (currentUser.TypeId == 2)
                {
                    Teacher newTeacher = new Teacher();
                    newTeacher.UserId = currentUser.Id;
                    (cbGroup.SelectedItem as Group).Curator = newTeacher.Id;
                    Entities.GetContext().Teacher.Add(newTeacher);
                }
                Entities.GetContext().User.Add(currentUser);
            }
            else 
            {
                if (currentUser.TypeId == 1)
                {
                    currentUser.Student.Single().GroupId = (cbGroup.SelectedItem as Group).Id;
                }
                else if (currentUser.TypeId == 2)
                {
                    (cbGroup.SelectedItem as Group).Curator = currentUser.Teacher.Single().Id;
                }
            }



            Entities.GetContext().SaveChanges();
            MessageBox.Show("Пользователь сохранен");
            this.DialogResult = true;
            this.Close();
        }

        private void cbUserType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((cbUserType.SelectedItem as UserType).Id == 1 || (cbUserType.SelectedItem as UserType).Id == 2)
            {
                cbGroup.Visibility = Visibility.Visible;
            }
            else cbGroup.Visibility = Visibility.Collapsed;
        }

        private void RemoveBTN_Click(object sender, RoutedEventArgs e)
        {
            if (currentUser.TypeId == 1) 
            {
                Entities.GetContext().Student.Remove(currentUser.Student.Single());
            }
            else if (currentUser.TypeId == 2)
            {
                Entities.GetContext().Teacher.Remove(currentUser.Teacher.Single());
            }
            Entities.GetContext().User.Remove(currentUser);
            Entities.GetContext().SaveChanges();
            this.DialogResult = true;
            MessageBox.Show("Пользователь удален");
            this.Close();
        }
    }
}
