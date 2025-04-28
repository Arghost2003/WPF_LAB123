using System;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace WpfApp2.Cone
{
    public class Cone3D : ModelVisual3D
    {
        private static readonly Brush DefaultColor = Brushes.Gray;
        private double _radius = 0.2;
        private double _height = 0.65;

        public Cone3D()
        {
            DrawCone();
        }

        // Свойство для радиуса
        public double Radius
        {
            get => _radius;
            set { _radius = value; DrawCone(); }
        }

        // Свойство для высоты
        public double Height
        {
            get => _height;
            set { _height = value; DrawCone(); }
        }

        // Свойство для положения конуса
        public Point3D Position { get; set; } = new Point3D(0, 0, 0);

        // Свойство для цвета конуса
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

        // Метод для отрисовки конуса
        private void DrawCone()
        {
            // Количество сегментов для аппроксимации окружности основания
            int segments = 32;

            // Вершины конуса
            Point3D[] vertices = new Point3D[segments + 2]; // +1 для вершины, +1 для центра основания
            vertices[0] = new Point3D(Position.X, Position.Y, Position.Z + Height); // Вершина конуса

            // Центр основания
            vertices[1] = new Point3D(Position.X, Position.Y, Position.Z);

            // Точки основания
            for (int i = 0; i < segments; i++)
            {
                double angle = 2 * Math.PI * i / segments;
                vertices[i + 2] = new Point3D(
                    Position.X + Radius * Math.Cos(angle),
                    Position.Y + Radius * Math.Sin(angle),
                    Position.Z
                );
            }

            // Грани конуса
            int[][] faces = new int[segments + 1][]; // +1 для основания

            // Боковые грани (треугольники)
            for (int i = 0; i < segments; i++)
            {
                faces[i] = new int[] { 0, i + 2, (i + 1) % segments + 2 };
            }

            // Основание (многоугольник)
            int[] baseFace = new int[segments + 1];
            baseFace[0] = 1; // Центр основания
            for (int i = 0; i < segments; i++)
            {
                baseFace[i + 1] = i + 2;
            }
            faces[segments] = baseFace;

            var m3dg = new Model3DGroup();
            var material = new DiffuseMaterial(Color);

            // Отрисовка боковых граней
            for (int i = 0; i < segments; i++)
            {
                m3dg.Children.Add(AddFace(vertices, faces[i], material));
            }

            // Отрисовка основания
            m3dg.Children.Add(AddFace(vertices, faces[segments], material));

            Content = m3dg;
        }
    }
}