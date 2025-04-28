using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Windows.Media;

namespace WpfApp2.Iso
{
    public class Iso3D : ModelVisual3D
    {
        private readonly static Brush _defaultColor = Brushes.Gray;

        public Iso3D()
        {
            DrawIcosa(_size, _pos, _front, _back, _left, _right, _top, _bottom);
        }

        // Размер по умолчанию
        private double _size = 0.5;

        // Поле задания размера
        public double Size
        {
            get => _size;
            set
            {
                _size = value;
                // После изменения значения поля фиксируем изменения
                DrawIcosa(_size, _pos, _front, _back, _left, _right, _top, _bottom);
            }
        }

        // Положение центра икосаэдра
        private Point3D _pos;
        public Point3D Position
        {
            get => _pos;
            set
            {
                _pos = value;
                // После изменения значения поля фиксируем изменения
                DrawIcosa(_size, _pos, _front, _back, _left, _right, _top, _bottom);
            }
        }

        // Материалы граней (цвета граней)
        private Brush _front = _defaultColor;
        public Brush Front
        {
            get => _front;
            set
            {
                _front = value;
                DrawIcosa(_size, _pos, _front, _back, _left, _right, _top, _bottom);
            }
        }

        private Brush _back = _defaultColor;
        public Brush Back
        {
            get => _back;
            set
            {
                _back = value;
                DrawIcosa(_size, _pos, _front, _back, _left, _right, _top, _bottom);
            }
        }

        private Brush _left = _defaultColor;
        public Brush Left
        {
            get => _left;
            set
            {
                _left = value;
                DrawIcosa(_size, _pos, _front, _back, _left, _right, _top, _bottom);
            }
        }

        private Brush _right = _defaultColor;
        public Brush Right
        {
            get => _right;
            set
            {
                _right = value;
                DrawIcosa(_size, _pos, _front, _back, _left, _right, _top, _bottom);
            }
        }

        private Brush _top = _defaultColor;
        public Brush Top
        {
            get => _top;
            set
            {
                _top = value;
                DrawIcosa(_size, _pos, _front, _back, _left, _right, _top, _bottom);
            }
        }

        private Brush _bottom = _defaultColor;
        public Brush Bottom
        {
            get => _bottom;
            set
            {
                _bottom = value;
                DrawIcosa(_size, _pos, _front, _back, _left, _right, _top, _bottom);
            }
        }

        // Добавление грани
        private static GeometryModel3D AddFace(
            Point3D point1,
            Point3D point2,
            Point3D point3,  // Вершины грани
            Material material)
        {
            GeometryModel3D geometryModel3D = new GeometryModel3D()
            {
                Geometry = new MeshGeometry3D()
                {
                    Positions = new Point3DCollection()
                    {
                        point1,
                        point2,
                        point3 // Вершины треугольника
                    },
                    TriangleIndices = new Int32Collection() { 0, 1, 2 }
                },
                Material = material // Цвет грани
            };
            return geometryModel3D;
        }

        private void DrawIcosa(
    double size,
    Point3D pos,
    Brush front,
    Brush back,
    Brush left,
    Brush right,
    Brush top,
    Brush bottom)
        {
            double phi = (1.0 + Math.Sqrt(5.0)) / 2.0;
            double scale = size / Math.Sqrt(1 + phi * phi);

            // Вершины нормализованы
            Point3D[] vertices = new Point3D[]
            {
        new Point3D(-1,  phi,  0), new Point3D(1,  phi,  0),
        new Point3D(-1, -phi,  0), new Point3D(1, -phi,  0),
        new Point3D( 0, -1,  phi), new Point3D(0,  1,  phi),
        new Point3D( 0, -1, -phi), new Point3D(0,  1, -phi),
        new Point3D( phi,  0, -1), new Point3D(phi,  0,  1),
        new Point3D(-phi,  0, -1), new Point3D(-phi,  0,  1)
            };

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = new Point3D(
                    vertices[i].X * scale + pos.X,
                    vertices[i].Y * scale + pos.Y,
                    vertices[i].Z * scale + pos.Z
                );
            }

            int[][] faces = new int[][]
            {
        new[] {0, 11, 5}, new[] {0, 5, 1}, new[] {0, 1, 7}, new[] {0, 7, 10}, new[] {0, 10, 11},
        new[] {1, 5, 9}, new[] {5, 11, 4}, new[] {11, 10, 2}, new[] {10, 7, 6}, new[] {7, 1, 8},
        new[] {3, 9, 4}, new[] {3, 4, 2}, new[] {3, 2, 6}, new[] {3, 6, 8}, new[] {3, 8, 9},
        new[] {4, 9, 5}, new[] {2, 4, 11}, new[] {6, 2, 10}, new[] {8, 6, 7}, new[] {9, 8, 1}
            };

            Model3DGroup m3dg = new Model3DGroup();
            Brush[] materials = { front, back, left, right, top, bottom };

            for (int i = 0; i < faces.Length; i++)
            {
                GeometryModel3D face = AddFace(
                    vertices[faces[i][0]],
                    vertices[faces[i][1]],
                    vertices[faces[i][2]],
                    new DiffuseMaterial(materials[i % materials.Length])
                );
                m3dg.Children.Add(face);
            }

            Content = m3dg;
        }
    }
}