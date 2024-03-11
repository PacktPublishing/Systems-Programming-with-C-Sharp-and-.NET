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

namespace ImageManipulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        double _redValue;
        double _greenValue;
        double _blueValue;
        BitmapImage bitmapImage;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(@"./sample.jpeg", UriKind.Relative);
            bitmapImage.EndInit();

            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < 1; i++)
            {


                ReadPixelDataUnsafe(bitmapImage);

            }
            stopwatch.Stop();
            var ellapsed = stopwatch.ElapsedMilliseconds;

            MessageBox.Show($" Time elapsed: {ellapsed:0,000} ms");


        }

        private void ReadPixelDataSafe(BitmapSource bitmapSource)
        {
            {
                int width = bitmapSource.PixelWidth;
                int height = bitmapSource.PixelHeight;

                // Create a new WriteableBitmap from the original bitmap
                WriteableBitmap writeableBitmap = new WriteableBitmap(bitmapSource);

                // Calculate stride (bytes per row)
                int stride = writeableBitmap.BackBufferStride; //(width * writeableBitmap.Format.BitsPerPixel + 7) / 8;

                // Create a byte array to hold the pixel data
                byte[] pixelData = new byte[height * stride];

                // Copy the pixel data from the WriteableBitmap
                writeableBitmap.CopyPixels(pixelData, stride, 0);

                // Access and modify the pixel data
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        // Calculate the pixel index in the byte array
                        int pixelIndex = y * stride + x * writeableBitmap.Format.BitsPerPixel / 8;

                        // Access individual color channels
                        var blue = Convert.ToDouble(pixelData[pixelIndex]);
                        var green = Convert.ToDouble(pixelData[pixelIndex + 1]);
                        var red = Convert.ToDouble(pixelData[pixelIndex + 2]);

                        // Recalculate RGB values
                        byte average = (byte)((red + green + blue) / 3);

                        // Update the pixel data
                        pixelData[pixelIndex] = average;
                        pixelData[pixelIndex + 1] = average;
                        pixelData[pixelIndex + 2] = average;
                    }
                }

                // Write the modified pixel data back to the WriteableBitmap
                writeableBitmap.WritePixels(new Int32Rect(0, 0, width, height), pixelData, stride, 0);

                // Set the updated bitmap as the source for the Image control
                image.Source = writeableBitmap;
            }
        }

        // Do not forget to use Unsafe in the project options at Build
        private unsafe void ReadPixelDataUnsafe(BitmapSource bitmapSource)
        {
            // validate input is not null
            if (bitmapSource == null)
            {
                return;
            }

            int width = bitmapSource.PixelWidth;
            int height = bitmapSource.PixelHeight;

            // Create a writable bitmap from the original bitmap
            WriteableBitmap writeableBitmap = new WriteableBitmap(bitmapSource);

            // Lock the bitmap to access the pixel data
            writeableBitmap.Lock();

            try
            {
                // Get the pointer to the pixel data
                IntPtr pixelDataPtr = writeableBitmap.BackBuffer;

                // Access the pixel data using unsafe code
                unsafe
                {
                    byte* pixelData = (byte*)pixelDataPtr.ToPointer();

                    // Calculate the stride (bytes per row)
                    int stride = writeableBitmap.BackBufferStride;

                    // Access the pixel data
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            // Calculate the pixel index in the byte array
                            int pixelIndex = y * stride + x * writeableBitmap.Format.BitsPerPixel / 8;

                            // Access individual color channels
                            var blue = Convert.ToDouble(pixelData[pixelIndex]);
                            var green = Convert.ToDouble(pixelData[pixelIndex + 1]);
                            var red = Convert.ToDouble(pixelData[pixelIndex + 2]);

                            // Do something with the color values
                            // ...

                            byte average = (byte)((red*_redValue + green*_greenValue + blue*_blueValue) / 3);
                            pixelData[pixelIndex + 2] = Convert.ToByte(red);
                            pixelData[pixelIndex + 1] = Convert.ToByte(green);
                            pixelData[pixelIndex] = Convert.ToByte(blue);

                        }
                    }
                }
            }
            finally
            {
                // Unlock the bitmap after accessing the pixel data
                writeableBitmap.Unlock();
            }

            image.Source = writeableBitmap;
        }

        private void SliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Validate no sliders are null
            if (SliderRed == null || SliderGreen == null || SliderBlue == null)
                return;

            _redValue = Convert.ToDouble(SliderRed.Value) / 100.0;
            _greenValue = Convert.ToDouble(SliderGreen.Value) / 100.0;
            _blueValue = Convert.ToDouble(SliderBlue.Value) / 100.0;

            ReadPixelDataUnsafe(bitmapImage);
        }
    }

}
