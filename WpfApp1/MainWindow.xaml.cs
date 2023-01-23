using System;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Drawing;

namespace WpfApp1

{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            TextBlock.Visibility = Visibility.Hidden;
            label.Visibility = Visibility.Hidden;
        }

        private string fileName = "";

        private void ButtonOpenFile_Click(object sender, RoutedEventArgs e) //выводим исходное изображение на экран
        {
            try
            {
                Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();
                openFileDlg.Filter = "Файлы рисунков (*.bmp, *.jpg)|*.bmp;*.jpg;*.tif";
                Nullable<bool> result = openFileDlg.ShowDialog();
                fileName = openFileDlg.FileName;
                imageDialog1.Source = new BitmapImage(new Uri(fileName));
            }
            catch (Exception)
            {
                MessageBox.Show("Выберите картинку");
            }
        }

        private void ButtonMakeGray_Click(object sender, RoutedEventArgs e) // делаем исходное изображение черно-белым
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

        private void ButtonGrayCounter_Click(object sender, RoutedEventArgs e) // расчитываем количество серого на исходном изображении
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
                label.Content = (count / ((double)bmpInput.Width * (double)bmpInput.Height) * 100).ToString() + " %";
                label.Visibility = Visibility.Visible;
                TextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("картинка не открыта");
            }
        }
    }
}
