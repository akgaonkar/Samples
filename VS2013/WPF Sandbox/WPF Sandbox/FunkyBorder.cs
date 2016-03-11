using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPF_Sandbox
{
    public class FunkyBorder : Decorator
    {
        public Brush BorderBrush
        {
            get { return (Brush)GetValue(BorderBrushProperty); }
            set { SetValue(BorderBrushProperty, value); }
        }

        public static readonly DependencyProperty BorderBrushProperty =
            DependencyProperty.Register("BorderBrush",
                                        typeof(Brush),
                                        typeof(FunkyBorder),
                                        new UIPropertyMetadata(Brushes.Transparent));

        protected override void OnRender(DrawingContext drawingContext)
        {
            // TODO, make pen thickness and corner width (currently 10) into dependency properties.
            // Also, handle case when border don't fit into given space without overlapping.

            if (_pen.Brush != BorderBrush)
            {
                _pen.Brush = BorderBrush;
            }

            drawingContext.DrawLine(_pen, new Point(0, 10), new Point(10, 0));
            drawingContext.DrawLine(_pen, new Point(10, 0), new Point(ActualWidth - 10, 0));
            drawingContext.DrawLine(_pen, new Point(ActualWidth - 10, 0), new Point(ActualWidth, 10));
            drawingContext.DrawLine(_pen, new Point(0, 10), new Point(0, ActualHeight - 10));
            drawingContext.DrawLine(_pen, new Point(ActualWidth, 10), new Point(ActualWidth, ActualHeight - 10));
            drawingContext.DrawLine(_pen, new Point(0, ActualHeight - 10), new Point(10, ActualHeight));
            drawingContext.DrawLine(_pen, new Point(10, ActualHeight), new Point(ActualWidth - 10, ActualHeight));
            drawingContext.DrawLine(_pen, new Point(ActualWidth - 10, ActualHeight), new Point(ActualWidth, ActualHeight - 10));
        }

        private Pen _pen = new Pen(Brushes.Transparent, 2);
    }
}
