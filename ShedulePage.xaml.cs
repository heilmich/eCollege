using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Логика взаимодействия для ShedulePage.xaml
    /// </summary>
    public partial class ShedulePage : Page
    {
        public ObservableCollection<Day> dayList = new ObservableCollection<Day>();
        public int lessonsPerDay = 8;
        public DateTime startDay = DateTime.Today;
        public DateTime endDay;
        public static bool isKeyDown = false;
        public ShedulePage()
        {
            InitializeComponent();
            SetWeek();
            UpdateSheduleAsync();
        }


        public void SetWeek() 
        {
            while(startDay.DayOfWeek != DayOfWeek.Monday) 
            {
                startDay = startDay.AddDays(-1);
            }
            endDay = startDay.AddDays(7);
            rStartDay.Text = startDay.ToShortDateString();
            rEndDay.Text = endDay.Date.ToShortDateString();

        }

        public void GetDataStudent(Student student)
        {
            MainWindow.lessonsList = student.Group.Lesson.ToList();
            MainWindow.marksList = student.Mark.ToList();
            MainWindow.subjectsList = student.Group.Lesson.Select(p => p.Subject).Distinct().ToList();
        }

        public async void UpdateSheduleAsync()
        {
            await Task.Run(() => UpdateShedule());

            mainSheduleGrid.ItemsSource = dayList;
        }

        public void UpdateShedule()
        {
            dayList.Clear();


            for (DateTime i = startDay; i.DayOfYear < endDay.DayOfYear; i = i.AddDays(1))
            {
                Day day = new Day();

                ObservableCollection<Lesson> list = new ObservableCollection<Lesson>(MainWindow.lessonsList.Where(p => p.Date.DayOfYear == i.DayOfYear));

                ObservableCollection<Mark> listMark = new ObservableCollection<Mark>();
                foreach (var item in list)
                {
                    var it = MainWindow.marksList.Where(p => p.LessonId == item.Id).FirstOrDefault();
                    if (it == null) it = new Mark();
                    listMark.Add(it);
                }

                day.MonthDay = Convert.ToString(i.Day);
                day.Marks = listMark;
                day.Lessons = list;
                dayList.Add(day);
            }
        }

        private void Click_PreviousWeek(object sender, MouseButtonEventArgs e)
        {
            startDay = startDay.AddDays(-7);
            SetWeek();
            UpdateShedule();
        }

        private void Click_NextWeek(object sender, MouseButtonEventArgs e)
        {
            startDay = startDay.AddDays(7);
            SetWeek();
            UpdateShedule();
        }
    }
}
