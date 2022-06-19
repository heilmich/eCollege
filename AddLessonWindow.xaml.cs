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
        public AddLessonWindow(ShedulePage shedulePage)
        {
            InitializeComponent();

            currentLesson = new Lesson();
            DateDP.SelectedDate = DateTime.Now;
            currentLesson.Date = DateTime.Now;

            currentLesson.OrderInShedule = 1;
            this.shedulePage = shedulePage;
            this.DataContext = currentLesson;
            //this.DialogResult = false;
            GetData();
        }

        

        // Создание окна для существующего урока
        public AddLessonWindow(Lesson lesson, ShedulePage shedulePage)
        {
            InitializeComponent();
            cbSubject.SelectedItem = lesson.Subject;
            cbTeacher.SelectedItem = lesson.Teacher;
            cbGroup.SelectedItem = lesson.Group;
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
        private void AddBTN_Click(object sender, RoutedEventArgs e)
        {
            
                currentLesson.TeacherId = selectedTeacher.Id;
                currentLesson.GroupId = selectedGroup.Id;
                currentLesson.SubjectId = selectedSubject.Id;


                var minOrder = 0; var maxOrder = 8;
                if (currentLesson.OrderInShedule <= minOrder || currentLesson.OrderInShedule >= maxOrder) 
                {
                    MessageBox.Show($"Очередь в расписании некорректна.\nДоступные рамки: {minOrder} - {maxOrder}.\nВаше значение: {currentLesson.OrderInShedule}");
                    return;
                }

                if (Entities.GetContext().Lesson.Where(p => p.Group.Id == selectedGroup.Id 
                && p.OrderInShedule == currentLesson.OrderInShedule 
                && p.Date == currentLesson.Date 
                && p.Id != currentLesson.Id).Count() > 0) 
                {
                    MessageBox.Show("Урок с очередью в расписании " + currentLesson.OrderInShedule + " уже существует.");
                    return;
                }

                // Если урока не существует, тогда добавляем новый
                if (Entities.GetContext().Lesson.Find(currentLesson.Id) == null) 
                    Entities.GetContext().Lesson.Add(currentLesson);

                Entities.GetContext().SaveChanges();
                MessageBox.Show("Урок добавлен.");

                this.DialogResult = true;
                this.Close();

            
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

        private void RemoveBTN_Click(object sender, RoutedEventArgs e)
        {

            Entities.GetContext().Mark.RemoveRange(currentLesson.Mark);    
            Entities.GetContext().Lesson.Remove(currentLesson);
            Entities.GetContext().SaveChanges();
            MessageBox.Show("Урок удален");
            this.DialogResult = true;
            this.Close();
            
        }
    }
}
