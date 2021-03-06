using System;
using System.ComponentModel;
using System.Globalization;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
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
using System.Diagnostics;
using System.Windows.Media.Effects;
using System.Windows.Media.Animation;

namespace eCollege
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        public static Entities db = new Entities();

        public static List<Mark> marksList = new List<Mark>();
        public static List<Lesson> lessonsList = new List<Lesson>();
        public static List<Subject> subjectsList = new List<Subject>();
        public static ObservableCollection<SubjectMark> subjectMarks = new ObservableCollection<SubjectMark>();

        public static MessengerPage messengerPage;
        public static ProfilePage profilePage;
        public static ShedulePage shedulePage;
        public static MarksPage marksPage;
        public static TeacherMarksPage teacherMarksPage;
        public static NewsPage newsPage;
        public static UsersPage usersPage;


        public static User currentUser;
        public static Student currentStudent;
        public static Teacher currentTeacher;

        public MainWindow(User user)
        {
            currentUser = db.User.Find(user.Id);
            currentUser.LoginDate = DateTime.Now;
            db.SaveChanges();

            GetData();

            profilePage = new ProfilePage();
            shedulePage = new ShedulePage();
            messengerPage = new MessengerPage();

            InitializeComponent();

            if (currentUser.TypeId == 3)
            {
                tbMarks.Visibility = Visibility.Collapsed;
                tbUsers.Visibility = Visibility.Visible;
            }

            NavigateProfile();

        }


        public void NavigateProfile()
        {
            GetData();
            mainFrame.Navigate(profilePage);
        }

        public void GetData()
        {
            if (currentUser.TypeId == 1) GetDataStudent();
            else if (currentUser.TypeId == 2) GetDataTeacher();
        }

        public static void UpdateUser(int userId)
        {
            currentUser = db.User.Find(userId);
        }
        public static void GetDataTeacher()
        {
            currentTeacher = currentUser.Teacher.First();
            lessonsList = currentTeacher.Lesson.ToList();
            teacherMarksPage = new TeacherMarksPage();
        }

        public static void GetDataStudent()
        {
            currentStudent = currentUser.Student.First();
            lessonsList = currentStudent.Group.Lesson.ToList();
            marksList = currentStudent.Mark.ToList();
            subjectsList = currentStudent.Group.Lesson.Select(p => p.Subject).Distinct().ToList();
            marksPage = new MarksPage();
        }


        private void Click_Profile(object sender, MouseButtonEventArgs e)
        {
            NavigateProfile();
        }

        private void Click_Shedule(object sender, MouseButtonEventArgs e)
        {
            mainFrame.Navigate(shedulePage);
        }

        private void Click_Marks(object sender, MouseButtonEventArgs e)
        {
            if (currentUser.TypeId == 1)
                mainFrame.Navigate(marksPage);
            else if (currentUser.TypeId == 2)
                mainFrame.Navigate(teacherMarksPage);
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/heilmich/eCollege");
        }

        private void Click_Message(object sender, MouseButtonEventArgs e)
        {
            mainFrame.Navigate(messengerPage);
        }

        private void Click_News(object sender, MouseButtonEventArgs e)
        {
            newsPage = new NewsPage();
            mainFrame.Navigate(newsPage);
        }
        private void Click_Users(object sender, MouseButtonEventArgs e)
        {
            usersPage = new UsersPage();
            mainFrame.Navigate(usersPage);
        }
    }
}
