using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using C1.WPF.Core;

namespace GridShowCase
{
    /// <summary>
    /// Cell that shows a rating value as an image with stars.
    /// </summary>
    public class RatingCell : StackPanel
    {
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

        public event EventHandler<MouseButtonEventArgs> RatingTapped;

        void img_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // calculate rating based on the index of the star
            C1Icon img = sender as C1Icon;
            RatingCell cell = img.Parent as RatingCell;
            int index = cell.Children.IndexOf(img);
            if (index > 0 || e.GetPosition(img).X > img.Width / 3)
            {
                index++;
            }

            // apply the new rating
            cell.Rating = index;
            cell.OnRatingTapped(sender, e);
        }
        static C1Icon GetStarImage()
        {
            var starIcon = C1IconTemplate.Star5.CreateIcon();
            starIcon.Width = 16;
            return starIcon;
        }
        static void OnRatingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RatingCell cell = (RatingCell)d;
            for (int i = 0; i < cell.Children.Count; i++)
            {
                cell.Children[i].Opacity = i < cell.Rating ? ON : OFF;
            }
        }

        private void OnRatingTapped(object sender, MouseButtonEventArgs e)
        {
            RatingTapped?.Invoke(sender, e);
        }
    }
}
