using System.Windows;
using System.Windows.Controls;

namespace BusyControlTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CollapsedAll();
            spin.Visibility = Visibility.Visible;
        }
        private void CollapsedAll()
        {
            spin.Visibility = Visibility.Collapsed;
            dots.Visibility = Visibility.Collapsed;
            bars.Visibility = Visibility.Collapsed;
            arc.Visibility = Visibility.Collapsed;
        }
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            var radio = (ContentControl)e.Source;
            
            if (radio.Content == null)
                return;

            var w = radio.Content.ToString().ToLower();
            CollapsedAll();

            if (w == "spin")
            {
                spin.Visibility = Visibility.Visible;
            }
            else if (w == "dots")
            {
                dots.Visibility = Visibility.Visible;
            } 
            else if (w == "bars")
            {
                bars.Visibility = Visibility.Visible;
            } 
            else if (w == "arc")
            {
                arc.Visibility = Visibility.Visible;
            }
        }
    }
}
