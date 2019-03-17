using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace DayDaily.Controls
{
    public class ShakingTextBlock : ContentControl // Derive from ContentControl (ideally should be Control or FrameworkElement though)
    {
        private ItemsControl _itemsControl = new ItemsControl(); // What our ShakingTextBlock will set as it's Content
        private Random _random = new Random();

        public string Text // Expose a Text Dependency Property so we can set it in XAML
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(ShakingTextBlock), new UIPropertyMetadata());

        public ShakingTextBlock()
        {
            Content = _itemsControl; // set the Content of our ShakingTextBlock

            // And set up the ItemsPanelTemplate (should ideally be set from XAML in a ResourceDictionary)
            FrameworkElementFactory frameworkElementFactory = new FrameworkElementFactory(typeof(StackPanel));
            ItemsPanelTemplate itemsPanelTemplate = new ItemsPanelTemplate(frameworkElementFactory);
            frameworkElementFactory.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);
            _itemsControl.ItemsPanel = itemsPanelTemplate;

            Loaded += new RoutedEventHandler(ShakingTextBlock_Loaded);
        }

        private void ShakingTextBlock_Loaded(object sender, RoutedEventArgs e)
        {
            _itemsControl.ItemsSource = Text; // Make our ItemsControl/Content show our Text

            for (int n = 0; n < _itemsControl.Items.Count; n++) // The ItemsControl will show each letter of our text in a ContentPresenter/TextBlock container 
            {
                ContentPresenter contentPresenter = _itemsControl.ItemContainerGenerator.ContainerFromIndex(n) as ContentPresenter; // Get the container for this letter
                contentPresenter.RenderTransformOrigin = new Point(.5, .5); // what point will we rotate around?
                contentPresenter.RenderTransform = new RotateTransform(0); // what sort of RenderTransform will we apply

                TimeSpan duration = TimeSpan.FromMilliseconds(_random.Next(200) + 100); // how long does this shake for this letter last
                int maxAngle = _random.Next(10) + 10; // and how far will it travel
                DoubleAnimation animation = new DoubleAnimation(-maxAngle, maxAngle, duration); // create the animation
                animation.AutoReverse = true; // make sure it goes back and forth ..
                animation.RepeatBehavior = RepeatBehavior.Forever; // forever

                var rt = (RotateTransform)contentPresenter.RenderTransform; // get our RenderTransform (which is really a RotateTransform)
                rt.BeginAnimation(RotateTransform.AngleProperty, animation); // and ask it to start our animation!
            }
        }
    }
}
