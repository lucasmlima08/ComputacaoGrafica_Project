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
using System.Drawing;
using ComputacaoGraficaProject.View;
using ComputacaoGraficaProject.Sintese;
using ComputacaoGraficaProject.Model;

namespace ComputacaoGraficaProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void projetos_Click(object sender, RoutedEventArgs e)
        {

            Button button = sender as Button;

            if (button == bPrimitivas)
            {
                ProjectPrimitivas primitivas = new ProjectPrimitivas();
                primitivas.Show();
            }
            else if (button == bTransformacoes2D)
            {
                ProjectTransformacoes2D transformacoes2D = new ProjectTransformacoes2D();
                transformacoes2D.Show();
            }
            else if (button == bTransformacoes3D)
            {
                ProjectTransformacoes3D transformacoes3D = new ProjectTransformacoes3D();
                transformacoes3D.Show();
            }
            else if (button == bProcessamento)
            {

            }
        }
    }
}
