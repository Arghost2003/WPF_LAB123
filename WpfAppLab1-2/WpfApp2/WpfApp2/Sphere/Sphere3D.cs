using System;
using System.Collections.Generic;
using System.Windows.Media.Media3D;
using System.Windows.Media;

namespace WpfApp2.Sphere
{
    public class Sphere3D : ModelVisual3D
    {
        private static readonly Brush DefaultColor = Brushes.Gray;
        private double _radius = 0.5;
        private int _segments = 32; // Количество сегментов для аппроксимации сферы

        public Sphere3D()
        {
            DrawSphere();
        }

        // Свойство для радиуса сферы
        public double Radius
        {
            get => _radius;
            set { _radius = value; DrawSphere(); }
        }

        // Свойство для положения сферы
        public Point3D Position { get; set; } = new Point3D(0, 0, 0);

        // Свойство для цвета сферы
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

        // Метод для отрисовки сферы
        private void DrawSphere()
        {
            var vertices = new Point3D[(_segments + 1) * (_segments + 1)];
            var faces = new List<int[]>();

            // Генерация вершин сферы
            for (int i = 0; i <= _segments; i++)
            {
                double lat = Math.PI * i / _segments; // Широта
                for (int j = 0; j <= _segments; j++)
                {
                    double lon = 2 * Math.PI * j / _segments; // Долгота
                    double x = Math.Sin(lat) * Math.Cos(lon);
                    double y = Math.Sin(lat) * Math.Sin(lon);
                    double z = Math.Cos(lat);

                    vertices[i * (_segments + 1) + j] = new Point3D(
                        Position.X + _radius * x,
                        Position.Y + _radius * y,
                        Position.Z + _radius * z
                    );
                }
            }

            // Генерация граней сферы
            for (int i = 0; i < _segments; i++)
            {
                for (int j = 0; j < _segments; j++)
                {
                    int v1 = i * (_segments + 1) + j;
                    int v2 = v1 + 1;
                    int v3 = (i + 1) * (_segments + 1) + j;
                    int v4 = v3 + 1;

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
                m3dg.Children.Add(AddFace(vertices, face, material));
            }

            Content = m3dg;
        }
    }
}