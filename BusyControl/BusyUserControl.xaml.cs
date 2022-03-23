using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace BusyControl
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class BusyUserControl : UserControl, INotifyPropertyChanged
    {
		public enum BusyType
		{
			Unknown,
			Spinner,
			Dots,
			Bars,
			Arc
		}
        public BusyUserControl()
        {
            InitializeComponent();
		}

		private void SetDynamicVariables()
		{
			Resources["PP"] = new Point(1.5 * SpRadius, SpRadius * (1 - Math.Sin(Math.PI / 3))); 
			Resources["SP"] = new Point(SpRadius, 0);
			Resources["SZ"] = new Size(SpRadius, SpRadius);

			Resources["CX"] = (double)SpRadius;
			Resources["CY"] = (double)SpRadius;
		}

		public static readonly DependencyProperty SpColorProperty = DependencyProperty.Register("SpColor", typeof(Brush), typeof(BusyUserControl), new PropertyMetadata(ColorChanged));
		private static void ColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var ctrl = (BusyUserControl)d;

			if (ctrl != null)
			{
				var b = (Brush)e.NewValue;
				ctrl.SetSpinColor(b);
			}
		}
		public Brush SpColor
		{
			get { return (Brush)GetValue(SpColorProperty); }
			set { SetValue(SpColorProperty, value); }
		}

		public static readonly DependencyProperty SpIsBusyProperty = DependencyProperty.Register("SpIsBusy", typeof(bool), typeof(BusyUserControl), new PropertyMetadata(true));
		public bool SpIsBusy
		{
			get { return (bool)GetValue(SpIsBusyProperty); }
			set { SetValue(SpIsBusyProperty, value); }
		}

		public static readonly DependencyProperty SpSpeedProperty = DependencyProperty.Register("SpSpeed", typeof(int), typeof(BusyUserControl), new PropertyMetadata(6, SpeedChanged));
		private static void SpeedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var ctrl = (BusyUserControl)d;

			if (ctrl != null)
				ctrl.SpSpeed = (int)e.NewValue;
		}

		public int SpSpeed
		{
			get { return (int)GetValue(SpSpeedProperty); }
			set { SetValue(SpSpeedProperty, value); }
		}
		public static readonly DependencyProperty SpSpRadiusProperty = DependencyProperty.Register("SpRadius", typeof(int), typeof(BusyUserControl), new PropertyMetadata(50, RadiusChanged));

		private static void RadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var ctrl = (BusyUserControl)d;

			if (ctrl != null)
				ctrl.SetSpinRadius((int)e.NewValue);
		}
		public int SpRadius
		{
			get { return (int)GetValue(SpSpRadiusProperty); }
			set { SetValue(SpSpRadiusProperty, value); }
		}
		public static readonly DependencyProperty SpTypeProperty = DependencyProperty.Register("SpType", typeof(BusyType), typeof(BusyUserControl), new PropertyMetadata(BusyType.Unknown, TypeChanged));
		private static void TypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var ctrl = (BusyUserControl)d;

			if (ctrl != null)
			{
				if (ctrl.SpType == BusyType.Dots)
					ctrl.DotsAnimation();
				else if (ctrl.SpType == BusyType.Spinner)
					ctrl.SpinAnimation();
				else if (ctrl.SpType == BusyType.Bars)
					ctrl.BarsAnimation();
				else if (ctrl.SpType == BusyType.Arc)
				{
					ctrl.SetDynamicVariables();
					ctrl.ArcAnimation();
				}
					

				ctrl.frameAnim.Render();
			}
		}
		public BusyType SpType
		{
			get { return (BusyType)GetValue(SpTypeProperty); }
			set { SetValue(SpTypeProperty, value); }
		}
		private void SpinAnimation()
		{
			KeyTime time = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0));
			int numFrames = spinnerCanvas.Children.Count;
			SetSpinRadius(SpRadius);

			frameAnim.Duration = new Duration(TimeSpan.FromSeconds(numFrames));

			for (int i = 0; i < numFrames; i++)
			{
				Animation.Frame f = new Animation.Frame();
				frameAnim.Frames.Add(f);

				var hr = (int)(-0.5 * SpRadius) + (int)(SpRadius*0.2);
				var hl = (int)(-0.5 * SpRadius);

				f.KeyTime = time;
				time = KeyTime.FromTimeSpan(time.TimeSpan + TimeSpan.FromSeconds(1)); // one frame per second (we can speed this up with SpeedRatio)

				// Line size:
				f.Add(new Setter(Line.Y2Property, hl.ToString(), ((Line)spinnerCanvas.Children[(i + numFrames - 1) % numFrames]).Name));
				f.Add(new Setter(Line.Y2Property, hr.ToString(), ((Line)spinnerCanvas.Children[(i) % numFrames]).Name));
				f.Add(new Setter(Line.Y2Property, hl.ToString(), ((Line)spinnerCanvas.Children[(i + 1) % numFrames]).Name));

				// Line opacity:
				f.Add(new Setter(OpacityProperty, "0.2", ((Line)spinnerCanvas.Children[(i + numFrames - 4) % numFrames]).Name));
				f.Add(new Setter(OpacityProperty, "1", ((Line)spinnerCanvas.Children[(i) % numFrames]).Name));
				f.Add(new Setter(OpacityProperty, "0.5", ((Line)spinnerCanvas.Children[(i + 1) % numFrames]).Name));
			}
		}
		private void SetSpinRadius(int r)
		{
			foreach (var item in spinnerCanvas.Children)
			{
				((Line)item).Y2 = (int)(-0.5 * r);
				((Line)item).Y1 = (int)(-0.5 * r) + (int)(r * 0.2);
			}
		}
		private void SetSpinColor(Brush b)
		{
			foreach (var item in spinnerCanvas.Children)
			{
				((Line)item).Stroke = b;
			}
		}
		private void DotsAnimation()
		{
			KeyTime time = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0));
			int numFrames = dotsCanvas.Children.Count;
			var rads = (SpRadius * 0.2).ToString();
			var radsx2 = (2 * SpRadius * 0.2).ToString();
			frameAnim.Duration = new Duration(TimeSpan.FromSeconds(numFrames));

			for (int i = 0; i < numFrames; i++)
			{
				Animation.Frame f = new Animation.Frame();
				frameAnim.Frames.Add(f);

				f.KeyTime = time;
				time = KeyTime.FromTimeSpan(time.TimeSpan + TimeSpan.FromSeconds(1)); // one frame per second (we can speed this up with SpeedRatio)

				// Circle size:
				f.Add(new Setter(WidthProperty, rads, ((Ellipse)dotsCanvas.Children[(i) % numFrames]).Name));
				f.Add(new Setter(HeightProperty, rads, ((Ellipse)dotsCanvas.Children[(i) % numFrames]).Name));
				f.Add(new Setter(OpacityProperty, "0.4", ((Ellipse)dotsCanvas.Children[(i) % numFrames]).Name));

				f.Add(new Setter(WidthProperty, radsx2, ((Ellipse)dotsCanvas.Children[(i + 1) % numFrames]).Name));
				f.Add(new Setter(HeightProperty, radsx2, ((Ellipse)dotsCanvas.Children[(i + 1) % numFrames]).Name));
				f.Add(new Setter(OpacityProperty, "1", ((Ellipse)dotsCanvas.Children[(i + 1) % numFrames]).Name));

				f.Add(new Setter(WidthProperty, rads, ((Ellipse)dotsCanvas.Children[(i + 2) % numFrames]).Name));
				f.Add(new Setter(HeightProperty, rads, ((Ellipse)dotsCanvas.Children[(i + 2) % numFrames]).Name));
				f.Add(new Setter(OpacityProperty, "0.4", ((Ellipse)dotsCanvas.Children[(i + 2) % numFrames]).Name));
			}
		}
		private void BarsAnimation()
		{
			KeyTime time = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0));
			int numFrames = barsCanvas.Children.Count;
			var rads = (SpRadius * 0.1).ToString();
			var radsx2 = (SpRadius * 0.8).ToString();
			frameAnim.Duration = new Duration(TimeSpan.FromSeconds(numFrames));

			for (int i = 0; i < numFrames; i++)
			{
				Animation.Frame f = new Animation.Frame();
				frameAnim.Frames.Add(f);

				f.KeyTime = time;
				time = KeyTime.FromTimeSpan(time.TimeSpan + TimeSpan.FromSeconds(1)); // one frame per second (we can speed this up with SpeedRatio)

				// Circle size:
				f.Add(new Setter(HeightProperty, rads, ((Rectangle)barsCanvas.Children[(i) % numFrames]).Name));
				f.Add(new Setter(OpacityProperty, "0.4", ((Rectangle)barsCanvas.Children[(i) % numFrames]).Name));

				f.Add(new Setter(HeightProperty, radsx2, ((Rectangle)barsCanvas.Children[(i+1) % numFrames]).Name));
				f.Add(new Setter(OpacityProperty, "1", ((Rectangle)barsCanvas.Children[(i+1) % numFrames]).Name));

				f.Add(new Setter(HeightProperty, rads, ((Rectangle)barsCanvas.Children[(i+2) % numFrames]).Name));
				f.Add(new Setter(OpacityProperty, "0.4", ((Rectangle)barsCanvas.Children[(i+2) % numFrames]).Name));
			}
		}
		private void ArcAnimation()
		{
			KeyTime time = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0));
			int numFrames = arcCanvas.Children.Count;
			frameAnim.Duration = new Duration(TimeSpan.FromSeconds(numFrames));
			
			for (int i = 0; i < numFrames; i++)
			{
				Animation.Frame f = new Animation.Frame();
				frameAnim.Frames.Add(f);

				f.KeyTime = time;
				time = KeyTime.FromTimeSpan(time.TimeSpan + TimeSpan.FromSeconds(1)); // one frame per second (we can speed this up with SpeedRatio)
				
				f.Add(new Setter(OpacityProperty, "1", ((Path)arcCanvas.Children[(i) % numFrames]).Name));
				f.Add(new Setter(OpacityProperty, "0", ((Path)arcCanvas.Children[(i + 1) % numFrames]).Name));
			}
		}
		public event PropertyChangedEventHandler PropertyChanged;
	}
}
