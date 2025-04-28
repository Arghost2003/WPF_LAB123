using System;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace WpfApp2.Dod
{
    public class Dod3d : ModelVisual3D
    {
        private static readonly Brush DefaultColor = Brushes.Gray;
        private double _size = 0.5;
        private Point3D _pos;
        private Brush _color = DefaultColor;

        public Dod3d()
        {
            DrawDodecahedron();
        }

        public double Size
        {
            get => _size;
            set { _size = value; DrawDodecahedron(); }
        }

        public Point3D Position
        {
            get => _pos;
            set { _pos = value; DrawDodecahedron(); }
        }

        public Brush Color
        {
            get => _color;
            set { _color = value; DrawDodecahedron(); }
        }

        private static GeometryModel3D AddFace(Point3D[] vertices, int[] indices, Material material)
        {
            var mesh = new MeshGeometry3D();
            foreach (int index in indices)
            {
                mesh.Positions.Add(vertices[index]);
            }

            // Создание треугольников для грани (5 вершин)
            mesh.TriangleIndices = new Int32Collection
            {
                0, 1, 2,
                0, 2, 3,
                0, 3, 4,
                1, 2, 3,
                1, 3, 4,
                2, 3, 4
            };

            return new GeometryModel3D { Geometry = mesh, Material = material };
        }

        private void DrawDodecahedron()
        {
            double phi = (1.0 + Math.Sqrt(5.0)) / 2.0; // Золотое сечение
            double scale = _size / Math.Sqrt(3); // Масштабирование

            // Вершины додекаэдра
            Point3D[] vertices = new Point3D[20]
            {
            //A [0:7]
                new Point3D(10, 10, 10),//A1            0
                new Point3D(-10, 10, 10),//A2           1
                new Point3D(10, -10, 10),//A3           2   
                new Point3D(10, 10, -10),//A4           3
                new Point3D(-10, -10, 10),//A5          4
                new Point3D(-10, 10, -10),//A6          5
                new Point3D(10, -10, -10),//A7          6
                new Point3D(-10, -10, -10),//A8         7
                //B [8:11]
                new Point3D(0, 10 * phi, 10 / phi),//B1   8
                new Point3D(0, -10 * phi, 10 / phi),//B2  9
                new Point3D(0, 10 * phi, -10 / phi),//B3  10
                new Point3D(0, -10 * phi, -10 / phi),//B4 11
                //C [12:15]
                new Point3D(10 / phi, 0, 10 * phi),//C1   12
                new Point3D(-10 / phi, 0, 10 * phi),//C2  13
                new Point3D(10 / phi, 0, -10 * phi),//C3  14
                new Point3D(-10 / phi, 0, -10 * phi),//C4 15
                //D [16:19]
                new Point3D(10 * phi, 10 / phi, 0),//D1   16
                new Point3D(-10 * phi, 10 / phi, 0),//D2  17
                new Point3D(10 * phi, -10 / phi, 0),//D3  18
                new Point3D(-10 * phi, -10 / phi, 0)//D4  19
                    };

                    // Определение граней
                int[][] faces =
                    {
                new[] {13, 4, 9, 2, 12}, // Грань 1
                new[] {1, 13, 12, 0, 13}, // Грань 2
                new[] {12, 2, 18, 16, 0}, // Грань 3
                new[] {0, 16, 3, 10, 8}, // Грань 4
                new[] {1, 8, 10, 5, 17}, // Грань 5
                new[] {13, 1, 17, 19, 4}, // Грань 6
                new[] {4, 19, 7, 11, 9}, // Грань 7
                new[] {2, 9, 11, 6, 18}, // Грань 8
                new[] {11, 7, 15, 14, 6}, // Грань 9
                new[] {18, 6, 14, 3, 16}, // Грань 10
                new[] {10, 3, 14, 15, 5}, // Грань 11
                new[] {19, 17, 5, 15, 7}   // Грань 12
            };

            // Масштабирование и перемещение вершин
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = new Point3D(
                    vertices[i].X * scale + _pos.X,
                    vertices[i].Y * scale + _pos.Y,
                    vertices[i].Z * scale + _pos.Z
                );
            }

            // Определение цветов для каждой грани
            Brush[] faceColors = new Brush[]
            {
                Brushes.Red,
                Brushes.Blue,
                Brushes.Green,
                Brushes.Yellow,
                Brushes.Orange,
                Brushes.Purple,
                Brushes.Cyan,
                Brushes.Black,
                Brushes.Brown,
                Brushes.LightCoral,
                Brushes.LightGreen,
                Brushes.LightBlue
            };

            var m3dg = new Model3DGroup();

            // Добавление граней с разными цветами
            for (int i = 0; i < faces.Length; i++)
            {
                var material = new DiffuseMaterial(faceColors[i % faceColors.Length]); // Используем цвет из массива
                m3dg.Children.Add(AddFace(vertices, faces[i], material));
            }

            Content = m3dg; // Устанавливаем содержимое для визуализации
        }
    }
}
