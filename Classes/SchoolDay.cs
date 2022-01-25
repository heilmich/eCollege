using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace eCollege
{
    public class SchoolDay : INotifyPropertyChanged
    {
        private string monthDay;
        private DateTime date;
        private ObservableCollection<Lesson> lessons;
        private ObservableCollection<Mark> marks;

        public string MonthDay
        {
            get { return monthDay; }
        }

        public DateTime Date
        {
            get { return date; }
            set
            {
                date = value;
                monthDay = date.Day + " " + DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(date.Month) + "\n"+ DateTimeFormatInfo.CurrentInfo.GetAbbreviatedDayName(date.DayOfWeek);
                OnPropertyChanged("Date");
            }
        }

        public ObservableCollection<Lesson> Lessons
        {
            get { return lessons; }
            set
            {
                lessons = value;
                OnPropertyChanged("Lessons");
            }
        }

        public ObservableCollection<Mark> Marks
        {
            get { return marks; }
            set
            {
                marks = value;
                OnPropertyChanged("Marks");
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
