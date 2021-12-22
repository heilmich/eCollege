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

    public class MarkConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            int mark;
            Lesson lesson = (Lesson)value;
            using (Entities db = new Entities()) 
            {
                
            }
                return value;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            return DependencyProperty.UnsetValue;
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

        public List<Lesson> lessonsList = new List<Lesson>();
        public List<Mark> marksList = new List<Mark>();

        public ObservableCollection<Day> dayList = new ObservableCollection<Day>();
        public int lessonsPerDay = 8;
        public DateTime startDay = DateTime.Now;
        public DateTime endDay = DateTime.Now.AddDays(7);


        public User currentUser;
        public Student currentStudent;
        public Teacher currentTeacher;

        public MainWindow(User user)
        {
            
            currentUser = db.User.Find(user.Id);
            InitializeComponent();
            SwitchUser();
        }

        public void SwitchUser()
        {
            switch (currentUser.TypeId)
            {
                case 1:
                    currentStudent = currentUser.Student;
                    lkGrid.DataContext = currentStudent;
                    UpdateShedule(currentStudent);
                    
                    break;
                case 2:
                    currentTeacher = currentUser.Teacher;
                    lkGrid.DataContext = currentTeacher;
                    break;
            }
        }


        public void UpdateShedule(Student student) 
        {
            dayList.Clear();
            lessonsList = student.Group.Lesson.ToList();
            marksList = student.Mark.ToList();
            
            for(DateTime i = startDay; i.DayOfYear < endDay.DayOfYear; i = i.AddDays(1)) 
            {
                Day day = new Day();
                ObservableCollection<Lesson> list = new ObservableCollection<Lesson>(lessonsList.Where(p => p.Date.DayOfYear == i.DayOfYear));
                ObservableCollection<Mark> listMark = new ObservableCollection<Mark>();
                foreach (var item in list) 
                {
                    listMark.Add(marksList.Where(p => p.LessonId == item.Id).FirstOrDefault());
                }


                
                day.MonthDay = Convert.ToString(i.Day);
                day.Marks = listMark;
                day.Lessons = list;
                dayList.Add(day);
            }

            mainShedule.ItemsSource = dayList;
        }

        


    }
}
