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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;

namespace WpfApp1

{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private string fileName = "";

        private void ButtonOpenFile_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();
            Nullable<bool> result = openFileDlg.ShowDialog();
            fileName = openFileDlg.FileName;
            imageDialog1.Source = new BitmapImage(new Uri(fileName));
        }

        private void ButtonMakeGray_Click(object sender, RoutedEventArgs e)
        {
            if (imageDialog1.Source != null)
            {
                var bmpInput = new Bitmap(fileName, true);
                var bmpOutput = new Bitmap(bmpInput.Width, bmpInput.Height);
                for (int i = 0; i < bmpInput.Height; i++)
                    for (int j = 0; j < bmpInput.Width; j++)
                    {
                        UInt32 pixel = (UInt32)(bmpInput.GetPixel(i, j).ToArgb());
                        float R = (float)((pixel & 0x00FF0000) >> 16); // красный
                        float G = (float)((pixel & 0x0000FF00) >> 8); // зеленый
                        float B = (float)(pixel & 0x000000FF); // синий
                        R = G = B = (R + G + B) / 3.0f;
                        UInt32 newPixel = 0xFF000000 | ((UInt32)R << 16) | ((UInt32)G << 8) | ((UInt32)B);
                        bmpOutput.SetPixel(i, j, System.Drawing.Color.FromArgb((int)newPixel));
                    }
                Bitmap bitmap = new Bitmap(bmpOutput);
                string directory = fileName.Substring(0, fileName.LastIndexOf('\\'));
                directory += "\\changed" + DateTime.Now.ToString("ff") + ".bmp";
                bitmap.Save(directory);
                imageDialog2.Source = new BitmapImage(new Uri(directory));
            }
            else
            {
                MessageBox.Show("картинка не открыта");
            }
        }

        private void ButtonGrayCounter_Click(object sender, RoutedEventArgs e)
        {
            if (imageDialog1.Source != null)
            {
                var bmpInput = new Bitmap(fileName, true);
                int count = 0;
                for (int i = 0; i < bmpInput.Height; i++)
                    for (int j = 0; j < bmpInput.Width; j++)
                    {
                        UInt32 pixel = (UInt32)(bmpInput.GetPixel(i, j).ToArgb());
                        float R = (float)((pixel & 0x00FF0000) >> 16); // красный
                        float G = (float)((pixel & 0x0000FF00) >> 8); // зеленый
                        float B = (float)(pixel & 0x000000FF); // синий
                        if (R == G && G == B)
                            count++;
                    }
                double percent = count / ((double)bmpInput.Width * (double)bmpInput.Height) * 100;
                label.Content = percent.ToString() + " %";
            }
            else
            {
                MessageBox.Show("картинка не открыта");
            }
        }
    }
}
