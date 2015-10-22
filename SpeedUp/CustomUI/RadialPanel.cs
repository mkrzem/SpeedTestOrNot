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

namespace SpeedUp.CustomUI
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:SpeedUp.CustomUI"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:SpeedUp.CustomUI;assembly=SpeedUp.CustomUI"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:RadialPanel/>
    ///
    /// </summary>
    public class RadialPanel : Panel
    {
        static RadialPanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RadialPanel), new FrameworkPropertyMetadata(typeof(RadialPanel)));
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            foreach (UIElement element in Children)
            {
                element.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            }

            return base.MeasureOverride(availableSize);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            double angle = 0;
            double angleIncrementStep = 0;
            double radiusX = finalSize.Width;
            double radiusY = finalSize.Height;

            if (Children.Count == 0)
            {
                return finalSize;
            }

            // Transforming to radians
            angleIncrementStep = (360 / Children.Count) * (Math.PI / 180);

            foreach (UIElement element in Children)
            {
                Point location = new Point(Math.Cos(angle) * radiusX, Math.Sin(angle) * radiusY);
                Point actualLocation = new Point(finalSize.Width / 2 + location.X - element.DesiredSize.Width / 2,
                                                     finalSize.Height / 2 + location.Y - element.DesiredSize.Height / 2);
                element.Arrange(new Rect(actualLocation.X, actualLocation.Y, element.DesiredSize.Width, element.DesiredSize.Height));

                angle += angleIncrementStep;
            }

            return base.ArrangeOverride(finalSize);
        }
    }
}
