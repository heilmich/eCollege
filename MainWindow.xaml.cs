using System;
using System.ComponentModel; // добавил
using System.Globalization; // добавил
using System.Collections.Generic;
using System.Collections.ObjectModel; // добавил
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

namespace eCollege
{
    public class ImageConverter : IValueConverter 
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo) 
        {
            return value;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    public class AllMarksConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            string str = "";
            foreach(var mark in (ObservableCollection<Mark>)value) 
            {
                str += $" {mark.Mark1}";
            }
            return str;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    public class AvgMarksConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            if (((ObservableCollection<Mark>)value).Count != 0)
            {
                return ((ObservableCollection<Mark>)value).Average(p => p.Mark1);
            }
            else return null;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    public class FinalMarksConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            if (((ObservableCollection<Mark>)value).Count == 0)
            {
                return null;
            }
            var avg = ((ObservableCollection<Mark>)value).Average(p => p.Mark1);
            if (avg >= 0 && avg < 2.5) return 2;
            else if (avg < 3.5) return 3;
            else if (avg < 4.5) return 4;
            else if (avg <= 5) return 5;
            return null;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    public class SubjectMark : INotifyPropertyChanged
    {
        private string subjectTitle;
        private ObservableCollection<Mark> marks;


        public string SubjectTitle
        {
            get { return subjectTitle; }
            set
            {
                subjectTitle = value;
                OnPropertyChanged("SubjectTitle");
            }
        }

        public ObservableCollection<Mark> Marks
        {
            get { return marks; }
            set
            {
                marks = value;
                OnPropertyChanged("Marks");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }


    public class Day : INotifyPropertyChanged
    {
        private string monthDay;
        private DateTime date;
        private ObservableCollection<Lesson> lessons;
        private ObservableCollection<Mark> marks;

        public string MonthDay
        {
            get { return monthDay; }
            set
            {
                monthDay = value;
                OnPropertyChanged("MonthDay");
            }
        }

        public DateTime Date
        {
            get { return date; }
            set
            {
                date = value;
                OnPropertyChanged("Date");
            }
        }

        public ObservableCollection<Lesson> Lessons
        {
            get { return lessons; }
            set
            {
                lessons = value;
                OnPropertyChanged("Lessons");
            }
        }

        public ObservableCollection<Mark> Marks
        {
            get { return marks; }
            set
            {
                marks = value;
                OnPropertyChanged("Marks");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
    
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

        public static StudentProfilePage studentProfilePage;
        public static TeacherProfilePage teacherProfilePage;
        public static ShedulePage shedulePage;
        public static MarksPage marksPage;


        public static User currentUser;
        public static Student currentStudent;
        public static Teacher currentTeacher;

        public MainWindow(User user)
        {
            currentUser = db.User.Find(user.Id);
            SwitchUser();
            InitializeComponent();
            NavigateProfile();
        }

        public void SwitchUser()
        {
            switch (currentUser.TypeId)
            {
                case 1:
                    GetDataStudent();
                    studentProfilePage = new StudentProfilePage();
                    shedulePage = new ShedulePage();
                    //tbShedule.Visibility = Visibility.Visible;
                    marksPage = new MarksPage();
                    //tbMarks.Visibility = Visibility.Visible;

                    break;
                case 2:
                    currentTeacher = currentUser.Teacher;
                    teacherProfilePage = new TeacherProfilePage();
                    break;
            }
        }

        public void NavigateProfile() 
        {
            GetDataStudent();
            switch (currentUser.TypeId)
            {
                case 1:
                    mainFrame.Navigate(studentProfilePage);
                    break;
                case 2:
                    mainFrame.Navigate(teacherProfilePage);
                    break;
            }
        }

        
        public void GetDataStudent() 
        {
            currentStudent = db.Student.Find(currentUser.StudentId);
            lessonsList = currentStudent.Group.Lesson.ToList();
            marksList = currentStudent.Mark.ToList();
            subjectsList = currentStudent.Group.Lesson.Select(p => p.Subject).Distinct().ToList();
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
            mainFrame.Navigate(marksPage);
        }
    }
}
