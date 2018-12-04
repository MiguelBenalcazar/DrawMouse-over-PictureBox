 Graphics graphics = Graphics.FromImage(aux);
                pen.LineJoin = LineJoin.Round;
                pen.Width = Convert.ToInt16(textBox1.Text);
               
                System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Red);
                using (graphics)
                {
                    graphics.DrawImage(gg,0,0);
                    graphics.DrawLine(pen, ini_Coord.X, ini_Coord.Y, TranslateStretchImageMousePosition(e.Location).X,
                                                    TranslateStretchImageMousePosition(e.Location).Y);
                    graphics.Save();
                    graphics.CompositingQuality = CompositingQuality.GammaCorrected;
                    //Rectangle rectangle = new Rectangle(TranslateStretchImageMousePosition(e.Location).X, 
                    //    TranslateStretchImageMousePosition(e.Location).Y, Convert.ToInt16(textBox1.Text), Convert.ToInt16(textBox1.Text));
                    //graphics.FillRectangle(myBrush, new Rectangle(TranslateStretchImageMousePosition(e.Location).X,
                    //    TranslateStretchImageMousePosition(e.Location).Y, Convert.ToInt16(textBox1.Text), Convert.ToInt16(textBox1.Text)));
                }


                    //Cv2.Line(img_, ini_Coord.X, ini_Coord.Y, TranslateStretchImageMousePosition(e.Location).X,
                    //                                TranslateStretchImageMousePosition(e.Location).Y, color, int.Parse(textBox1.Text), LineTypes.AntiAlias);
                    ini_Coord = TranslateStretchImageMousePosition(e.Location);
                    pictureBox1.Image = aux;
                    gg = aux;
                    pictureBox1.Refresh();
