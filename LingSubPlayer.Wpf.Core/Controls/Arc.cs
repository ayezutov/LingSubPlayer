using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LingSubPlayer.Wpf.Core.Controls
{
    public class Pie : Shape
    {
        public static readonly DependencyProperty CenterXProperty = DependencyProperty.Register(
            "CenterX", typeof (double), typeof (Pie), new FrameworkPropertyMetadata(default(double), FrameworkPropertyMetadataOptions.AffectsRender));

        public double CenterX
        {
            get { return (double) GetValue(CenterXProperty); }
            set { SetValue(CenterXProperty, value); }
        }

        public static readonly DependencyProperty CenterYProperty = DependencyProperty.Register(
            "CenterY", typeof(double), typeof(Pie), new FrameworkPropertyMetadata(default(double), FrameworkPropertyMetadataOptions.AffectsRender));

        public double CenterY
        {
            get { return (double) GetValue(CenterYProperty); }
            set { SetValue(CenterYProperty, value); }
        }

        public static readonly DependencyProperty RadiusProperty = DependencyProperty.Register(
            "Radius", typeof(double), typeof(Pie), new FrameworkPropertyMetadata(default(double), FrameworkPropertyMetadataOptions.AffectsRender));

        public double Radius
        {
            get { return (double) GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        public static readonly DependencyProperty RotationProperty = DependencyProperty.Register(
            "Rotation", typeof(double), typeof(Pie), new FrameworkPropertyMetadata(default(double), FrameworkPropertyMetadataOptions.AffectsRender));

        public double Rotation
        {
            get { return (double) GetValue(RotationProperty); }
            set { SetValue(RotationProperty, value); }
        }

        public static readonly DependencyProperty AngleProperty = DependencyProperty.Register(
            "Angle", typeof(double), typeof(Pie), new FrameworkPropertyMetadata(default(double), FrameworkPropertyMetadataOptions.AffectsRender));

        public double Angle
        {
            get { return (double) GetValue(AngleProperty); }
            set { SetValue(AngleProperty, value); }
        }

        public Pie()
        {
            Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#4386D8"));
            Stroke = Brushes.Black;
            StrokeThickness = 1;
        }

        public static Point ComputeCartesianCoordinate(double angle, double radius)
        {
            // convert to radians
            double angleRad = (Math.PI / 180.0) * (angle - 90);

            double x = radius * Math.Cos(angleRad);
            double y = radius * Math.Sin(angleRad);

            return new Point(x, y);
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                StreamGeometry geometry = new StreamGeometry();
                //geometry.FillRule = FillRule.EvenOdd;

                using (StreamGeometryContext context = geometry.Open())
                {
                    DrawGeometry(context);
                }

                geometry.Freeze();

                return geometry;
            }
        }

        private void DrawGeometry(StreamGeometryContext context)
        {
            if (Angle >= 360)
            {
                context.DrawGeometry(new EllipseGeometry(new Point(CenterX, CenterY), Radius, Radius), false);
                return;
            }

            Point outerArcStartPoint = ComputeCartesianCoordinate(Rotation, Radius);
            outerArcStartPoint.Offset(CenterX, CenterY);

            Point outerArcEndPoint = ComputeCartesianCoordinate(Rotation + Angle, Radius);
            outerArcEndPoint.Offset(CenterX, CenterY);

            bool largeArc = Angle > 180.0;
            Size outerArcSize = new Size(Radius, Radius);

            context.BeginFigure(outerArcStartPoint, false, false);
            context.ArcTo(outerArcEndPoint, outerArcSize, 0, largeArc, SweepDirection.Clockwise, true, true);
        }
    }

    public static class GeometryExtensions
    {
        public static void DrawGeometry(this StreamGeometryContext ctx, Geometry geo, bool isFilled)
        {
            var pathGeometry = geo as PathGeometry ?? PathGeometry.CreateFromGeometry(geo);
            foreach (var figure in pathGeometry.Figures)
                ctx.DrawFigure(figure, isFilled, true);
        }

        public static void DrawFigure(this StreamGeometryContext ctx, PathFigure figure, bool isFilled, bool isClosed)
        {
            ctx.BeginFigure(figure.StartPoint, isFilled, isClosed);
            foreach (var segment in figure.Segments)
            {
                var lineSegment = segment as LineSegment;
                if (lineSegment != null) { ctx.LineTo(lineSegment.Point, lineSegment.IsStroked, lineSegment.IsSmoothJoin); continue; }

                var bezierSegment = segment as BezierSegment;
                if (bezierSegment != null) { ctx.BezierTo(bezierSegment.Point1, bezierSegment.Point2, bezierSegment.Point3, bezierSegment.IsStroked, bezierSegment.IsSmoothJoin); continue; }

                var quadraticSegment = segment as QuadraticBezierSegment;
                if (quadraticSegment != null) { ctx.QuadraticBezierTo(quadraticSegment.Point1, quadraticSegment.Point2, quadraticSegment.IsStroked, quadraticSegment.IsSmoothJoin); continue; }

                var polyLineSegment = segment as PolyLineSegment;
                if (polyLineSegment != null) { ctx.PolyLineTo(polyLineSegment.Points, polyLineSegment.IsStroked, polyLineSegment.IsSmoothJoin); continue; }

                var polyBezierSegment = segment as PolyBezierSegment;
                if (polyBezierSegment != null) { ctx.PolyBezierTo(polyBezierSegment.Points, polyBezierSegment.IsStroked, polyBezierSegment.IsSmoothJoin); continue; }

                var polyQuadraticSegment = segment as PolyQuadraticBezierSegment;
                if (polyQuadraticSegment != null) { ctx.PolyQuadraticBezierTo(polyQuadraticSegment.Points, polyQuadraticSegment.IsStroked, polyQuadraticSegment.IsSmoothJoin); continue; }

                var arcSegment = segment as ArcSegment;
                if (arcSegment != null) { ctx.ArcTo(arcSegment.Point, arcSegment.Size, arcSegment.RotationAngle, arcSegment.IsLargeArc, arcSegment.SweepDirection, arcSegment.IsStroked, arcSegment.IsSmoothJoin); }
            }
        }
    }
}