using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Windows.Media;

namespace WpfApp2.Tetr
{
    public class Tetr3D : ModelVisual3D
    {
        private readonly static Brush _defaultColor = Brushes.Gray;

        public Tetr3D()
        {
            DrawTetr(_size, _pos, _front, _top, _left, _back);
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
                DrawTetr(_size, _pos, _front, _top, _left, _back);
            }
        }

        // Положение центра тетраэдра
        private Point3D _pos;
        public Point3D Position
        {
            get => _pos;
            set
            {
                _pos = value;
                // После изменения значения поля фиксируем изменения
                DrawTetr(_size, _pos, _front, _top, _left, _back);
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
                DrawTetr(_size, _pos, _front, _top, _left, _back);
            }
        }

        private Brush _top = _defaultColor;
        public Brush Top
        {
            get => _top;
            set
            {
                _top = value;
                DrawTetr(_size, _pos, _front, _top, _left, _back);
            }
        }

        private Brush _left = _defaultColor;
        public Brush Left
        {
            get => _left;
            set
            {
                _left = value;
                DrawTetr(_size, _pos, _front, _top, _left, _back);
            }
        }

        private Brush _back = _defaultColor;
        public Brush Back
        {
            get => _back;
            set
            {
                _back = value;
                DrawTetr(_size, _pos, _front, _top, _left, _back);
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

        private void DrawTetr(
            double size,
            Point3D pos,
            Brush front,
            Brush top,
            Brush left,
            Brush back)
        {
            // Вершины тетраэдра
            double absX = size / 2;
            double absY = size / 2;
            double absZ = size / 2;

            Point3D vertex1 = new Point3D(-absX + pos.X, -absY + pos.Y, -absZ + pos.Z); // Нижняя левая задняя
            Point3D vertex2 = new Point3D(absX + pos.X, -absY + pos.Y, -absZ + pos.Z);  // Нижняя правая задняя
            Point3D vertex3 = new Point3D(0 + pos.X, absY + pos.Y, -absZ + pos.Z);     // Верхняя задняя
            Point3D vertex4 = new Point3D(0 + pos.X, 0 + pos.Y, absZ + pos.Z);         // Передняя вершина

            Model3DGroup m3dg = new Model3DGroup();

            // Добавление граней
            // 1 Передняя грань
            DiffuseMaterial material = new DiffuseMaterial(front);
            GeometryModel3D faceFront = AddFace(vertex1, vertex2, vertex4, material);
            m3dg.Children.Add(faceFront);

            // 2 Левая грань
            material = new DiffuseMaterial(left);
            GeometryModel3D faceLeft = AddFace(vertex1, vertex4, vertex3, material);
            m3dg.Children.Add(faceLeft);

            // 3 Правая грань
            material = new DiffuseMaterial(back);
            GeometryModel3D faceRight = AddFace(vertex2, vertex3, vertex4, material);
            m3dg.Children.Add(faceRight);

            // 4 Нижняя грань
            material = new DiffuseMaterial(top);
            GeometryModel3D faceBottom = AddFace(vertex1, vertex3, vertex2, material);
            m3dg.Children.Add(faceBottom);

            // Сохранение данных объекта
            Content = m3dg;
        }
    }
}