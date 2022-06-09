using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using iText;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;

namespace eCollege
{
    /// <summary>
    /// Логика взаимодействия для MarksPage.xaml
    /// </summary>
    public partial class MarksPage : Page
    {

        public Document document;

        public MarksPage()
        {
            InitializeComponent();
            UpdateMarks();
            
        }



        public void UpdateMarks()
        {
            MainWindow.subjectMarks.Clear();

            foreach (var item in MainWindow.subjectsList)
            {
                SubjectMark sm = new SubjectMark();
                sm.SubjectTitle = item.Title;
                sm.Marks = new ObservableCollection<Mark>(MainWindow.currentStudent.Mark.Where(p => p.Lesson.Subject == item));
                MainWindow.subjectMarks.Add(sm);
            }

            finalMarksGrid.ItemsSource = MainWindow.subjectMarks;
        }

        public void CreateDocument(object sender, RoutedEventArgs e) 
        {
            document = new Document();

            string docstr = null;

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "PDF files (*.pdf)|*.pdf;";
            sfd.Title = "Выберите папку для сохранения";
            sfd.ShowDialog();
            if ( sfd.FileName == null || sfd.FileName == "") return;
            

            using (var writer = PdfWriter.GetInstance(document, new FileStream(sfd.FileName, FileMode.Create))) 
            {
                document.Open();

                BaseFont baseFont = BaseFont.CreateFont("C:\\Windows\\Fonts\\arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                iTextSharp.text.Font font = new iTextSharp.text.Font(baseFont, iTextSharp.text.Font.DEFAULTSIZE, iTextSharp.text.Font.NORMAL);
                
                PdfPTable marksTable = new PdfPTable(4);

                PdfPCell subjectHeader = new PdfPCell(new Phrase("Предмет", font));
                marksTable.AddCell(subjectHeader);
                PdfPCell marksHeader = new PdfPCell(new Phrase("Оценки", font));
                marksTable.AddCell(marksHeader);
                PdfPCell avgMarkHeader = new PdfPCell(new Phrase("Средний балл", font));
                marksTable.AddCell(avgMarkHeader);
                PdfPCell finalMarkHeader = new PdfPCell(new Phrase("Итоговая оценка", font));
                marksTable.AddCell(finalMarkHeader);

                foreach (var item in MainWindow.subjectsList)
                {
                    PdfPCell subject = new PdfPCell(new Phrase(item.Title, font));
                    marksTable.AddCell(subject);
                    
                    var marksList = new ObservableCollection<Mark>(MainWindow.currentStudent.Mark.Where(p => p.Lesson.Subject == item));
                    var marksPhrase = new Phrase("", font);
                    foreach (var m in marksList) 
                    {
                        if (m.Mark1 != 0)
                        marksPhrase.Add(m.Mark1 + " ");
                    }
                    PdfPCell mark = new PdfPCell(marksPhrase);
                    marksTable.AddCell(mark);

                    string avg = Convert.ToString(marksList.Where(p => p.Mark1 >= 1).Average(p => p.Mark1));
                    PdfPCell avgMark = new PdfPCell(new Phrase("" + avg, font));
                    marksTable.AddCell(avgMark);

                    string final = Convert.ToString(Math.Round(marksList.Where(p => p.Mark1 >= 1).Average(p => p.Mark1)));
                    PdfPCell finalMark = new PdfPCell(new Phrase("" + final, font));
                    marksTable.AddCell(finalMark);

                }

                document.Add(marksTable);
                document.Add(new Phrase("Лооол"));
                document.Close();
                
                writer.Close();
            }
           
        }

    }
}
