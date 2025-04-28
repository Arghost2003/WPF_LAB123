using System;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace WpfApp2.Okt
{
    public class Okt3D : ModelVisual3D
    {
        private readonly static Brush _defaultColor = Brushes.Gray;

        public Okt3D()
        {
            _pos = new Point3D(0, 0, 0); // Инициализация позиции
            DrawOcta(_size, _pos, _front, _back, _left, _right, _top, _bottom);
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
                DrawOcta(_size, _pos, _front, _back, _left, _right, _top, _bottom);
            }
        }

        // Положение центра октаэдра
        private Point3D _pos;
        public Point3D Position
        {
            get => _pos;
            set
            {
                _pos = value;
                // После изменения значения поля фиксируем изменения
                DrawOcta(_size, _pos, _front, _back, _left, _right, _top, _bottom);
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
                DrawOcta(_size, _pos, _front, _back, _left, _right, _top, _bottom);
            }
        }

        private Brush _back = _defaultColor;
        public Brush Back
        {
            get => _back;
            set
            {
                _back = value;
                DrawOcta(_size, _pos, _front, _back, _left, _right, _top, _bottom);
            }
        }

        private Brush _left = _defaultColor;
        public Brush Left
        {
            get => _left;
            set
            {
                _left = value;
                DrawOcta(_size, _pos, _front, _back, _left, _right, _top, _bottom);
            }
        }

        private Brush _right = _defaultColor;
        public Brush Right
        {
            get => _right;
            set
            {
                _right = value;
                DrawOcta(_size, _pos, _front, _back, _left, _right, _top, _bottom);
            }
        }

        private Brush _top = _defaultColor;
        public Brush Top
        {
            get => _top;
            set
            {
                _top = value;
                DrawOcta(_size, _pos, _front, _back, _left, _right, _top, _bottom);
            }
        }

        private Brush _bottom = _defaultColor;
        public Brush Bottom
        {
            get => _bottom;
            set
            {
                _bottom = value;
                DrawOcta(_size, _pos, _front, _back, _left, _right, _top, _bottom);
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

        private void DrawOcta(
            double size,
            Point3D pos,
            Brush front,
            Brush back,
            Brush left,
            Brush right,
            Brush top,
            Brush bottom)
        {
            // Вершины октаэдра
            double absX = size / 2;
            double absY = size / 2;
            double absZ = size / 2;

            Point3D vertexTop = new Point3D(0 + pos.X, absY + pos.Y, 0 + pos.Z);       // Верхняя вершина
            Point3D vertexBottom = new Point3D(0 + pos.X, -absY + pos.Y, 0 + pos.Z);   // Нижняя вершина
            Point3D vertexFront = new Point3D(0 + pos.X, 0 + pos.Y, absZ + pos.Z);     // Передняя вершина
            Point3D vertexBack = new Point3D(0 + pos.X, 0 + pos.Y, -absZ + pos.Z);     // Задняя вершина
            Point3D vertexLeft = new Point3D(-absX + pos.X, 0 + pos.Y, 0 + pos.Z);     // Левая вершина
            Point3D vertexRight = new Point3D(absX + pos.X, 0 + pos.Y, 0 + pos.Z);     // Правая вершина

            Model3DGroup m3dg = new Model3DGroup();

            // Добавление граней
            // 1 Верхняя передняя грань
            DiffuseMaterial material = new DiffuseMaterial(front);
            GeometryModel3D faceTopFront = AddFace(vertexTop, vertexFront, vertexRight, material);
            m3dg.Children.Add(faceTopFront);

            // 2 Верхняя правая грань
            material = new DiffuseMaterial(right);
            GeometryModel3D faceTopRight = AddFace(vertexTop, vertexRight, vertexBack, material);
            m3dg.Children.Add(faceTopRight);

            // 3 Верхняя задняя грань
            material = new DiffuseMaterial(back);
            GeometryModel3D faceTopBack = AddFace(vertexTop, vertexBack, vertexLeft, material);
            m3dg.Children.Add(faceTopBack);

            // 4 Верхняя левая грань
            material = new DiffuseMaterial(left);
            GeometryModel3D faceTopLeft = AddFace(vertexTop, vertexLeft, vertexFront, material);
            m3dg.Children.Add(faceTopLeft);

            // 5 Нижняя передняя грань
            material = new DiffuseMaterial(front);
            GeometryModel3D faceBottomFront = AddFace(vertexBottom, vertexRight, vertexFront, material);
            m3dg.Children.Add(faceBottomFront);

            // 6 Нижняя правая грань
            material = new DiffuseMaterial(right);
            GeometryModel3D faceBottomRight = AddFace(vertexBottom, vertexBack, vertexRight, material);
            m3dg.Children.Add(faceBottomRight);

            // 7 Нижняя задняя грань
            material = new DiffuseMaterial(back);
            GeometryModel3D faceBottomBack = AddFace(vertexBottom, vertexLeft, vertexBack, material);
            m3dg.Children.Add(faceBottomBack);

            // 8 Нижняя левая грань
            material = new DiffuseMaterial(left);
            GeometryModel3D faceBottomLeft = AddFace(vertexBottom, vertexFront, vertexLeft, material);
            m3dg.Children.Add(faceBottomLeft);

            // Сохранение данных объекта
            Content = m3dg;
        }
    }
}