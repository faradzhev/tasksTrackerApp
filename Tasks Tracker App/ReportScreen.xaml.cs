using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Tasks_Tracker_App
{
    /// <summary>
    /// Логика взаимодействия для ReportScreen.xaml
    /// </summary>
    public partial class ReportScreen : Window
    {
        public ReportScreen()
        {
            InitializeComponent();

            Uri iconUri = new Uri("pack://application:,,,/icon.ico", UriKind.RelativeOrAbsolute);
            this.Icon = BitmapFrame.Create(iconUri);
        }

        public ReportScreen(string text) : this()
        {
            textBox.Text = text;
        }

        private void saveToFileBtn_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                FileName = DateTime.Now.ToLongDateString() + " Report.txt",
                Filter = "Text File (*.txt)|*.txt",
                InitialDirectory = @"C:\Users\%USERNAME%\Desktop\",
            };

            saveFileDialog.ShowDialog();

            File.WriteAllText(saveFileDialog.FileName, textBox.Text);
        }

        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
