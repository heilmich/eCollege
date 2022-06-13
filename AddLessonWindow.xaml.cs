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
        public ShedulePage shedulePage;

        // Создание окна для нового урока
        public AddLessonWindow(SchoolDay schoolDay, ShedulePage shedulePage)
        {
            InitializeComponent();

            currentLesson = new Lesson();
            currentLesson.Date = schoolDay.Date;
            currentLesson.OrderInShedule = 1;
            this.shedulePage = shedulePage;
            this.DataContext = currentLesson;
            this.DialogResult = false;
            GetData();
        }

        // Создание окна для существующего урока
        public AddLessonWindow(Lesson lesson, ShedulePage shedulePage)
        {
            InitializeComponent();

            currentLesson = lesson;
            this.DataContext = currentLesson;
            this.shedulePage = shedulePage;
            this.Title = "Редактировать занятие";
            GetData();
        }

        // Заполнение ComboBox'ов из базы данных
        public void GetData() 
        {
            cbGroup.ItemsSource = Entities.GetContext().Group.ToList();
            cbSubject.ItemsSource = Entities.GetContext().Subject.ToList();
            cbSubject.SelectedItem = currentLesson.Subject;
            cbTeacher.ItemsSource = Entities.GetContext().Teacher.ToList();
        }

        // Добавление (обновление) занятия в базу данных
        private void Click_lbAdd(object sender, MouseButtonEventArgs e)
        {
            try
            {
                currentLesson.TeacherId = selectedTeacher.Id;
                currentLesson.GroupId = selectedGroup.Id;
                currentLesson.SubjectId = selectedSubject.Id;

                if (Entities.GetContext().Lesson.Where(p => p.Group.Id == selectedGroup.Id && p.OrderInShedule == currentLesson.OrderInShedule && p.Date == currentLesson.Date && p.Id != currentLesson.Id).Count() > 0) 
                {
                    MessageBox.Show("Урок с очередью в расписании " + currentLesson.OrderInShedule + " уже существует.");
                    return;
                }

                // Если урока не существует, тогда добавляем новый
                if (Entities.GetContext().Lesson.Find(currentLesson.Id) == null) Entities.GetContext().Lesson.Add(currentLesson);
                Entities.GetContext().SaveChanges();
                MessageBox.Show("Урок добавлен.");
                this.DialogResult = true;
                this.Close();
            } catch (Exception ex) 
            {
                MessageBox.Show("Произошла ошибка! \nКод ошибки: " + ex.Message);
            }
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
