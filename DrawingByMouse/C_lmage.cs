using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;


namespace DrawingByMouse
{
    class C_lmage
    {
        //Open Image
        public static Mat C_Image_OpenImage(string path, int type)
        {
            Mat Image = new Mat();
            switch (type)
            {
                case 1:                                                             //Case 1 return color
                    Image = new Mat(path, ImreadModes.Color);
                    break;  
                case 2:                                                             //Case 2 return GrayScale
                     Image = new Mat(path, ImreadModes.GrayScale);
                    break;
            }
            return Image;
        }
        //convert image into gray values

        public static Mat C_Image_GrayValue(Mat image)
        {
            Mat image_copy = new Mat();
            Cv2.CvtColor(image, image_copy, ColorConversionCodes.BGR2GRAY);
            return image_copy;
        }
        //Normalize
        public static Mat C_Image_Normalize(Mat image)
        {
            Mat image_copy = new Mat();
            Cv2.Normalize(image,image_copy,0,255,NormTypes.MinMax,-1,null);
            return image_copy;
        }
        //Thresholding
        public static Mat C_Image_Thresholding(Mat image, double thres)
        {
            Mat image_copy = new Mat();
          
            Cv2.Threshold(image, image_copy, thres, 255, ThresholdTypes.Binary);
            Cv2.Threshold(image_copy, image_copy, thres, 255, ThresholdTypes.Otsu);
            return image_copy;
        }
        //Mirror
        public static Mat C_Image_Mirror(Mat image)
        {
            Mat image_copy = new Mat();
            Cv2.Flip(image, image_copy, FlipMode.X);
            return image_copy;
        }
        //FLIP
        public static Mat C_Image_Flip(Mat image)
        {
            Mat image_copy = new Mat();
            Cv2.Flip(image, image_copy, FlipMode.Y);
            return image_copy;
        }

        //Zoom
        public static Mat C_Image_Resize(Mat image,int width,int height)
        {
            Mat image_copy = new Mat();
            Cv2.Resize(image, image_copy, new OpenCvSharp.Size(width, height), 0, 0, InterpolationFlags.Lanczos4);
            return image_copy;
        }
        public static Mat C_Image_Resize(Mat image,double factor)
        {
            Mat image_copy = new Mat();
            Cv2.Resize(image, image_copy, OpenCvSharp.Size.Zero, factor, factor, InterpolationFlags.Lanczos4);
            return image_copy;
        }

        //Gaussian filter .. blur
        public static Mat C_Image_Gaussian(Mat image,int size_x,int size_y,double sigma)
        {
            Mat image_copy = new Mat();
            Cv2.GaussianBlur(image, image_copy, new OpenCvSharp.Size(size_x, size_y), sigma,0,BorderTypes.Replicate);
            return image_copy;
        }
        
        //Sobel filter .. 
        public static Mat C_Image_Sobel(Mat image, int x_order, int y_order)
        {
            Mat image_copy = new Mat();
            Cv2.Sobel(image,image_copy, -1,x_order,y_order,3,1,0,BorderTypes.Default);
            return image_copy;
        }
        // https://docs.opencv.org/2.4/modules/imgproc/doc/filtering.html?highlight=sobel#sobel

        //Canny filter
        public static Mat C_Image_Canny(Mat image,double thres1)
        {
            Mat image_copy = new Mat();
            Cv2.Canny(image, image_copy, thres1, thres1, 3, true);
            //Cv2.Canny(image, image_copy, thres1, thres1*2,3,true);   
            return image_copy;
        }
        //Draw contours
        public static Mat C_Image_Draw_Contours(Mat image, Mat Origi_Image)
        {
            OpenCvSharp.Point[][] contours_data;
            HierarchyIndex[] hierarchyIndexes;
            Cv2.FindContours(image, out contours_data, out hierarchyIndexes,RetrievalModes.Tree,ContourApproximationModes.ApproxSimple);
            for(int i = 0; i < contours_data.Length; i++)
            {
                Cv2.DrawContours(Origi_Image, contours_data, i, Scalar.Red, 2, LineTypes.Link8, hierarchyIndexes);
            }
            return Origi_Image;
        }
        //watershed
       /* public static Mat C_Image_Watershed(Mat image, Mat Origi_Image, Scalar color)
        {
            var componentCount = 0;
            var rnd = new Random();
            var watershedImage=new Mat();
            OpenCvSharp.Point[][] contours_data;
            HierarchyIndex[] hierarchyIndexes;
            Mat img = C_Image_GrayValue(image);

            Cv2.FindContours(img, out contours_data, out hierarchyIndexes, RetrievalModes.CComp, ContourApproximationModes.ApproxSimple);
           
            Mat markers= new Mat(Origi_Image.Size(), MatType.CV_32S, s: Scalar.All(0));
            for (int i = 0; i < contours_data.Length; i++)
            {
                Cv2.DrawContours(markers, contours_data, i, Scalar.All(componentCount + 1), -1, LineTypes.Link8, hierarchyIndexes);
                componentCount++;
            }
            if (componentCount != 0)
            {
                var colorTable = new List<Vec3b>();
                for (var i = 0; i < componentCount; i++)
                {
                    //colorTable.Add(color.ToVec3b());
                    var b = rnd.Next(0, 255); //Cv2.TheRNG().Uniform(0, 255);
                    var g = rnd.Next(0, 255); //Cv2.TheRNG().Uniform(0, 255);
                    var r = rnd.Next(0, 255); //Cv2.TheRNG().Uniform(0, 255);

                    colorTable.Add(new Vec3b((byte)b, (byte)g, (byte)r));
                }

                Cv2.Watershed(Origi_Image, markers);

                Cv2.ImShow("test", markers);
                 watershedImage = new Mat(markers.Size(), MatType.CV_8UC3);

                // paint the watershed image
                for (var i = 0; i < markers.Rows; i++)
                {
                    for (var j = 0; j < markers.Cols; j++)
                    {
                        var idx = markers.At<int>(i, j);
                        if (idx == -1)
                        {
                            watershedImage.Set(i, j, new Vec3b(255, 255, 255));
                        }
                        else if (idx <= 0 || idx > componentCount)
                        {
                            watershedImage.Set(i, j, new Vec3b(0, 0, 0));
                        }
                        else
                        {
                            //watershedImage.Set(i, j, color.ToVec3b());
                           watershedImage.Set(i, j, colorTable[idx - 1]);
                        }
                    }
                }
               // watershedImage = watershedImage * 0.5 + Origi_Image * 0.5;
                // watershedImage = watershedImage * 0.5;
            }
            

            return watershedImage;
        }*/
       
        //Convert from Mat to Bitmap
        public static Bitmap MatToBitmap(Mat image)
        {
            return OpenCvSharp.Extensions.BitmapConverter.ToBitmap(image);
        }
        
        //Convert from Bitmap to Mat
        public static Mat BitmapToMat(Bitmap image)
        {
            return OpenCvSharp.Extensions.BitmapConverter.ToMat(image);
        }
        

    }
}
