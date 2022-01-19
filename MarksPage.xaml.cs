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
    /// Логика взаимодействия для MarksPage.xaml
    /// </summary>
    public partial class MarksPage : Page
    {

        public MarksPage()
        {
            InitializeComponent();
            UpdateMarksAsync();

        }

        public void GetDataStudent(Student student)
        {
            MainWindow.lessonsList = student.Group.Lesson.ToList();
            MainWindow.marksList = student.Mark.ToList();
            MainWindow.subjectsList = student.Group.Lesson.Select(p => p.Subject).Distinct().ToList();
        }

        public async void UpdateMarksAsync()
        {
            await Task.Run(() => UpdateMarks());

            finalMarksGrid.ItemsSource = MainWindow.subjectMarks;
        }
        public void UpdateMarks()
        {
            MainWindow.subjectMarks.Clear();

            foreach (var item in MainWindow.subjectsList)
            {
                SubjectMark sm = new SubjectMark();
                sm.SubjectTitle = item.Title;
                sm.Marks = new ObservableCollection<Mark>(MainWindow.currentStudent.Mark.Where(p => p.Lesson.Subject == item));
                MainWindow.subjectMarks.Add(sm);
            }
        }


    }
}
