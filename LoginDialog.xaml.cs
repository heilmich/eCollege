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
    /// Логика взаимодействия для LoginDialog.xaml
    /// </summary>
    public partial class LoginDialog : Window
    {
        public static Entities db = new Entities();
        public User user;
        public LoginDialog()
        {

            InitializeComponent();
           
        }

        private void btnSignIn_Click(object sender, RoutedEventArgs e)
        {
            Authorization authorization = new Authorization();
            User user = (User)authorization.SignIn(tbLogin.Text, tbPassword.Text, db);
            if (user == null) return;
            MainWindow mainWindow = new MainWindow(user);
            db.Dispose();
            mainWindow.Show();
            this.Close();
        }

    }
}
