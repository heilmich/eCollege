

using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;


namespace eCollege 
{
    public class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            if (value == null) return "placeholder.png";
            return (BitmapImage)Image.Deserialize((string)value);
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    public class AllMarksConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            ObservableCollection<Mark> list = (ObservableCollection<Mark>)value;
            if (list.Count == 0) return "нет оценок";
            string str = "";
            foreach (var mark in list)
            {
                if (mark.Mark1 != 0) str += $" {mark.Mark1}";
            }
            return str;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    public class MarkConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            if ((int)value == 0)
                return "нет оценки";
            else return value;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    public class AvgMarksConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            var list = ((ObservableCollection<Mark>)value).Where(p => p.Mark1 >= 1);
            if (list.Count() != 0)
            {
                return Math.Round(list.Average(p => p.Mark1), 2);
            }
            else return null;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    public class FinalMarksConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            var list = ((ObservableCollection<Mark>)value).Where(p => p.Mark1 >= 1);
            if (list.Count() == 0)
            {
                return null;
            }
            var avg = list.Average(p => p.Mark1);
            if (avg >= 0 && avg < 2.5) return 2;
            else if (avg < 3.5) return 3;
            else if (avg < 4.5) return 4;
            else if (avg <= 5) return 5;
            return null;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    public class UserTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            return ((User)value); //допилить
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            return DependencyProperty.UnsetValue;
        }

        
    }

    public class HomeTaskConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            if (value == null) return "нет д/з";
            return value; 
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            return DependencyProperty.UnsetValue;
        }


    }

    public class TeacherFIOConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            Teacher t = (Teacher)value;
            if (t == null) return null;
            string fio = t.User.LastName + " " + t.User.FirstName.First() + ". " + t.User.Patronymic.First() + ". ";
            return fio;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            return DependencyProperty.UnsetValue;
        }


    }

    public class GroupConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            var user = (User)value;
            if (user.TypeId == 1) return user.Student.First().Group.Name;
            else if (user.TypeId == 2) return user.Teacher.First().Group.First().Name;
            return null;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            return DependencyProperty.UnsetValue;
        }


    }

    public class CourseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            var user = (User)value;
            if (user.TypeId == 1) return user.Student.First().Group.Course;
            else if (user.TypeId == 2) return user.Teacher.First().Group.First().Course;
            return null;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            return DependencyProperty.UnsetValue;
        }


    }
}