using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp;

namespace DrawingByMouse
{
    public partial class Form1 : Form
    {
        System.Drawing.Color color = new System.Drawing.Color();
        int pen_size = new int();
        float x1, x2, y1, y2;

        System.Drawing.Point mDown = System.Drawing.Point.Empty;
        System.Drawing.Point mCurrent = System.Drawing.Point.Empty;
        Bitmap bitmap;
        Bitmap a,m,empty;
        Pen pen = new Pen(Brushes.Red);
        Graphics g;
        C_lmage c_img = new C_lmage();
        Boolean down = new Boolean();
        Mat img_;
        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            //mDown = System.Drawing.Point.Empty;
            down = false;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            mDown = e.Location;
           mDown = TranslateStretchImageMousePosition(mDown);
            down = true;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            long bytes1 = GC.GetTotalMemory(false);
            if (e.Button == System.Windows.Forms.MouseButtons.Left && down)
            {
                
                pen.Width = pen_size;
                pen.Color = color;
                //set the linejoin (not needed, but gives better results)
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;

                using (g = Graphics.FromImage(bitmap))
                {
                     
                    Graphics dd = Graphics.FromImage(empty);
                
                    g.DrawImage(a, 0, 0);
                    mCurrent = e.Location;
                    mCurrent = TranslateStretchImageMousePosition(mCurrent);
                    g.DrawLine(pen, mDown, mCurrent);//
                    
                    dd.DrawImage(m, 0, 0);
                    dd.DrawLine(pen, mDown, mCurrent);
                    
                    g.SmoothingMode = SmoothingMode.HighQuality;
                 
                }



                m = empty;
                pictureBox1.Invalidate();
                a = bitmap;
                pictureBox1.Image = bitmap;


                mDown = e.Location;
               mDown = TranslateStretchImageMousePosition(mDown);

            }
            if (e.Button == System.Windows.Forms.MouseButtons.Right && down)
            {
                mCurrent = e.Location;
                mCurrent = TranslateStretchImageMousePosition(mCurrent);
                pen.Width = 2;
                //set the linejoin (not needed, but gives better results)
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;

                using (g = Graphics.FromImage(bitmap))
                {


                    g.Clear(Color.Red);
                }




                pictureBox1.Invalidate();
                a = bitmap;
                pictureBox1.Image = bitmap;


                mDown = e.Location;

            }
        }
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            //if (mDown != Point.Empty)
            //    e.Graphics.DrawLine(Pens.White, mDown, mCurrent);
            
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)

            {
                pictureBox1.Image = null;

                Invalidate();

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Mat aa = C_lmage.BitmapToMat(bitmap);
            Mat bb= C_lmage.BitmapToMat(empty);
            aa = C_lmage.C_Image_Resize(aa, img_.Width, img_.Height);
            Cv2.ImWrite("C:\\Users\\Malky_PC\\Pictures\\test\\test.jpg", aa);
            Cv2.ImWrite("C:\\Users\\Malky_PC\\Pictures\\test\\test_.png", bb);
        }

        private void button4_Click(object sender, EventArgs e)
        {

           if(colorDialog1.ShowDialog() == DialogResult.OK)
            {
                color = colorDialog1.Color;
                Console.WriteLine("color -> " + color.ToString());
            }
            
           
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            pen_size = int.Parse(textBox1.Text);     
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Open Image";
                dlg.Filter = "bmp files (*.bmp)|*.bmp";

                if (dlg.ShowDialog() == DialogResult.OK)
                {

                    img_= C_lmage.C_Image_OpenImage(dlg.FileName,1);
                    //Mat img = C_lmage.C_Image_Resize(img_, pictureBox1.Width, pictureBox1.Height);
                    Bitmap gg = C_lmage.MatToBitmap(img_);
                    pictureBox1.Image = gg;
                    a = new Bitmap(gg);//bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                    bitmap = new Bitmap(a.Width, a.Height);
                    m = new Bitmap(img_.Width, img_.Height);
                    empty = new Bitmap(img_.Width, img_.Height);
                    //bitmap = new Bitmap(dlg.FileName);
                }
            }
        }

        protected System.Drawing.Point TranslateStretchImageMousePosition(System.Drawing.Point coordinates)
        {
            float x = (float)(pictureBox1.Image.Width / pictureBox1.ClientSize.Width);
            float y = (float)(pictureBox1.Image.Height / pictureBox1.ClientSize.Height);

            Console.WriteLine(" pictureBox image width -> " + pictureBox1.Image.Width.ToString() 
                + " pictureBox image client -> " + pictureBox1.ClientSize.Width.ToString()+" "+x.ToString());
            Console.WriteLine(" pictureBox image height -> " + pictureBox1.Image.Height.ToString()
                + " pictureBox image client -> " + pictureBox1.ClientSize.Height.ToString() + " " + y.ToString());

            return new System.Drawing.Point((int)(coordinates.X*x), (int)(coordinates.Y*y));

            //// test to make sure our image is not null
            //if (img_ == null) return coordinates;
            //// Make sure our control width and height are not 0
            //if (Width == 0 || Height == 0) return coordinates;
            //// First, get the ratio (image to control) the height and width
            //float ratioWidth = (float)img_.Width / Width;
            //float ratioHeight = (float)img_.Height / Height;
            //// Scale the points by our ratio
            //float newX = coordinates.X;
            //float newY = coordinates.Y;
            //newX *= ratioWidth;
            //newY *= ratioHeight;
            //return new System.Drawing.Point((int)newX, (int)newY);
        }
        protected float[] Position(System.Drawing.Point coordinates)
        {
            float[] coordinates_ = new float[2];
            coordinates_[0] = (float)pictureBox1.Image.Width / (float)pictureBox1.ClientSize.Width;  //x
            coordinates_[1] = (float)pictureBox1.Image.Height / (float)pictureBox1.ClientSize.Height; //y

            Console.WriteLine(" pictureBox image width -> " + pictureBox1.Image.Width.ToString()
                + " pictureBox image client -> " + pictureBox1.ClientSize.Width.ToString() + " " + coordinates_[0]);
            Console.WriteLine(" pictureBox image height -> " + pictureBox1.Image.Height.ToString()
                + " pictureBox image client -> " + pictureBox1.ClientSize.Height.ToString() + " " + coordinates_[1].ToString());

            return coordinates_;

            //// test to make sure our image is not null
            //if (img_ == null) return coordinates;
            //// Make sure our control width and height are not 0
            //if (Width == 0 || Height == 0) return coordinates;
            //// First, get the ratio (image to control) the height and width
            //float ratioWidth = (float)img_.Width / Width;
            //float ratioHeight = (float)img_.Height / Height;
            //// Scale the points by our ratio
            //float newX = coordinates.X;
            //float newY = coordinates.Y;
            //newX *= ratioWidth;
            //newY *= ratioHeight;
            //return new System.Drawing.Point((int)newX, (int)newY);
        }
    }
}
