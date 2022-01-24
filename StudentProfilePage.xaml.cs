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
    /// Логика взаимодействия для StudentProfilePage.xaml
    /// </summary>
    public partial class StudentProfilePage : Page
    {
        ObservableCollection<Subject> subjectList;
        public StudentProfilePage()
        {
            InitializeComponent();
            lkGrid.DataContext = MainWindow.currentStudent;
            UpdateLBMarks();
        }

        public void UpdateLBMarks() 
        {
            subjectList = GetData();
            if (subjectList == null) 
            {
                tipLabel.Text = "У тебя нет проблем с оценками. Молодец!";
            }
            lbMarks.ItemsSource = subjectList;
            tipLabel.Text = "У тебя есть задолженности по этим предметам.";

        }

        public ObservableCollection<Subject> GetData() 
        {
            return new ObservableCollection<Subject>(Entities.GetContext().Database.SqlQuery<Subject>("SELECT * FROM Subject " +
                                                                                                "WHERE Subject.Id IN ( " +
                                                                                                "SELECT Subject.Id FROM Mark JOIN Lesson ON Mark.LessonId = Lesson.Id " +
                                                                                                "JOIN Subject ON Lesson.SubjectId = Subject.Id " +
                                                                                                "GROUP BY Subject.Id " +
                                                                                                "HAVING AVG(Mark1) < 3.5 ) "));
        }
    }
}
