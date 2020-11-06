using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MainTestApplication
{
    /// <summary>
    /// Cell that shows a rating value as an image with stars.
    /// </summary>
    public class RatingCell : StackPanel
    {
        static ImageSource _star;
        const int MAXRATING = 5;
        const double OFF = 0.2;
        const double ON = 1.0;

        /// <summary>
        /// Identifies the <see cref="ItemsSource"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RatingProperty =
            DependencyProperty.Register(
                "Rating",
                typeof(int),
                typeof(RatingCell),
                new PropertyMetadata(0, OnRatingChanged));

        public RatingCell()
        {
            if (_star == null)
            {
                _star = ImageCell.GetImageSource("star.png");
            }
            Orientation = Orientation.Horizontal;
            for (int i = 0; i < 5; i++)
            {
                var img = GetStarImage();
                img.Opacity = OFF;
                img.MouseLeftButtonDown += img_MouseLeftButtonDown;
                Children.Add(img);
            }
        }
        public int Rating
        {
            get { return (int)GetValue(RatingProperty); }
            set { SetValue(RatingProperty, value); }
        }
        void img_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // calculate rating based on the index of the star
            Image img = sender as Image;
            RatingCell cell = img.Parent as RatingCell;
            int index = cell.Children.IndexOf(img);
            if (index > 0 || e.GetPosition(img).X > img.Width / 3)
            {
                index++;
            }

            // apply the new rating
            cell.Rating = index;
            Animate(img);
        }
        static Image GetStarImage()
        {
            var img = new Image();
            img.Source = _star;
            img.Width = img.Height = 17;
            img.Stretch = Stretch.None;
            return img;
        }
        static void OnRatingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RatingCell cell = (RatingCell)d;
            for (int i = 0; i < cell.Children.Count; i++)
            {
                cell.Children[i].Opacity = i < cell.Rating ? ON : OFF;
            }
        }

        static Image _animate;
        static Storyboard _sb;
        void Animate(Image img)
        {
            // create storyboard
            if (_sb == null)
            {
                _sb = new Storyboard();
                _sb.Duration = new Duration(TimeSpan.FromMilliseconds(20));
                _sb.Completed += sb_Completed;
            }

            // suspend current animation if any
            if (_animate != null)
            {
                _sb.Stop();
                _animate.RenderTransform = null;
            }

            // start new animation
            img.RenderTransform = new ScaleTransform(2, 2, img.Width / 2, img.Height / 2);
            _animate = img;
            _sb.Begin();
        }

        void sb_Completed(object sender, EventArgs e)
        {
            var st = _animate.RenderTransform as ScaleTransform;
            if (st != null)
            {
                if (st.ScaleX > 1)
                {
                    st.ScaleX -= .1;
                    st.ScaleY -= .1;
                    _sb.Begin();
                }
                return;
            }
        }
    }
}
