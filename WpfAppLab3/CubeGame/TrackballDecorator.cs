using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Media3D;

    
namespace CubeGame.arr
{
        public class TrackballDecorator : Decorator
    {
        private Point _previousPosition2D;
        private Vector3D _previousPosition3D = new Vector3D(0, 0, 1);

        private Transform3DGroup _transform;
        private RotateTransform3D _rotate;
        private AxisAngleRotation3D _rotation;

        private Viewport3D _viewport;

        public TrackballDecorator()
        {
            _rotation = new AxisAngleRotation3D(new Vector3D(0, 1, 0), 0);
            _rotate = new RotateTransform3D(_rotation);
            _transform = new Transform3DGroup();
            _transform.Children.Add(_rotate);
            this.Transform = _transform;

            this.PreviewMouseDown += OnMouseDown;
            this.PreviewMouseMove += OnMouseMove;
        }

        public Transform3D Transform { get; private set; }

        protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
        {
            base.OnVisualChildrenChanged(visualAdded, visualRemoved);
            if (visualAdded is Viewport3D vp)
            {
                _viewport = vp;
                ApplyTransform();
            }
        }

        private void ApplyTransform()
        {
            if (_viewport == null) return;

            foreach (var child in _viewport.Children)
            {
                if (child is ModelVisual3D model)
                {
                    model.Transform = this.Transform;
                }
            }
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            _previousPosition2D = e.GetPosition(this);
            _previousPosition3D = ProjectToTrackball(this.ActualWidth, this.ActualHeight, _previousPosition2D);
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed) return;

            Point currentPosition = e.GetPosition(this);
            Vector3D currentPosition3D = ProjectToTrackball(this.ActualWidth, this.ActualHeight, currentPosition);

            Vector3D axis = Vector3D.CrossProduct(_previousPosition3D, currentPosition3D);
            double angle = Vector3D.AngleBetween(_previousPosition3D, currentPosition3D);

            if (axis.Length == 0) return;

            AxisAngleRotation3D rotation = new AxisAngleRotation3D(axis, angle);
            RotateTransform3D rotate = new RotateTransform3D(rotation);

            _transform.Children.Insert(0, rotate);
            ApplyTransform();

            _previousPosition2D = currentPosition;
            _previousPosition3D = currentPosition3D;
        }

        private Vector3D ProjectToTrackball(double width, double height, Point point)
        {
            double x = (point.X - width / 2) / (width / 2);
            double y = -(point.Y - height / 2) / (height / 2);

            double z2 = 1 - x * x - y * y;
            double z = z2 > 0 ? Math.Sqrt(z2) : 0;

            return new Vector3D(x, y, z);
        }
    }
}
