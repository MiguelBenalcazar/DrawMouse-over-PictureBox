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
        Mat img_;
        Scalar color = Scalar.FromRgb(0,0,0);
        Pen pen = new Pen(Brushes.Red,10);
        

        Bitmap aux;
        Bitmap gg;
        Bitmap img_aux;
   
        Graphics graphics;

        Mat mat_aux;

        public Form1()
        {
            InitializeComponent();
            textBox1.Text = Convert.ToString(10);
            button4.BackColor = Color.Red;

           panel1.AutoScroll = true;
           panel1.Controls.Add(pictureBox1);

            pen.EndCap = pen.StartCap = LineCap.Round;
        }

        private void pictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {

           
            
            float factor = 0;
            if (ModifierKeys.HasFlag(Keys.Alt))
            {
                if (e.Delta > 0)
                {
                    factor = 1.1f;
                    pictureBox1.Width = (int)(pictureBox1.Width * factor);
                    pictureBox1.Height = (int)(pictureBox1.Height * factor);
                }
                    
                else
                {
                    factor = 0.9f;
                    pictureBox1.Width = (int)(pictureBox1.Width * factor);
                    pictureBox1.Height = (int)(pictureBox1.Height * factor);
                    if (pictureBox1.Height <= panel1.Height || pictureBox1.Width <= panel1.Width)
                    {
                        pictureBox1.Width = panel1.Width;
                        pictureBox1.Height = panel1.Height;
                    }
                    else
                        return;
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
           

            System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Red);
            if (e.Button == MouseButtons.Left && start_Paint)
            {


                pen.Width = Convert.ToInt16(textBox1.Text);


                using (graphics = Graphics.FromImage(aux))
                {
                    graphics.DrawLine(pen, ini_Coord, TranslateStretchImageMousePosition(e.Location));
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    ini_Coord = TranslateStretchImageMousePosition(e.Location);

                }
                mat_aux = C_lmage.BitmapToMat(aux);  //////ERROR





                aux.MakeTransparent();

                Graphics g = Graphics.FromImage(img_aux);

                g.DrawImage(aux, 0, 0);

                pictureBox1.Image = img_aux;
                pictureBox1.Refresh();






            }
            GC.Collect();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                pictureBox1.Image = null;
                Invalidate();
            }
            img_ = null;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            Mat aux_mat = C_lmage.BitmapToMat(aux);
            Cv2.ImWrite("C:\\Users\\LAB01-PC\\Pictures\\test\\test.jpg", mat_aux);
           
        }

        private void button4_Click(object sender, EventArgs e)
        {
           if(colorDialog1.ShowDialog() == DialogResult.OK)
            {
                button4.BackColor = colorDialog1.Color;
                pen.Color = Color.FromArgb(255, colorDialog1.Color);
                color = Scalar.FromRgb(colorDialog1.Color.R, colorDialog1.Color.G, colorDialog1.Color.B);
            }
                
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
                    gg = C_lmage.MatToBitmap(img_);
                    pictureBox1.Image = gg;

                    aux = new Bitmap(img_.Width,img_.Height);

                    Graphics graphics_aux = Graphics.FromImage(aux);
   

                    graphics_aux.Clear(Color.Black);


                    img_aux = new Bitmap(gg);

                }
            }
        }

        protected System.Drawing.Point TranslateStretchImageMousePosition(System.Drawing.Point coordinates)
        {
            float x = (float)pictureBox1.Image.Width / pictureBox1.ClientSize.Width;
            float y = (float)pictureBox1.Image.Height / pictureBox1.ClientSize.Height;
            return new System.Drawing.Point((int)(coordinates.X*x), (int)(coordinates.Y*y));

        }

        private void zoom_in_Click(object sender, EventArgs e)
        {
            float factor = 1.1f;
            pictureBox1.Width = (int)(pictureBox1.Width * factor);
            pictureBox1.Height = (int)(pictureBox1.Height * factor);

        }

        private void Zoom_out_Click(object sender, EventArgs e)
        {

            float factor = 0.9f;
            pictureBox1.Width = (int)(pictureBox1.Width * factor);
            pictureBox1.Height = (int)(pictureBox1.Height * factor);
            if (pictureBox1.Height <= panel1.Height || pictureBox1.Width <= panel1.Width)
            {
                pictureBox1.Width = panel1.Width;
                pictureBox1.Height = panel1.Height;
            }
            else
                return;
        }

     
    }
}
