using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Media3D;

namespace WpfApp2
{

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed) return;

            // Угол поворота
            double angle = 45;

            // Центр вращения (зависит от размера и положения куба)
            Point3D center = new Point3D(0, 0, 0); // Убедитесь, что это центр вашего куба

            // Создаем новую группу трансформаций
            Transform3DGroup transform = new Transform3DGroup();

            // Добавляем вращение вокруг осей X, Y, Z
            transform.Children.Add(new RotateTransform3D(
                new AxisAngleRotation3D(new Vector3D(1, 0, 0), angle),
                center
            ));
            transform.Children.Add(new RotateTransform3D(
                new AxisAngleRotation3D(new Vector3D(0, 1, 0), angle),
                center
            ));
            transform.Children.Add(new RotateTransform3D(
                new AxisAngleRotation3D(new Vector3D(0, 0, 1), angle),
                center
            ));

            // Применяем трансформацию к кубу
            Cube1.Content.Transform = transform;
        }
    }

    }
