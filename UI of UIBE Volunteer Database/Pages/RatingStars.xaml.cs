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

namespace WpfApplication1.Pages
{
    /// <summary>
    /// RatingStars.xaml 的交互逻辑
    /// </summary>
    public class FivePointStar : UserControl
    {
        private double radius = 20;


        private double currentPart = 1;


        private Brush selectBackground = new SolidColorBrush(Colors.YellowGreen);


        private Brush unselectBackgroud = new SolidColorBrush(Colors.DarkGray);


        /// <summary>  
             /// 半径  
             /// </summary>  
        public double Radius
        {
            get
            {
                object result = GetValue(RadiusProperty);


                if (result == null)
                {
                    return radius;
                }


                return (double)result;
            }


            set
            {
                SetValue(RadiusProperty, value);


                this.InvalidateVisual();
            }
        }


        public static DependencyProperty RadiusProperty =
        DependencyProperty.Register("Radius", typeof(double),
        typeof(FivePointStar), new UIPropertyMetadata());


        /// <summary>  
             /// 当前是否是一颗星  
             /// </summary>  
        public double CurrentPart
        {
            get
            {
                object result = GetValue(CurrentPartProperty);


                if (result == null)
                {
                    return currentPart;
                }
                return (double)result;
            }


            set
            {
                SetValue(CurrentPartProperty, value);


                this.InvalidateVisual();
            }
        }


        public static DependencyProperty CurrentPartProperty =
        DependencyProperty.Register("CurrentPart", typeof(double),
        typeof(FivePointStar), new UIPropertyMetadata());


        /// <summary>  
             /// 选中颜色  
             /// </summary>  
        public Brush SelectBackground
        {
            get
            {
                object result = GetValue(SelectBackgroundProperty);


                if (result == null)
                {
                    return selectBackground;
                }


                return (Brush)result;
            }


            set
            {
                SetValue(SelectBackgroundProperty, value);
            }
        }


        public static DependencyProperty SelectBackgroundProperty =
        DependencyProperty.Register("SelectBackground", typeof(Brush),
        typeof(FivePointStar), new UIPropertyMetadata());


        /// <summary>  
             /// 未选中颜色  
             /// </summary>  
        public Brush UnSelectBackground
        {
            get
            {
                object result = GetValue(UnSelectBackgroundProperty);


                if (result == null)
                {
                    return unselectBackgroud;
                }


                return (Brush)result;
            }


            set
            {
                SetValue(UnSelectBackgroundProperty, value);
            }
        }


        public static DependencyProperty UnSelectBackgroundProperty =
        DependencyProperty.Register("UnSelectBackground", typeof(Brush),
        typeof(FivePointStar), new UIPropertyMetadata());




        public FivePointStar()
        : base()
        {
            this.Loaded += new RoutedEventHandler(FivePointStar_Loaded);
        }


        void FivePointStar_Loaded(object sender, RoutedEventArgs e)
        {
            this.MinHeight = Radius * 2;


            this.MaxHeight = Radius * 2;


            this.MinWidth = Radius * 2;


            this.MaxWidth = Radius * 2;


            this.Background = Brushes.Transparent;
        }


        protected override void OnRender(System.Windows.Media.DrawingContext dc)
        {
            base.OnRender(dc);


            Point center = new Point();


            PointCollection Points = GetFivePoint(center);


            Canvas ca = new Canvas();


            Polygon plg = new Polygon();


            plg.Points = Points;


            plg.Stroke = Brushes.Transparent;


            plg.StrokeThickness = 2;


            if (CurrentPart == 1)
            {
                plg.Fill = this.SelectBackground;
            }
            else
            {
                plg.Fill = this.UnSelectBackground;
            }


            plg.FillRule = FillRule.Nonzero;


            ca.Children.Add(plg);




            this.Content = ca;






            //Brush b = new SolidColorBrush(Colors.Yellow);  


            //Pen p = new Pen(b, 2);  


            //var path = new Path();  


            //var gc = new GeometryConverter();  


            //path.Data = (Geometry)gc.ConvertFromString(string.Format("M {0} {1} {2} {3} {4} Z",  
            //    Points[0], Points[1], Points[2], Points[3], Points[4]));  


            //path.Fill = Brushes.Yellow;  


            //dc.DrawGeometry(b, p, path.Data);  
        }


        /// <summary>  
             /// 根据半径和圆心确定五个点  
             /// </summary>  
             /// <param name="center"></param>  
             /// <returns></returns>  
        private PointCollection GetFivePoint(Point center)
        {
            double r = Radius;


            double h1 = r * Math.Sin(18 * Math.PI / 180);


            double h2 = r * Math.Cos(18 * Math.PI / 180);


            double h3 = r * Math.Sin(36 * Math.PI / 180);


            double h4 = r * Math.Cos(36 * Math.PI / 180);


            Point p1 = new Point(r, center.X);


            Point p2 = new Point(r - h2, r - h1);


            Point p3 = new Point(r - h3, r + h4);


            Point p4 = new Point(r + h3, p3.Y);


            Point p5 = new Point(r + h2, p2.Y);


            List<Point> values = new List<Point>() { p1, p3, p5, p2, p4 };


            PointCollection pcollect = new PointCollection(values);


            return pcollect;
        }
    }
}
