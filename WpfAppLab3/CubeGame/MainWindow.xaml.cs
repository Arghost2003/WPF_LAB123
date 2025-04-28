using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Input;
using System.Windows.Controls;

namespace CubeGame
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            AddCubesToScene();
        }

        private void AddCubesToScene()
        {
            var group = new Model3DGroup();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    GeometryModel3D cube = CreateCube(x * 2, y * 2, 0);
                    group.Children.Add(cube);
                }
            }

            var model = new ModelVisual3D { Content = group };
            viewport.Children.Add(model);
        }

        private GeometryModel3D CreateCube(double x, double y, double z)
        {
            MeshGeometry3D mesh = new MeshGeometry3D();
            double size = 0.9;

            Point3D[] pts = new Point3D[]
            {
                new Point3D(x - size, y - size, z + size),
                new Point3D(x + size, y - size, z + size),
                new Point3D(x + size, y + size, z + size),
                new Point3D(x - size, y + size, z + size),
                new Point3D(x - size, y - size, z - size),
                new Point3D(x + size, y - size, z - size),
                new Point3D(x + size, y + size, z - size),
                new Point3D(x - size, y + size, z - size)
            };

            int[] indices =
            {
                0,1,2, 0,2,3,
                1,5,6, 1,6,2,
                5,4,7, 5,7,6,
                4,0,3, 4,3,7,
                3,2,6, 3,6,7,
                4,5,1, 4,1,0
            };

            foreach (int i in indices)
                mesh.Positions.Add(pts[i]);

            var material = new DiffuseMaterial(new SolidColorBrush(Colors.SkyBlue));
            GeometryModel3D cube = new GeometryModel3D(mesh, material);
            cube.BackMaterial = material;

            return cube;
        }
    }
}
