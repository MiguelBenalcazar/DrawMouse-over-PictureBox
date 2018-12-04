using OpenCvSharp;
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
