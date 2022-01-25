using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace eCollege
{
    public class SubjectMark : INotifyPropertyChanged
    {
        private string subjectTitle;
        private ObservableCollection<Mark> marks;


        public string SubjectTitle
        {
            get { return subjectTitle; }
            set
            {
                subjectTitle = value;
                OnPropertyChanged("SubjectTitle");
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
