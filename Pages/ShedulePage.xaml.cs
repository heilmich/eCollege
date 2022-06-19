using Microsoft.Build.Tasks.Xaml;
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
        public ObservableCollection<SchoolDay> dayList = new ObservableCollection<SchoolDay>();
        public Group currentGroup;
        public ObservableCollection<Lesson> listLesson;
        public int lessonsPerDay = 8;
        public DateTime startDay = DateTime.Today;
        public DateTime endDay;
        public static bool isKeyDown = false;

        public ShedulePage()
        {
            InitializeComponent();
            SetWeek();
            listLesson = new ObservableCollection<Lesson>();
            mainSheduleGrid.ItemsSource = dayList;
            if (MainWindow.currentUser.TypeId == 1)
            {
                currentGroup = MainWindow.currentUser.Student.First().Group;
            }
            else if (MainWindow.currentUser.TypeId == 2)
            {
                currentGroup = MainWindow.currentUser.Teacher.First().Group.First();
                dgMark.Visibility = Visibility.Collapsed;
                dgHomeTask.IsReadOnly = false;
                
            }
            else if (MainWindow.currentUser.TypeId == 3)
            {
                AddLessonBTN.Visibility = Visibility.Visible;
                
                dgMark.Visibility = Visibility.Collapsed;
                dgLessons.IsReadOnly = false;
                GetGroups_cbGroup();
            }
            UpdateShedule();
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


        public async void UpdateSheduleAsync()
        {
            await Task.Run(() => UpdateShedule());
        }

        public void UpdateShedule()
        {
            var listLessonsTemp = currentGroup.Lesson.ToList();
            if (MainWindow.currentUser.UserType.Id == 2) 
            {
                listLessonsTemp = MainWindow.currentUser.Teacher.First().Lesson.ToList();
            }
            listLesson.Clear();

            foreach (var item in listLessonsTemp)
            {
                listLesson.Add(item);
            }

            dayList.Clear();
            for (DateTime i = startDay; i.DayOfYear < endDay.DayOfYear; i = i.AddDays(1))
            {
                SchoolDay day = new SchoolDay();

                ObservableCollection<Lesson> list = new ObservableCollection<Lesson>(listLesson.Where(p => p.Date.DayOfYear == i.DayOfYear).OrderBy(p => p.OrderInShedule));

                ObservableCollection<Mark> listMark = new ObservableCollection<Mark>();
                foreach (var item in list)
                {
                    var it = MainWindow.marksList.Where(p => p.LessonId == item.Id).FirstOrDefault();
                    if (it == null) it = new Mark();
                    listMark.Add(it);
                }
                day.Date = i.Date;
                day.Marks = listMark;
                day.Lessons = list;
                dayList.Add(day);
            }


        }

        private void GetGroups_cbGroup() 
        {
            cbGroup.Visibility = Visibility.Visible;
            cbGroup.ItemsSource = Entities.GetContext().Group.ToList();
        }

        private void Click_PreviousWeek(object sender, RoutedEventArgs e)
        {
            startDay = startDay.AddDays(-7);
            SetWeek();
            UpdateShedule();
        }

        private void Click_NextWeek(object sender, RoutedEventArgs e)
        {
            startDay = startDay.AddDays(7);
            SetWeek();
            UpdateShedule();
        }

        private void SelectionChanged_cbGroup(object sender, SelectionChangedEventArgs e)
        {
            currentGroup = (Group)cbGroup.SelectedItem;
           
            UpdateShedule();
        }


        private void dgLessons_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            

                if (MainWindow.currentUser.TypeId != 3) return;

                var item = (Lesson)((DataGrid)sender).SelectedItem;
                var sheditem = (SchoolDay)mainSheduleGrid.SelectedItem;

                if (item != null)
                {
                    AddLessonWindow addLessonWindow = new AddLessonWindow((Lesson)((DataGrid)sender).SelectedItem, this);
                    if (addLessonWindow.ShowDialog() == true) UpdateShedule();
                }
                /*else if (item != null && sheditem.GetType() == typeof(SchoolDay))
                {
                    AddLessonWindow addLessonWindow = new AddLessonWindow((SchoolDay)mainSheduleGrid.SelectedItem, this);
                    if (addLessonWindow.ShowDialog() == true) UpdateShedule();
                }*/
            
        }

        private void dgLessons_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MainWindow.currentUser.TypeId != 3) return;

            var item = (Lesson)((DataGrid)sender).SelectedItem;


            if (item != null)
            {
                AddLessonWindow addLessonWindow = new AddLessonWindow((Lesson)((DataGrid)sender).SelectedItem, this);
                if (addLessonWindow.ShowDialog() == true) UpdateShedule();
            }
            /*else if (item != null && item.GetType() == typeof(SchoolDay))
            {
                AddLessonWindow addLessonWindow = new AddLessonWindow((SchoolDay)mainSheduleGrid.SelectedItem, this);
                if (addLessonWindow.ShowDialog() == true) UpdateShedule();
            }*/
        }

        private void dgHomeTask_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (MainWindow.currentUser.TypeId != 2) return;

            var lesson = e.Row.Item as Lesson;
            string hometask = lesson.HomeTask;

            if (lesson == null) return;

            Entities.GetContext().Lesson.Find(lesson.Id).HomeTask = hometask;

            Entities.GetContext().SaveChanges();
            MessageBox.Show($"Обновлено {lesson.Id} + {hometask}");
        }

        private void AddLessonBTN_Click(object sender, RoutedEventArgs e)
        {
            AddLessonWindow addLessonWindow = new AddLessonWindow(this);
            if (addLessonWindow.ShowDialog() == true) UpdateShedule();
        }

        private void EditLessonBTN_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.currentUser.TypeId != 3) return;

            //var item = ((DataGrid)sender).SelectedItem;
            var sheditem = (SchoolDay)mainSheduleGrid.SelectedItem;

            
            if (sheditem != null && sheditem.GetType() == typeof(SchoolDay))
            {
                //AddLessonWindow addLessonWindow = new AddLessonWindow((SchoolDay)mainSheduleGrid.SelectedItem, this);
                //if (addLessonWindow.ShowDialog() == true) UpdateShedule();
            }
        }
    }
}
