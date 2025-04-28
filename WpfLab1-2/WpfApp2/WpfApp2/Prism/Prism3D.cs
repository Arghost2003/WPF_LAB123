using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace WpfApp2.Prism
{
    public class Prism3D : ModelVisual3D
    {
        private readonly static Brush _defaultColor = Brushes.Blue;

        public Prism3D()
        {
            DrawPrism(
                _size,
                _pos,
                _front,
                _back,
                _left,
                _right,
                _top,
                _bottom,
                _sideFront,
                _sideBack
            );
        }

        private double _size = 0.5;
        public double Size
        {
            get => _size;
            set { _size = value; UpdateModel(); }
        }

        private Point3D _pos = new Point3D(0, 0, 0);
        public Point3D Position
        {
            get => _pos;
            set { _pos = value; UpdateModel(); }
        }

        // Материалы для 8 граней
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

        // Новые свойства для дополнительных граней
        private Brush _sideFront = _defaultColor;
        public Brush SideFront
        {
            get => _sideFront;
            set { _sideFront = value; UpdateModel(); }
        }

        private Brush _sideBack = _defaultColor;
        public Brush SideBack
        {
            get => _sideBack;
            set { _sideBack = value; UpdateModel(); }
        }

        private void UpdateModel()
        {
            DrawPrism(_size, _pos, _front, _back, _left, _right, _top, _bottom, _sideFront, _sideBack);
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

        private void DrawPrism(
     double size,
     Point3D pos,
     Brush front,
     Brush back,
     Brush left,
     Brush right,
     Brush top,
     Brush bottom,
     Brush sideFront,
     Brush sideBack)
        {
            double height = size * 2;
            double halfSize = size / 2;

            // Вершины треугольной призмы
            Point3D[] vertices =
            {
        // Основания
        new Point3D(-halfSize, -halfSize, -height / 2), // 0
        new Point3D(halfSize, -halfSize, -height / 2),  // 1
        new Point3D(0, halfSize, -height / 2),          // 2
        new Point3D(-halfSize, -halfSize, height / 2),  // 3
        new Point3D(halfSize, -halfSize, height / 2),   // 4
        new Point3D(0, halfSize, height / 2)            // 5
    };

            // Применяем позицию
            for (int i = 0; i < vertices.Length; i++)
                vertices[i] = new Point3D(
                    vertices[i].X + pos.X,
                    vertices[i].Y + pos.Y,
                    vertices[i].Z + pos.Z
                );

            // Индексы граней (каждая грань - 2 треугольника)
            int[][] faces =
            {
        // Основания (2)
        new[] {0, 2, 1},  // Нижнее
        new[] {3, 4, 5},   // Верхнее

        // Боковые грани (6)
        new[] {0, 3, 5}, new[] {0, 5, 2}, // Левая
        new[] {1, 5, 4}, new[] {1, 2, 5}, // Правая
        new[] {0, 1, 4}, new[] {0, 4, 3}  // Передняя/задняя
    };

            // Определение цветов для каждой грани
            Brush[] materials =
            {
        Brushes.Red,    // Нижнее основание
        Brushes.Green,  // Верхнее основание
        Brushes.Black,   // Левая 1
        Brushes.Black, // Левая 2
        Brushes.Orange, // Правая 1
        Brushes.Purple, // Правая 2
        Brushes.Cyan,   // Передняя 1
        Brushes.Magenta  // Передняя 2
    };

            Model3DGroup model = new Model3DGroup();
            for (int i = 0; i < faces.Length; i++)
                model.Children.Add(AddFace(
                    vertices[faces[i][0]],
                    vertices[faces[i][1]],
                    vertices[faces[i][2]],
                    new DiffuseMaterial(materials[i])
                ));

            Content = model;
        }
    }
}