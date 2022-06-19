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
        public DateTime startDay;
        public DateTime endDay;
        public DateTime firstDayOfStudying;
        public DateTime lastDayOfStudying;
        public Group currentGroup;
        public Subject currentSubject;
        public Student currentStudent;
        public Teacher currentTeacher;
        public ObservableCollection<Student> studentList = new ObservableCollection<Student>();
        public ObservableCollection<Subject> subjectList = new ObservableCollection<Subject>();
        public ObservableCollection<Group> groupList = new ObservableCollection<Group>();
        public ObservableCollection<StudMark> studMarks = new ObservableCollection<StudMark>(); // оценки студента


        //public ObservableCollection<StudSubjMarks> subjMarks = new ObservableCollection<StudSubjMarks>();
        //public ObservableCollection<MarkLesson> markLessons = new ObservableCollection<MarkLesson>();

        public TeacherMarksPage()
        {
            InitializeComponent();
            currentTeacher = MainWindow.currentTeacher;

            firstDayOfStudying = new DateTime(2010, 01, 01);
            if (firstDayOfStudying == null) return;
            startDay = firstDayOfStudying;
            StartDayDP.SelectedDate = firstDayOfStudying;

            lastDayOfStudying = new DateTime(2040, 01, 01);
            if (lastDayOfStudying == null) return;
            endDay = lastDayOfStudying;
            EndDayDP.SelectedDate = lastDayOfStudying;

            SetCBData();
            marksGrid.ItemsSource = studMarks;
            GetDataGroup();
        }

        public void SetCBData() 
        {
            cbGroup.ItemsSource = groupList;
            cbStudent.ItemsSource = studentList;
            cbSubject.ItemsSource = subjectList;
        }

        public void GetDataMarks() 
        {
           

            if (currentStudent == null || currentGroup == null || currentSubject == null) return;

            studMarks.Clear();

            var list = Entities.GetContext().Lesson.Where( p => p.Teacher.Id == MainWindow.currentTeacher.Id 
                && p.SubjectId == currentSubject.Id 
                && p.GroupId == currentGroup.Id)
                .Where(p => p.Date >= startDay 
                    && p.Date <= endDay)
                .ToList();

            foreach(var item in list) 
            {
                StudMark stud = new StudMark();

                stud.Student = currentStudent;
                stud.Lesson = item;
                stud.Mark = Entities.GetContext().Mark
                    .Where(p => p.StudentId == stud.Student.Id && p.LessonId == item.Id)
                    .DefaultIfEmpty().First();
                if (stud.Mark == null) stud.SetMark();
                stud.Mark1 = stud.Mark.Mark1;
                stud.Date = item.Date;
                
                studMarks.Add(stud);
            }

            
        }


        public void GetDataGroup() 
        {
            groupList.Clear();
            foreach(var item in (new ObservableCollection<Group>( Entities.GetContext().Group.SqlQuery("SELECT * FROM [Group] " +
                                                                                "WHERE Id IN ( " +
                                                                                "SELECT Lesson.GroupId FROM Lesson JOIN Teacher ON Lesson.TeacherId = Teacher.Id " +
                                                                                $"WHERE Teacher.Id = {MainWindow.currentTeacher.Id} ) ")
                                                                                .ToList()))) 
            {
                groupList.Add(item);
            }
            
        } 

        public void GetDataSubject() 
        {
            subjectList.Clear();
            foreach(var item in (new ObservableCollection<Subject>( Entities.GetContext().Subject.SqlQuery("SELECT * FROM Subject " +
                                                                                "WHERE Subject.Id IN ( " +
                                                                                "SELECT SubjectId FROM Lesson JOIN Teacher ON Lesson.TeacherId = Teacher.Id " +
                                                                                "  " +
                                                                                $"WHERE Teacher.Id = {MainWindow.currentTeacher.Id} AND Lesson.GroupId = {currentGroup.Id} ) ")
                                                                                .ToList()))) 
            {
                subjectList.Add(item);
            }
        }

        public void GetDataStudent() 
        {
            studentList.Clear();
            foreach(var item in (new ObservableCollection<Student>( Entities.GetContext().Student.SqlQuery("SELECT * FROM Student " +
                                                                                $"WHERE GroupId = {currentGroup.Id} ")
                                                                                .ToList()))) 
            {
                studentList.Add(item);
            }
        }

        private void SelectionChanged_cbGroup(object sender, SelectionChangedEventArgs e)
        {
            currentGroup = (Group)(cbGroup.SelectedItem);
            GetDataSubject();
        }

        private void SelectionChanged_cbSubject(object sender, SelectionChangedEventArgs e)
        {
            currentSubject = (Subject)(cbSubject.SelectedItem);
            GetDataStudent();
            
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

        private void StartDayDP_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            startDay = (DateTime)StartDayDP.SelectedDate;
            GetDataMarks();
        }

        private void EndDayDP_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            endDay = (DateTime)EndDayDP.SelectedDate;
            GetDataMarks();
        }
    }
}
