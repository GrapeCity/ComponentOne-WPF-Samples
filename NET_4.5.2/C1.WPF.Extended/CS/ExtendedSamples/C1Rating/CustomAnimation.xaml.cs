using C1.WPF;
using C1.WPF.Extended;
using ExtendedSamples.C1Rating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExtendedSamples
{
    /// <summary>
    /// Interaction logic for CustomAnimation.xaml
    /// </summary>
    public partial class DemoCustomAnimation : UserControl
    {
        private IAnimationFactory customAnimationFlickerFactory = new CustomAnimationFlickerFactory();
        private IAnimationFactory customAnimationRotateFactory = new CustomAnimationRotateFactory();

        public DemoCustomAnimation()
        {
            InitializeComponent();
            InitializeComboBoxItems();
        }

        private void InitializeComboBoxItems()
        {
            animationType.ItemsSource = (Enum.GetValues(typeof(AnimationType)));
            animationType.SelectedIndex = 0;
            customAnimationType.ItemsSource = new List<string>()
            {
                "Flicker",
                "Rotate",
            };
            customAnimationType.SelectedIndex = 0;
        }

        private void animationType_SelectedIndexChanged(object sender, C1.WPF.PropertyChangedEventArgs<int> e)
        {
            if (sender != null)
            {
                C1ComboBox c = sender as C1ComboBox;
                int index = c.SelectedIndex;
                if (index < 0)
                {
                    c1Rating.AnimationType = (AnimationType)0;
                }
                else
                c1Rating.AnimationType = (AnimationType)c.SelectedIndex;
            }
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            string content = (string)rb.Content;
            if (content == "Continuous")
            {
                c1Rating.AnimationMode = (AnimationMode)0;
                customRating.AnimationMode = (AnimationMode)0;
            }
            if (content == "Synchronous")
            {
                c1Rating.AnimationMode = (AnimationMode)1;
                customRating.AnimationMode = (AnimationMode)1;
            }
            if (content == "First One")
            {
                c1Rating.AnimationOrigin = (AnimationOrigin)0;
                customRating.AnimationOrigin = (AnimationOrigin)0;
            }
            if (content == "Last One")
            {
                c1Rating.AnimationOrigin = (AnimationOrigin)1;
                customRating.AnimationOrigin = (AnimationOrigin)1;
            }
        }

        private void customAnimationType_SelectedIndexChanged(object sender, PropertyChangedEventArgs<int> e)
        {
            if (sender != null)
            {
                C1ComboBox c = sender as C1ComboBox;
                string type = (string)c.SelectedItem;
                if (type == "Flicker")
                {
                    customRating.AnimationFactory = customAnimationFlickerFactory;
                }
                if (type == "Rotate")
                {
                    customRating.AnimationFactory = customAnimationRotateFactory;
                }
            }
        }
    }
}
