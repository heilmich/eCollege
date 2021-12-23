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

        public List<Lesson> lessonsList = new List<Lesson>();
        public List<Mark> marksList = new List<Mark>();
        public List<Subject> subjectsList = new List<Subject>();

        public ObservableCollection<SubjectMark> subjectMarks = new ObservableCollection<SubjectMark>();

        public ObservableCollection<Day> dayList = new ObservableCollection<Day>();
        public int lessonsPerDay = 8;
        public DateTime startDay = DateTime.Now.AddDays(-1);
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
                    currentStudent = db.Student.Find(currentUser.StudentId);
                    lkGrid.DataContext = currentStudent;
                    finalMarks.Visibility = Visibility.Visible;
                    GetDataStudent(currentStudent);
                    UpdateSheduleAsync(currentStudent);
                    UpdateFinalMarksAsync(currentStudent);

                    break;
                case 2:
                    currentTeacher = currentUser.Teacher;
                    lkGrid.DataContext = currentTeacher;
                    break;
            }
        }

        public async void UpdateSheduleAsync(Student student) 
        {
            await Task.Run(() => UpdateShedule(student));
 
            mainSheduleGrid.ItemsSource = dayList;
        }
        
        public void GetDataStudent(Student student) 
        {
            lessonsList = student.Group.Lesson.ToList();
            marksList = student.Mark.ToList();
            subjectsList = student.Group.Lesson.Select(p => p.Subject).Distinct().ToList();
        }
        public void UpdateShedule(Student student) 
        {
            dayList.Clear();
            
            
            for(DateTime i = startDay; i.DayOfYear < endDay.DayOfYear; i = i.AddDays(1)) 
            {
                Day day = new Day();

                ObservableCollection<Lesson> list = new ObservableCollection<Lesson>(lessonsList.Where(p => p.Date.DayOfYear == i.DayOfYear));

                ObservableCollection<Mark> listMark = new ObservableCollection<Mark>();
                foreach (var item in list) 
                {
                    var it = marksList.Where(p => p.LessonId == item.Id).FirstOrDefault();
                    if (it == null) it = new Mark();
                    listMark.Add(it);
                }

                day.MonthDay = Convert.ToString(i.Day);
                day.Marks = listMark;
                day.Lessons = list;
                dayList.Add(day);
            }
        }

        public async void UpdateFinalMarksAsync(Student student)
        {
            await Task.Run(() => UpdateFinalMarks(student));
            
            finalMarksGrid.ItemsSource = subjectMarks;
        }
        public void UpdateFinalMarks(Student student) 
        {
            subjectMarks.Clear();

            foreach(var item in subjectsList) 
            {
                SubjectMark sm = new SubjectMark();
                sm.SubjectTitle = item.Title;
                sm.Marks = new ObservableCollection<Mark>(currentStudent.Mark.Where( p => p.Lesson.Subject == item));
                subjectMarks.Add(sm);
            }
        }
    }
}
