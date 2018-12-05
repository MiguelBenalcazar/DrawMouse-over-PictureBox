using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using OpenCvSharp;

namespace DrawingByMouse
{
    public partial class Form1 : Form
    {
        bool start_Paint = new bool();

        System.Drawing.Point ini_Coord = System.Drawing.Point.Empty;
        System.Drawing.Point Current_Coord = System.Drawing.Point.Empty;
        Mat img_=new Mat();
        Mat img_aux = new Mat();
        Scalar color = Scalar.Red;
       

        public Form1()
        {
            InitializeComponent();
            textBox1.Text = Convert.ToString(10);
            button4.BackColor = Color.Red;

           panel1.AutoScroll = true;
           panel1.Controls.Add(pictureBox1);

        }

        private void pictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {

           
            
            float factor = 0;
            if (ModifierKeys.HasFlag(Keys.Alt))
            {
                if (e.Delta > 0)
                {
                    if (pictureBox1.Width < panel1.Width * 20 || pictureBox1.Height < panel1.Height * 20)
                    {
                        factor = 1.1f;
                        pictureBox1.Width = (int)(pictureBox1.Width * factor);
                        pictureBox1.Height = (int)(pictureBox1.Height * factor);
                    }
                    else
                        return;

                    
                }
                    
                else
                {
                    factor = 0.9f;
                    if (pictureBox1.Width * factor <= panel1.Width || pictureBox1.Height * factor <= panel1.Height)
                    {
                        pictureBox1.Width = panel1.Width;
                        pictureBox1.Height = panel1.Height;
                    }
                    else
                    {
                        pictureBox1.Width = (int)(pictureBox1.Width * factor);
                        pictureBox1.Height = (int)(pictureBox1.Height * factor);
                    }





                }
                #region Move PictureBox1 inside of the Panel1
                System.Drawing.Point panelcenter = new System.Drawing.Point((panel1.Width / 2), (panel1.Height / 2)); // find the centerpoint o
                System.Drawing.Point offsetinpicturebox = new System.Drawing.Point((pictureBox1.Location.X + e.Location.X), (pictureBox1.Location.Y + e.Location.Y)); // find the offset of the mouse click
                System.Drawing.Point offsetfromcenter = new System.Drawing.Point((panelcenter.X - offsetinpicturebox.X), (panelcenter.Y - offsetinpicturebox.Y)); // find the difference between the mouse click and the center
                panel1.AutoScrollPosition = new System.Drawing.Point(
                    (Math.Abs(panel1.AutoScrollPosition.X) + (-1 * offsetfromcenter.X)),
                    (Math.Abs(panel1.AutoScrollPosition.Y) + (-1 * offsetfromcenter.Y))
                );
                #endregion Move PictureBox1 inside of the Panel1
            }
            int jajaa=(panel1.AutoScrollPosition.Y) ;
      
            
            if (ModifierKeys.HasFlag(Keys.Control))
            {
                if (e.Delta > 0)
                    panel1.AutoScrollPosition = new System.Drawing.Point(-panel1.AutoScrollPosition.X + 120, jajaa);
                else
                    panel1.AutoScrollPosition = new System.Drawing.Point(-panel1.AutoScrollPosition.X-120, jajaa);
            }
            else
                return;




        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            start_Paint = false;
            ini_Coord = System.Drawing.Point.Empty;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            ini_Coord = TranslateStretchImageMousePosition(e.Location);
            start_Paint = true;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            //if (e.Button == MouseButtons.Left && start_Paint)
            //{
            //    Cv2.Line(img_aux, ini_Coord.X, ini_Coord.Y, TranslateStretchImageMousePosition(e.Location).X,
            //                                                TranslateStretchImageMousePosition(e.Location).Y, color, int.Parse(textBox1.Text), LineTypes.AntiAlias);
            //    ini_Coord = TranslateStretchImageMousePosition(e.Location);


            //    Mat dest = new Mat();
            //    Mat watershed= C_lmage.C_Image_Watershed(img_aux, img_, color);

            //    Cv2.AddWeighted(img_, 1.0, watershed, 0.9, 0.5, dest);  //Combination of two images
            //    pictureBox1.Image = C_lmage.MatToBitmap(watershed);
            //}


            if (e.Button == MouseButtons.Left && start_Paint)
            {
                Cv2.Line(img_aux, ini_Coord.X, ini_Coord.Y, TranslateStretchImageMousePosition(e.Location).X,
                                                            TranslateStretchImageMousePosition(e.Location).Y, color, int.Parse(textBox1.Text), LineTypes.AntiAlias);
                ini_Coord = TranslateStretchImageMousePosition(e.Location);
                Mat dest = new Mat();
                Cv2.AddWeighted(img_, 1.0, img_aux, 0.8, 0.0, dest);  //Combination of two images
                pictureBox1.Image = C_lmage.MatToBitmap(dest);
            }
            if (e.Button == MouseButtons.Right && start_Paint)
            {
                Cv2.Line(img_aux, ini_Coord.X, ini_Coord.Y, TranslateStretchImageMousePosition(e.Location).X,
                                                            TranslateStretchImageMousePosition(e.Location).Y, Scalar.Black, int.Parse(textBox1.Text), LineTypes.AntiAlias);
                ini_Coord = TranslateStretchImageMousePosition(e.Location);
                Mat dest = new Mat();
                Cv2.AddWeighted(img_, 1.0, img_aux, 0.8, 0.0, dest);  //Combination of two images
                pictureBox1.Image = C_lmage.MatToBitmap(dest);
            }

            GC.Collect(); //let w
        }

 



        private void button4_Click(object sender, EventArgs e)
        {
           if(colorDialog1.ShowDialog() == DialogResult.OK)
            {
                button4.BackColor = colorDialog1.Color;
              
                color = Scalar.FromRgb(colorDialog1.Color.R, colorDialog1.Color.G, colorDialog1.Color.B);
            }
                
        }



        protected System.Drawing.Point TranslateStretchImageMousePosition(System.Drawing.Point coordinates)
        {
            float x = (float)pictureBox1.Image.Width / pictureBox1.ClientSize.Width;
            float y = (float)pictureBox1.Image.Height / pictureBox1.ClientSize.Height;
            return new System.Drawing.Point((int)(coordinates.X*x), (int)(coordinates.Y*y));

        }



        private void openImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Open Image";
                dlg.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png, *.bmp) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png; *.bmp";

                if (dlg.ShowDialog() == DialogResult.OK)
                {

                    img_ = C_lmage.C_Image_OpenImage(dlg.FileName, 1);
                    pictureBox1.Image = C_lmage.MatToBitmap(img_);
                    img_aux = img_.EmptyClone();

                }
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Cv2.ImWrite("C:\\Users\\LAB01-PC\\Pictures\\test\\test.jpg", img_aux);
            Mat gray_img = new Mat();
            Cv2.CvtColor(img_aux, gray_img, ColorConversionCodes.BGR2GRAY);
            Cv2.ImWrite("C:\\Users\\LAB01-PC\\Pictures\\test\\test1.jpg", gray_img);
        }

        private void cleanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                pictureBox1.Image = null;
                Invalidate();
            }
            img_ = null;
        }

        private void zoomInToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (pictureBox1.Width < panel1.Width * 20 || pictureBox1.Height < panel1.Height * 20)
            {
                float factor = 1.1f;
                pictureBox1.Width = (int)(pictureBox1.Width * factor);
                pictureBox1.Height = (int)(pictureBox1.Height * factor);
            }
            else
                return;
        }

        private void zoomOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            float factor = 0.9f;
            if (pictureBox1.Width * factor <= panel1.Width || pictureBox1.Height * factor <= panel1.Height)
            {
                pictureBox1.Width = panel1.Width;
                pictureBox1.Height = panel1.Height;
            }
            else
            {
                pictureBox1.Width = (int)(pictureBox1.Width * factor);
                pictureBox1.Height = (int)(pictureBox1.Height * factor);
            }
        }
    }
}
