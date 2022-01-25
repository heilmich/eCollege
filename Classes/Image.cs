using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.Windows;

namespace eCollege
{
    internal class Image
    {
        public static string SerializeFromDialog() 
        {
            string picstr = null;
            
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg; *.png; *.jpeg";
            openFileDialog.Multiselect = false;
            openFileDialog.Title = "Выберите изображение";
            openFileDialog.ShowDialog();
            if (openFileDialog.FileName != null)
                picstr = Serialize(openFileDialog.FileName);
            else MessageBox.Show("Вы не выбрали изображение");
            return picstr;
        }
        public static string Serialize(string path) // сериализация в строку
        {
            string pic;
            DirectoryInfo dirInfo = new DirectoryInfo(path);

            using (FileStream fs = File.OpenRead(path))
            {
                byte[] array = new byte[fs.Length];
                fs.Read(array, 0, array.Length);

                pic = JsonSerializer.Serialize(array);
                return pic;

            }
        }

        public static BitmapImage Deserialize(string value) // десериализация json строки из БД в изображение
        {
            byte[] array = System.Convert.FromBase64String(JsonSerializer.Deserialize<string>((string)value));
            MemoryStream ms = new MemoryStream(array, 0, array.Length);
            BitmapImage img = new BitmapImage();

            img.BeginInit();
            img.StreamSource = ms;
            img.CacheOption = BitmapCacheOption.OnLoad;
            img.EndInit();
            img.Freeze();
            return img;
        }
    }
}
