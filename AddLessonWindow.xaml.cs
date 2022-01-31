using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для AddLessonWindow.xaml
    /// </summary>
    public partial class AddLessonWindow : Window
    {
        public Lesson currentLesson;
        public Group selectedGroup;
        public Teacher selectedTeacher;
        public Subject selectedSubject;
        public AddLessonWindow()
        {
            InitializeComponent();
            currentLesson = new Lesson();
            currentLesson.Date = DateTime.Today.AddDays(1);
            this.DataContext = currentLesson;
            GetData();
        }

        public AddLessonWindow(Lesson lesson)
        {
            InitializeComponent();
            currentLesson = lesson;
            this.DataContext = currentLesson;
            GetData();
        }

        public void GetData() 
        {
            cbGroup.ItemsSource = Entities.GetContext().Group.ToList();
            cbSubject.ItemsSource = Entities.GetContext().Subject.ToList();
            cbTeacher.ItemsSource = Entities.GetContext().Teacher.ToList();
        }

        private void Click_lbAdd(object sender, MouseButtonEventArgs e)
        {
            currentLesson.TeacherId = selectedTeacher.Id;
            currentLesson.GroupId = selectedGroup.Id;
            currentLesson.SubjectId = selectedSubject.Id;
            if (Entities.GetContext().Lesson.Find(currentLesson.Id) == null) Entities.GetContext().Lesson.Add(currentLesson);

            Entities.GetContext().SaveChanges();
            MessageBox.Show("Урок добавлен.");
        }

        private void SelectionChanged_cbGroup(object sender, SelectionChangedEventArgs e)
        {
            selectedGroup = (Group)cbGroup.SelectedItem;
        }

        private void SelectionChanged_cbSubject(object sender, SelectionChangedEventArgs e)
        {
            selectedSubject = (Subject)cbSubject.SelectedItem;
        }

        private void SelectionChanged_cbTeacher(object sender, SelectionChangedEventArgs e)
        {
            selectedTeacher = (Teacher)cbTeacher.SelectedItem;
        }
    }
}
