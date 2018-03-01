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
        private void MouseMoveTimer_Tick(object sender, EventArgs e)
        {
            
        }
        private void frmMain_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!mouseMoveTimer)
            {
                MouseMoveTimer.Start();
            }
            else
            {
                MouseMoveTimer.Stop();

            }
        }

        private void MouseMoveTimer_Tick_1(object sender, EventArgs e)
        {
            Point cursor = new Point();
            GetCursorPos(ref cursor);

            var c = GetColorAt(cursor);
            this.BackColor = c;

            if (c.R == c.G && c.G < 64 && c.B > 128)
            {
                MessageBox.Show("Blue");
            }
        }
    }
}
