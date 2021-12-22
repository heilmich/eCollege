using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace eCollege
{
    class Authorization
    {
        private int minPasswordLength;
        private int minLoginLength;

        public Authorization() 
        {
            minPasswordLength = 7;
            minLoginLength = 4;
        }
        public bool SignCheck(string password, string login)
        {
            bool check = true;
            //Проверка длины пароля
            if (password.Length < minPasswordLength)
            {
                MessageBox.Show("Пароль слишком короткий", "Ошибка");
                check = false;
            }

            //Проверка длины логина
            if (login.Length < minLoginLength)
            {
                MessageBox.Show("Логин слишком короткий", "Ошибка");
                check = false;
            }

            //Проверка на наличие хотя бы одной заглавной буквы
            var str = password.ToArray();
            bool HasUpperChar = false;
            foreach (var item in str)
            {
                if (char.IsUpper(item))
                {
                    HasUpperChar = true;
                }
            }
            if (HasUpperChar == false)
            {
                MessageBox.Show("Нужна как минимум одна заглавная буква", "Ошибка");
                check = false;
            }

            if (check == false) return false;
            return true;

        }

        /* public static Students StudentFind(Users user)
        {
            using (Entities db = new Entities())
            {
                var student = from p in db.Students
                              where p.Код_студента == user.Код_студента
                              select p;
                foreach (var item in student)
                {
                    return item;
                }
            }
            return null;
        }

        public static Teachers TeacherFind(Users user)
        {
            using (Entities db = new Entities())
            {
                var teacher = from p in db.Teachers
                              where p.Код_преподавателя == user.Код_преподавателя
                              select p;
                foreach (var item in teacher)
                {
                    return item;
                }
            }
            return null;
        }

        */
        
        public Object SignIn(string login, string password, Entities db)
        {


                //Проверка на соответствие правилам ввода логина и пароля
            if (SignCheck(password, login) == false) return null;

            User user = db.User.Where( p => p.Login == login && p.Password == password).FirstOrDefault();
            if (user != null) return user;              

            //Действие при неудачном входе
            return null;
        }

    }
}
