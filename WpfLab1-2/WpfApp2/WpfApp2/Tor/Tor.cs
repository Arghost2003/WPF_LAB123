using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace WpfApp2.Tor
{
    public class Тор : ModelVisual3D
    {
        private static readonly Brush DefaultColor = Brushes.Gray;
        private double _majorRadius = 1.0; // Большой радиус тора
        private double _minorRadius = 0.3; // Малый радиус тора
        private int _majorSegments = 32; // Количество сегментов по большому радиусу
        private int _minorSegments = 16; // Количество сегментов по малому радиусу

        public Тор()
        {
            DrawTorus();
        }

        // Свойство для большого радиуса тора
        public double MajorRadius
        {
            get => _majorRadius;
            set { _majorRadius = value; DrawTorus(); }
        }

        // Свойство для малого радиуса тора
        public double MinorRadius
        {
            get => _minorRadius;
            set { _minorRadius = value; DrawTorus(); }
        }

        // Свойство для положения тора
        public Point3D Position { get; set; } = new Point3D(0, 0, 0);

        // Свойство для цвета тора
        public Brush Color { get; set; } = DefaultColor;

        // Метод для создания грани
        private static GeometryModel3D AddFace(Point3D[] vertices, int[] indices, Material material)
        {
            var mesh = new MeshGeometry3D();
            foreach (int index in indices)
            {
                mesh.Positions.Add(vertices[index]);
            }

            // Создание треугольников для грани
            for (int i = 1; i < indices.Length - 1; i++)
            {
                mesh.TriangleIndices.Add(0);
                mesh.TriangleIndices.Add(i);
                mesh.TriangleIndices.Add(i + 1);
            }

            return new GeometryModel3D { Geometry = mesh, Material = material };
        }

        // Метод для отрисовки тора
        private void DrawTorus()
        {
            var vertices = new List<Point3D>();
            var faces = new List<int[]>();

            // Генерация вершин тора
            for (int i = 0; i < _majorSegments; i++)
            {
                double majorAngle = 2 * Math.PI * i / _majorSegments;
                for (int j = 0; j < _minorSegments; j++)
                {
                    double minorAngle = 2 * Math.PI * j / _minorSegments;

                    // Вычисление координат точки на торе
                    double x = (_majorRadius + _minorRadius * Math.Cos(minorAngle)) * Math.Cos(majorAngle);
                    double y = (_majorRadius + _minorRadius * Math.Cos(minorAngle)) * Math.Sin(majorAngle);
                    double z = _minorRadius * Math.Sin(minorAngle);

                    vertices.Add(new Point3D(
                        Position.X + x,
                        Position.Y + y,
                        Position.Z + z
                    ));
                }
            }

            // Генерация граней тора
            for (int i = 0; i < _majorSegments; i++)
            {
                for (int j = 0; j < _minorSegments; j++)
                {
                    int v1 = i * _minorSegments + j;
                    int v2 = ((i + 1) % _majorSegments) * _minorSegments + j;
                    int v3 = v1 + 1;
                    int v4 = v2 + 1;

                    // Проверка на выход за границы массива
                    if (v3 >= vertices.Count || v4 >= vertices.Count)
                        continue;

                    // Два треугольника для каждого квадрата
                    faces.Add(new int[] { v1, v2, v3 });
                    faces.Add(new int[] { v2, v4, v3 });
                }
            }

            var m3dg = new Model3DGroup();
            var material = new DiffuseMaterial(Color);

            // Отрисовка всех граней
            foreach (var face in faces)
            {
                m3dg.Children.Add(AddFace(vertices.ToArray(), face, material));
            }

            Content = m3dg;
        }
    }
}