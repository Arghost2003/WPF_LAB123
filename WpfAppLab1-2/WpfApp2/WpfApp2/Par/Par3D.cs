using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace WpfApp2.Par
{
    public class Par3D : ModelVisual3D
    {
        private static readonly Brush _defaultColor = Brushes.Gray;

        public Par3D()
        {
            DrawPar(_sizeX, _sizeY, _sizeZ, _pos, _front, _back, _left, _right, _top, _bottom);
        }

        #region Размеры
        private double _sizeX = 1.0; // Длина (по оси X)
        private double _sizeY = 0.5; // Ширина (по оси Y)
        private double _sizeZ = 0.3; // Высота (по оси Z)

        public double SizeX
        {
            get => _sizeX;
            set { _sizeX = value; UpdateModel(); }
        }

        public double SizeY
        {
            get => _sizeY;
            set { _sizeY = value; UpdateModel(); }
        }

        public double SizeZ
        {
            get => _sizeZ;
            set { _sizeZ = value; UpdateModel(); }
        }
        #endregion

        #region Позиция и материалы
        private Point3D _pos = new Point3D(0, 0, 0);
        public Point3D Position
        {
            get => _pos;
            set { _pos = value; UpdateModel(); }
        }

        private Brush _front = _defaultColor;
        public Brush Front
        {
            get => _front;
            set { _front = value; UpdateModel(); }
        }

        private Brush _back = _defaultColor;
        public Brush Back
        {
            get => _back;
            set { _back = value; UpdateModel(); }
        }

        private Brush _left = _defaultColor;
        public Brush Left
        {
            get => _left;
            set { _left = value; UpdateModel(); }
        }

        private Brush _right = _defaultColor;
        public Brush Right
        {
            get => _right;
            set { _right = value; UpdateModel(); }
        }

        private Brush _top = _defaultColor;
        public Brush Top
        {
            get => _top;
            set { _top = value; UpdateModel(); }
        }

        private Brush _bottom = _defaultColor;
        public Brush Bottom
        {
            get => _bottom;
            set { _bottom = value; UpdateModel(); }
        }
        #endregion

        private void UpdateModel()
        {
            DrawPar(_sizeX, _sizeY, _sizeZ, _pos, _front, _back, _left, _right, _top, _bottom);
        }

        private GeometryModel3D AddFace(Point3D p1, Point3D p2, Point3D p3, Material material)
        {
            return new GeometryModel3D
            {
                Geometry = new MeshGeometry3D
                {
                    Positions = { p1, p2, p3 },
                    TriangleIndices = { 0, 1, 2 }
                },
                Material = material
            };
        }

        private void DrawPar(
            double sizeX, double sizeY, double sizeZ, // Размеры по осям X, Y, Z
            Point3D pos,
            Brush front, Brush back,
            Brush left, Brush right,
            Brush top, Brush bottom)
        {
            double halfX = sizeX / 2;
            double halfY = sizeY / 2;
            double halfZ = sizeZ / 2;

            // Вершины параллелепипеда
            Point3D[] vertices =
            {
                // Нижняя грань
                new Point3D(-halfX, -halfY, -halfZ), // 0
                new Point3D(halfX, -halfY, -halfZ),  // 1
                new Point3D(halfX, halfY, -halfZ),   // 2
                new Point3D(-halfX, halfY, -halfZ),  // 3

                // Верхняя грань
                new Point3D(-halfX, -halfY, halfZ),  // 4
                new Point3D(halfX, -halfY, halfZ),   // 5
                new Point3D(halfX, halfY, halfZ),     // 6
                new Point3D(-halfX, halfY, halfZ)     // 7
            };

            // Применяем позицию
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = new Point3D(
                    vertices[i].X + pos.X,
                    vertices[i].Y + pos.Y,
                    vertices[i].Z + pos.Z);
            }

            // Индексы граней (каждая грань - 2 треугольника)
            int[][] faces =
            {
                // Передняя грань
                new[] {4, 5, 6}, new[] {4, 6, 7},
                // Задняя грань
                new[] {0, 3, 2}, new[] {0, 2, 1},
                // Левая грань
                new[] {0, 4, 7}, new[] {0, 7, 3},
                // Правая грань
                new[] {1, 2, 6}, new[] {1, 6, 5},
                // Верхняя грань
                new[] {7, 6, 2}, new[] {7, 2, 3},
                // Нижняя грань
                new[] {0, 1, 5}, new[] {0, 5, 4}
            };

            Brush[] materials =
            {
                front, front,    // Передняя
                back, back,      // Задняя
                left, left,      // Левая
                right, right,    // Правая
                top, top,        // Верхняя
                bottom, bottom   // Нижняя
            };

            Model3DGroup model = new Model3DGroup();
            for (int i = 0; i < faces.Length; i++)
            {
                model.Children.Add(AddFace(
                    vertices[faces[i][0]],
                    vertices[faces[i][1]],
                    vertices[faces[i][2]],
                    new DiffuseMaterial(materials[i])
                ));
            }

            Content = model;
        }
    }
}