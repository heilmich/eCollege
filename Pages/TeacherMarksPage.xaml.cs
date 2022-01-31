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
    /// Логика взаимодействия для TeacherMarksPage.xaml
    /// </summary>


    
    public partial class TeacherMarksPage : Page
    {
        public Group currentGroup;
        public Subject currentSubject;
        public Student currentStudent;
        public ObservableCollection<StudMark> studMarks = new ObservableCollection<StudMark>();

        //public ObservableCollection<StudSubjMarks> subjMarks = new ObservableCollection<StudSubjMarks>();
        //public ObservableCollection<MarkLesson> markLessons = new ObservableCollection<MarkLesson>();

        public TeacherMarksPage()
        {
            InitializeComponent();
            GetDataGroup();
        }

        public void GetDataMarks() 
        {
            if (currentStudent == null) return;
            studMarks.Clear();
            var list = Entities.GetContext().Lesson.Where( p => p.Teacher.Id == MainWindow.currentTeacher.Id && p.SubjectId == currentSubject.Id && p.GroupId == currentGroup.Id).ToList();
            foreach(var item in list) 
            {
                StudMark stud = new StudMark();

                stud.Student = currentStudent;
                stud.Lesson = item;
                stud.Mark = Entities.GetContext().Mark.Where(p => p.StudentId == stud.Student.Id && p.LessonId == item.Id).DefaultIfEmpty().First();
                if (stud.Mark == null) stud.SetMark();
                stud.Mark1 = stud.Mark.Mark1;
                stud.Date = item.Date;
                
                studMarks.Add(stud);
            }

            marksGrid.ItemsSource = studMarks;
        }


        /* public void GetDataMarkss() 
        {
            subjMarks.Clear();
            marksGrid.Columns.Clear();
            

            
            var list = new ObservableCollection<Lesson>(Entities.GetContext().Lesson.Where(p => p.Teacher.Id == MainWindow.currentTeacher.Id && p.Group.Id == currentGroup.Id && p.Subject.Id == currentSubject.Id));

            

            foreach (var student in currentGroup.Student)
            {
                StudSubjMarks stud = new StudSubjMarks();
                stud.student = student;
                stud.marks = new ObservableCollection<Mark>(Entities.GetContext().Mark.Where(p => p.Lesson.Teacher == MainWindow.currentTeacher && p.Student == stud.student && p.Lesson.Subject == currentSubject));
                ObservableCollection<Mark> listMark = new ObservableCollection<Mark>();
                foreach (var item in list)
                {
                    var it = MainWindow.marksList.Where(p => p.LessonId == item.Id).FirstOrDefault();
                    if (it == null) it = new Mark();
                    listMark.Add(it);
                }
                subjMarks.Add(stud);
            }

            foreach(var item in list) 
            {
                MarkLesson mark = new MarkLesson();
                mark.Date = item.Date;
                mark.students = subjMarks;
                markLessons.Add(mark);
                DataGridTextColumn col = new DataGridTextColumn();
                Binding bind = new Binding();
                bind.Source = mark;
                col.Binding = bind;
                Binding bindHeader = new Binding();
                bindHeader.Source = mark.Date;
                col.Header = bindHeader;
                marksGrid.Columns.Add(col);
            }


            marksGrid.ItemsSource = markLessons;
        } */

        public void GetDataGroup() 
        {
            cbGroup.ItemsSource = Entities.GetContext().Database.SqlQuery<Group>("SELECT * FROM [Group] " +
                                                                                "WHERE Id IN ( " +
                                                                                "SELECT Lesson.GroupId FROM Lesson JOIN Teacher ON Lesson.TeacherId = Teacher.Id " +
                                                                                $"WHERE Teacher.Id = {MainWindow.currentTeacher.Id} ) ")
                                                                                .ToList();
            
        } 

        public void GetDataSubject() 
        {
            cbSubject.ItemsSource = Entities.GetContext().Database.SqlQuery<Subject>("SELECT * FROM Subject " +
                                                                                "WHERE Subject.Id IN ( " +
                                                                                "SELECT SubjectId FROM Lesson JOIN Teacher ON Lesson.TeacherId = Teacher.Id " +
                                                                                "  " +
                                                                                $"WHERE Teacher.Id = {MainWindow.currentTeacher.Id} AND Lesson.GroupId = {currentGroup.Id} ) ")
                                                                                .ToList();
        }

        public void GetDataStudent() 
        {
            cbStudent.ItemsSource = Entities.GetContext().Database.SqlQuery<Student>("SELECT * FROM Student " +
                                                                                $"WHERE GroupId = {currentGroup.Id} ")
                                                                                .ToList(); ;
        }

        private void SelectionChanged_cbGroup(object sender, SelectionChangedEventArgs e)
        {
            currentGroup = (Group)(cbGroup.SelectedItem);
            GetDataSubject();
            cbSubject.SelectedIndex = 0;
        }

        private void SelectionChanged_cbSubject(object sender, SelectionChangedEventArgs e)
        {
            currentSubject = (Subject)(cbSubject.SelectedItem);
            GetDataStudent();
            cbStudent.SelectedIndex = 0;
        }

        private void SelectionChanged_cbStudent(object sender, SelectionChangedEventArgs e) 
        {
            currentStudent = (Student)(cbStudent.SelectedItem);
            GetDataMarks();
        }

        private void RowEditEnding_marksGrid(object sender, DataGridRowEditEndingEventArgs e)
        {
            var stud = (StudMark)marksGrid.SelectedItem;

            if (stud.Mark == null) return;
            if (Entities.GetContext().Mark.Find(stud.Mark.Id) == null || Entities.GetContext().Mark.Find(stud.Mark.Id).Id == 0)
            {
                Mark mark = new Mark();
                mark = stud.Mark;
                mark.Id = stud.Mark.Id;
                Entities.GetContext().Mark.Add(mark);
            }
            else
            {
                Entities.GetContext().Mark.Find(stud.Mark.Id).Mark1 = stud.Mark.Mark1;
            }


            Entities.GetContext().SaveChanges();
            
            //var mark = Entities.GetContext().Mark.Where( p => p.StudentId == stud.Student.Id && p.LessonId == stud.Lesson.Id);
        }
    }
}
