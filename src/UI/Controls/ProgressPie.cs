﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace JuliusSweetland.OptiKey.UI.Controls
{
    public class ProgressPie : Canvas
    {
        public ProgressPie()
        {
            SizeChanged += (sender, args) => Render();
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof (double), typeof (ProgressPie),
                new PropertyMetadata(default(double), PropertyChangedCallback));

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(double), typeof(ProgressPie),
                new PropertyMetadata(100.0, PropertyChangedCallback));

        public double MaxValue
        {
            get { return (double)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        public static readonly DependencyProperty FillProperty =
            DependencyProperty.Register("Fill", typeof (Brush), typeof (ProgressPie),
                new PropertyMetadata(default(Brush), PropertyChangedCallback));

        public Brush Fill
        {
            get { return (Brush) GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }

        private static void PropertyChangedCallback(
            DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var progressPie = dependencyObject as ProgressPie;
            if (progressPie != null)
            {
                progressPie.Render();
            }
        }

        private void Render()
        {
            var angle = Value * 360 / MaxValue;
            var centreX = ActualWidth / 2;
            var centreY = ActualHeight / 2;
            var radius = Math.Min(centreX, centreY);

            Children.Clear();

            Path piePath;

            if (angle >= 360)
            {
                piePath = new Path
                {
                    Fill = this.Fill,
                    //Stroke = this.Stroke,
                    //StrokeThickness = 1,
                    Data = new GeometryGroup
                    {
                        //FillRule = FillRule.EvenOdd,
                        Children = new GeometryCollection
                        {
                            new EllipseGeometry
                            {
                                Center = new Point(centreX, centreY),
                                RadiusX = radius,
                                RadiusY = radius
                            }
                        }
                    }
                };
            }
            else
            {
                var innerArcStartPoint = OffsetEx(ComputeCartesianCoordinate(0, 0), centreX, centreY);
                var innerArcEndPoint = OffsetEx(ComputeCartesianCoordinate(angle, 0), centreX, centreY);
                var outerArcStartPoint = OffsetEx(ComputeCartesianCoordinate(0, radius), centreX, centreY);
                var outerArcEndPoint = OffsetEx(ComputeCartesianCoordinate(angle, radius), centreX, centreY);

                bool largeArc = angle > 180.0;
                var outerArcSize = new Size(radius, radius);
                var innerArcSize = new Size(0, 0);

                piePath = new Path
                {
                    Fill = this.Fill,
                    //Stroke = this.Stroke,
                    //StrokeThickness = 1,
                    Data = new PathGeometry
                    {
                        Figures = new PathFigureCollection
                        {
                            new PathFigure
                            {
                                StartPoint = innerArcStartPoint,
                                Segments = new PathSegmentCollection
                                {
                                    new LineSegment
                                    {
                                        Point = outerArcStartPoint
                                    },
                                    new ArcSegment
                                    {
                                        Point = outerArcEndPoint,
                                        Size = outerArcSize,
                                        IsLargeArc = largeArc,
                                        SweepDirection = SweepDirection.Clockwise,
                                        RotationAngle = 0
                                    },
                                    new LineSegment
                                    {
                                        Point = innerArcEndPoint
                                    },
                                    new ArcSegment
                                    {
                                        Point = innerArcStartPoint,
                                        Size = innerArcSize,
                                        IsLargeArc = largeArc,
                                        SweepDirection = SweepDirection.Counterclockwise,
                                        RotationAngle = 0
                                    }
                                }
                            }
                        }
                    }
                };
            }
            
            Children.Add(piePath);
        }

        /// <summary>
        /// Converts a coordinate from the polar coordinate system to the cartesian coordinate system.
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        private Point ComputeCartesianCoordinate(double angle, double radius)
        {
            // convert to radians
            double angleRad = (Math.PI / 180.0) * (angle - 90);

            double x = radius * Math.Cos(angleRad);
            double y = radius * Math.Sin(angleRad);

            return new Point(x, y);
        }

        private Point OffsetEx(Point point, double x, double y)
        {
            return new Point(point.X + x, point.Y + y);
        }
    }
}
