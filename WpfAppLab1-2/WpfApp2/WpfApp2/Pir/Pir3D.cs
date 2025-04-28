using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Windows.Media;

namespace WpfApp2.Pir
{
    public class Pir3D : ModelVisual3D
    {
        private readonly static Brush _defaultColor = Brushes.Red;

        public Pir3D()
        {
            DrawPir(
                _size,
                _topSize,
                _height,
                _pos,
                _front,
                _back,
                _left,
                _right,
                _top,
                _bottom
            );
        }

        private double _size = 1.0; // Размер нижнего основания
        private double _topSize = 0.5; // Размер верхнего основания
        private double _height = 1.0; // Высота усеченной пирамиды

        public double Size
        {
            get => _size;
            set { _size = value; UpdateModel(); }
        }

        public double TopSize
        {
            get => _topSize;
            set { _topSize = value; UpdateModel(); }
        }

        public double Height
        {
            get => _height;
            set { _height = value; UpdateModel(); }
        }

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
            DrawPir(_size, _topSize, _height, _pos, _front, _back, _left, _right, _top, _bottom);
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

        private void DrawPir(
            double size,
            double topSize,
            double height,
            Point3D pos,
            Brush front,
            Brush back,
            Brush left,
            Brush right,
            Brush top,
            Brush bottom)
        {
            double halfSize = size / 2;
            double halfTopSize = topSize / 2;

            // Вершины усеченной пирамиды
            Point3D[] vertices =
            {
                // Нижнее основание
                new Point3D(-halfSize, -halfSize, -height / 2), // 0
                new Point3D(halfSize, -halfSize, -height / 2),  // 1
                new Point3D(halfSize, halfSize, -height / 2),   // 2
                new Point3D(-halfSize, halfSize, -height / 2),  // 3

                // Верхнее основание
                new Point3D(-halfTopSize, -halfTopSize, height / 2), // 4
                new Point3D(halfTopSize, -halfTopSize, height / 2),  // 5
                new Point3D(halfTopSize, halfTopSize, height / 2),    // 6
                new Point3D(-halfTopSize, halfTopSize, height / 2)    // 7
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
                // Нижнее основание
                new[] {0, 1, 2}, new[] {0, 2, 3},

                // Верхнее основание
                new[] {4, 6, 5}, new[] {4, 7, 6},

                // Боковые грани
                new[] {0, 4, 5}, new[] {0, 5, 1}, // Передняя
                new[] {1, 5, 6}, new[] {1, 6, 2}, // Правая
                new[] {2, 6, 7}, new[] {2, 7, 3}, // Задняя
                new[] {3, 7, 4}, new[] {3, 4, 0}  // Левая
            };

            Brush[] materials =
            {
                bottom, bottom, // Нижнее основание
                top, top,       // Верхнее основание
                front, front,   // Передняя
                right, right,   // Правая
                back, back,     // Задняя
                left, left      // Левая
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