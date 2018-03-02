/*
Exhibit - A program used by color-blind people to help them identify the color of a pixel on 
          the screen by moving the cursor on it. This can greatly help them in situations 
          where they want to be sure to choose the right color, but can't identify it 
          due to their inability to differntiate certain colors.
Author  - Salah Al-Dhaferi
Usage   - Free for everyone.
-----------------------------------------------------------------------------------------
I've used [John Gietzen] answer from this stackoverflow page [https://stackoverflow.com/questions/1483928/how-to-read-the-color-of-a-screen-pixel]. Thanks John!
*/
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
namespace exhibit
{
    public partial class frmMain : Form
    {
        private bool mouseMoveTimer = false;
        
        Bitmap screenPixel = new Bitmap(1, 1, PixelFormat.Format32bppArgb);

        [DllImport("user32.dll")]
        static extern bool GetCursorPos(ref Point lpPoint);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);

        public frmMain()
        {
            InitializeComponent();
        }
        public Color GetColorAt(Point location)
        {
            using (Graphics gdest = Graphics.FromImage(screenPixel))
            {
                using (Graphics gsrc = Graphics.FromHwnd(IntPtr.Zero))
                {
                    IntPtr hSrcDC = gsrc.GetHdc();
                    IntPtr hDC = gdest.GetHdc();
                    int retval = BitBlt(hDC, 0, 0, 1, 1, hSrcDC, location.X, location.Y, (int)CopyPixelOperation.SourceCopy);
                    gdest.ReleaseHdc();
                    gsrc.ReleaseHdc();
                }
            }

            return screenPixel.GetPixel(0, 0);
        }
        
        private void frmMain_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!mouseMoveTimer)
            {
                MouseMoveTimer.Start();
                mouseMoveTimer = true;
            }
            else
            {
                MouseMoveTimer.Stop();
                mouseMoveTimer = false;

            }
        }
        private static string HexConverter(Color c)
        {
            return String.Format("{0:X6}", c.ToArgb() & 0x00FFFFFF);
            
        }

        private void MouseMoveTimer_Tick_1(object sender, EventArgs e)
        {
            Point cursor = new Point();
            GetCursorPos(ref cursor);

            var c = GetColorAt(cursor);
            
            textBox1.Text = NameOfColorHSV(c);

        }
        #region RGB
        /*
        Using RGB wasn't the best idea since there was some sort of mix-up between color values. Also,
        The arangment of the colors was very important to be in an ascending order to make sure that 
        The values are accurate.
        */
        private string NameOfColorRGB(Color c)
        {
            //This will check the RGB values of the color and will determine the name of the color.
            //All colors are based on the values from workwithcolor.com page [Check link bellow].
            //http://www.workwithcolor.com/orange-brown-color-hue-range-01.htm

            //The result that will be returned.
            string colorName = null;            
            
            //Pink-Red
            if ((c.R <= 255 && c.R >= 101) && (c.G <= 193 && c.G >= 0) && (c.B <= 204 && c.B >= 11))
            {
                colorName = "Pink-Red (" + c.R + "," + c.G + "," + c.B + ")";

            }
            //Pink
            else if ((c.R <= 255 && c.R >= 86) && (c.G <= 209 && c.G >= 0) && (c.B <= 220 && c.B >= 25))
            {
                colorName = "Pink (" + c.R + "," + c.G + "," + c.B + ")";

            }
            //Magenta-Pink
            else if ((c.R <= 255 && c.R >= 97) && (c.G <= 240 && c.G >= 0) && (c.B <= 245 && c.B >= 81))
            {
                colorName = "Magenta-Pink (" + c.R + "," + c.G + "," + c.B + ")";

            }
            //Magenta
            else if ((c.R <= 255 && c.R >= 93) && (c.G <= 159 && c.G >= 0) && (c.B <= 255 && c.B >= 84))
            {
                colorName = "Magenta (" + c.R + "," + c.G + "," + c.B + ")";

            }
            //Blue-Magenta
            else if ((c.R <= 191 && c.R >= 105) && (c.G <= 148 && c.G >= 0) && (c.B <= 255 && c.B >= 150))
            {
                colorName = "Blue-Magenta (" + c.R + "," + c.G + "," + c.B + ")";

            }
            //Blue
            else if ((c.R <= 248 && c.R >= 0) && (c.G <= 248 && c.G >= 0) && (c.B <= 255 && c.B >= 102))
            {
                colorName = "Blue (" + c.R + "," + c.G + "," + c.B + ")";

            }
            //Cyan-Blue
            else if ((c.R <= 240 && c.R >= 0) && (c.G <= 248 && c.G >= 33) && (c.B <= 255 && c.B >= 71))
            {
                colorName = "Cyan-Blue (" + c.R + "," + c.G + "," + c.B + ")";

            }
            //Cyan
            else if ((c.R <= 231 && c.R >= 0) && (c.G <= 255 && c.G >= 79) && (c.B <= 255 && c.B >= 79))
            {
                colorName = "Cyan (" + c.R + "," + c.G + "," + c.B + ")";

            }
            //Green-Cyan
            else if ((c.R <= 178 && c.R >= 0) && (c.G <= 255 && c.G >= 36) && (c.B <= 212 && c.B >= 32))
            {
                colorName = "Green-Cyan (" + c.R + "," + c.G + "," + c.B + ")";

            }
            //Green
            else if ((c.R <= 178 && c.R >= 0) && (c.G <= 255 && c.G >= 72) && (c.B <= 175 && c.B >= 0))
            {
                colorName = "Green (" + c.R + "," + c.G + "," + c.B + ")";

            }
            //Yellow-Green
            else if ((c.R <= 223 && c.R >= 75) && (c.G <= 255 && c.G >= 83) && (c.B <= 130 && c.B >= 0))
            {
                colorName = "Yellow-Green (" + c.R + "," + c.G + "," + c.B + ")";

            }
            //Yellow
            else if ((c.R <= 255 && c.R >= 181) && (c.G <= 255 && c.G >= 166) && (c.B <= 240 && c.B >= 0))
            {
                colorName = "Yellow (" + c.R + "," + c.G + "," + c.B + ")";

            }
            //Orange-Yellow

            else if ((c.R <= 255 && c.R >= 145) && (c.G <= 248 && c.G >= 113) && (c.B <= 226 && c.B >= 0))
            {
                colorName = "Orange-Yellow (" + c.R + "," + c.G + "," + c.B + ")";

            }
            //Orange & Brown
            else if ((c.R <= 255 && c.R >= 61) && (c.G <= 245 && c.G >= 43) && (c.B <= 238 && c.B >= 0))
            {
                colorName = "Orange & Brown (" + c.R + "," + c.G + "," + c.B + ")";

            }
            //Red-Orange
            else if ((c.R <= 255 && c.R >= 109) && (c.G <= 153 && c.G >= 51) && (c.B <= 123 && c.B >= 0))
            {
                colorName = "Red-Orange (" + c.R + "," + c.G + "," + c.B + ")";

            }
            //Red
            else if ((c.R <= 255 && c.R >= 50) && (c.G <= 250 && c.G >= 0) && (c.B <= 250 && c.B >= 0))
            {
                colorName = "Red (" + c.R + "," + c.G + "," + c.B + ")";
            }
            //Uknown - Color can't be found [DEBUG]
            else
            {
                colorName = "Unkown (" + c.R + "," + c.G + "," + c.B + ")";

            }


            return colorName;
        }
        #endregion
        #region HSV
        /*
        Using HSV was better, and the right way in my opinion, find the values of colors in a less 
        complicated way.
        */
        private string NameOfColorHSV(Color c)
        {
            //Determines the name of the color based on it's HSV value.
            string colorName = null;

            int max = Math.Max(c.R, Math.Max(c.G, c.B));
            int min = Math.Min(c.R, Math.Min(c.G, c.B));

            var Hue = Math.Round(c.GetHue(), 2);
            var Saturation = ((max == 0) ? 0 : 1d - (1d * min / max)) * 100;
            Saturation = Math.Round(Saturation, 2);
            var Value = Math.Round(((max / 255d) * 100), 2);

            //The HSV model distributes colors in a 360(DEGREE) circle where all the colors are distributed
            //into a 60(DEGREE) slices. Value and saturation determine the brightness and 

            //White
            if (Value == 100 && Saturation < 5)
            {
                colorName = "White (" + Hue + "," + Saturation + "," + Value + ")";

            }
            else if (Value == 0 && Saturation < 95)
            {
                colorName = "Black (" + Hue + "," + Saturation + "," + Value + ")";

            }
            //Pink-Red
            else if (Hue >= 346 && Hue <= 355)
            {
                colorName = "Pink-Red (" + Hue + "," + Saturation + "," + Value + ")";

            }
            //Pink
            else if (Hue >= 331 && Hue <= 345)
            {
                colorName = "Pink (" + Hue + "," + Saturation + "," + Value + ")";

            }
            //Magenta-Pink
            else if (Hue >= 321 && Hue <= 330)
            {
                colorName = "Magenta-Pink (" + Hue + "," + Saturation + "," + Value + ")";

            }
            //Magenta
            else if (Hue >= 281 && Hue <= 320)
            {
                colorName = "Magenta (" + Hue + "," + Saturation + "," + Value + ")";

            }
            //Blue-Magenta
            else if (Hue >= 241 && Hue <= 280)
            {
                colorName = "Blue-Magenta (" + Hue + "," + Saturation + "," + Value + ")";

            }
            //Blue
            else if (Hue >= 221 && Hue <= 240)
            {
                colorName = "Blue (" + Hue + "," + Saturation + "," + Value + ")";

            }
            //Cyan-Blue
            else if (Hue >= 201 && Hue <= 220)
            {
                colorName = "Cyan-Blue (" + Hue + "," + Saturation + "," + Value + ")";

            }
            //Cyan
            else if (Hue >= 170 && Hue <= 200)
            {
                colorName = "Cyan (" + Hue + "," + Saturation + "," + Value + ")";

            }
            //Green-Cyan
            else if (Hue >= 141 && Hue <= 169)
            {
                colorName = "Green-Cyan (" + Hue + "," + Saturation + "," + Value + ")";

            }
            //Green
            else if (Hue >= 81 && Hue <= 140)
            {
                colorName = "Green (" + Hue + "," + Saturation + "," + Value + ")";

            }
            //Yellow-Green
            else if (Hue >= 61 && Hue <= 80)
            {
                colorName = "Yellow-Green (" + Hue + "," + Saturation + "," + Value + ")";

            }
            //Yellow
            else if (Hue >= 51 && Hue <= 60)
            {
                colorName = "Yellow (" + Hue + "," + Saturation + "," + Value + ")";

            }
            //Orange-Yellow

            else if (Hue >= 41 && Hue <= 50)
            {
                colorName = "Orange-Yellow (" + Hue + "," + Saturation + "," + Value + ")";

            }
            //Orange & Brown
            else if (Hue >= 21 && Hue <= 40)
            {
                colorName = "Orange & Brown (" + Hue + "," + Saturation + "," + Value + ")";

            }
            //Red-Orange
            else if (Hue >= 11 && Hue <= 20)
            {
                colorName = "Red-Orange (" + Hue + "," + Saturation + "," + Value + ")";

            }
            //Red
            else if (Hue >= 355 || Hue <= 10)
            {
                colorName = "Red (" + Hue + "," + Saturation + "," + Value + ")";
            }
            //Uknown - Color can't be found [DEBUG]
            else
            {
                colorName = "Unkown (" + Hue + "," + Saturation + "," + Value + ")";

            }



            return colorName;
        }
        #endregion

    }
}
