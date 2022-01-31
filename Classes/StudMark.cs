using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace eCollege
{
    public class StudMark : INotifyPropertyChanged
    {
        private Student student;
        private DateTime date;
        private Lesson lesson;
        private Mark mark;
        private int mark1;
        public Entities context;

        public void SetMark() 
        {
            mark = new Mark();
            mark.StudentId = student.Id;
            mark.LessonId = lesson.Id;
            mark.Mark1 = mark1;
        }
        public Student Student 
        {
            get { return student; }
            set 
            { 
                student = value;
                OnPropertyChanged("Student");   
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

        public Lesson Lesson
        {
            get { return lesson; }
            set
            {
                lesson = value;
                OnPropertyChanged("Lesson");
            }
        }

        public Mark Mark
        {
            get { return mark; }
            set
            {
                if (value == null) return;
                if (mark == null) 
                {
                    SetMark();
                }
                mark = value;
                OnPropertyChanged("Mark");
            }
        }

        public int Mark1
        {
            get { return mark1; }
            set {
                if (mark == null) SetMark();
                mark1 = value;
                mark.Mark1 = value;
                OnPropertyChanged("Mark1");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }


    }
}
