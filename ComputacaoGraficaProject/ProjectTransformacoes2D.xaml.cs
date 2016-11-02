using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ComputacaoGraficaProject.View;
using ComputacaoGraficaProject.Model;
using ComputacaoGraficaProject.Sintese.Primitivas;
using ComputacaoGraficaProject.Sintese.Transformacoes;

namespace ComputacaoGraficaProject
{
    /// <summary>
    /// Interaction logic for ProjectTransformacoes2D.xaml
    /// </summary>
    public partial class ProjectTransformacoes2D : Window
    {
        public ProjectTransformacoes2D()
        {
            InitializeComponent();
        
            Referencias.imagemDraw = imagemDraw;
            Referencias.imageDrawAbscissas = imagemDrawAbscissas;

            Referencias.listaRetas = listaRetas;
            Referencias.listViewRetas = listViewPontos;

            Referencias.listaTransformacoes = listaTransformacoes;
            Referencias.listViewTransformacoes = listViewTransformacoes;

            limparDados();
        }

        private Boolean imagemIniciada = false;
        private Ponto ponto;

        private int sizeImageX = 0;
        private int sizeImageY = 0;

        // Método de criação e verificação da imagem.
        private Boolean validacaoImagem()
        {
            if (!imagemIniciada)
            {
                sizeImageX = (int)imagemLabelAbscissas.ActualWidth;
                sizeImageY = (int)imagemLabelAbscissas.ActualHeight;

                Referencias.sizeImageX = sizeImageX;
                Referencias.sizeImageY = sizeImageY;

                imagemDraw.ImageSource = null;
                Imagem imagem = new Imagem();
                imagem.desenharAbscissas2D();
                ponto = new Ponto();
                resolucaoTela.Content = "Resolução da Tela: " + sizeImageX + " x " + sizeImageY;
                imagemIniciada = true;
            }
            return true;
        }

        // Reseta a imagem atual.
        private void resetarImagem()
        {
            imagemIniciada = false;
            validacaoImagem();
        }

        // Evento de limpar os objetos da tela.
        private void limparTela_Click(object sender, EventArgs e)
        {
            limparDados();
            resetarImagem();
        }

        // Limpar dados.
        private void limparDados()
        {
            Referencias.listaRetas.Clear();
            Referencias.listViewRetas.Items.Clear();
            X_Translacao.Text = ""; Y_Translacao.Text = "";
            X_Escala.Text = ""; Y_Escala.Text = "";
            anguloRotacao.Text = "";
            X_Cisalhamento.Text = ""; Y_Cisalhamento.Text = "";
        }

        private List<double[]> listaTransformacoes = new List<double[]>();

        // Evento de envio das transformações definidas.
        private void transformar_Click(object sender, RoutedEventArgs e)
        {
            if (validacaoImagem())
            {
                Transformacoes2D transformacoes2D = new Transformacoes2D();
                transformacoes2D.conjuntoDeTransformacoes(listaTransformacoes);
            }

            listViewTransformacoes.Items.Clear();
            listaTransformacoes = new List<double[]>();
        }

        // Evento que envia para a classe de transformação, o tipo de transformação escolhida pelo usuário.
        private void bTransformacao_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = sender as Button;

                double[] info = null;

                if (button == tTransladar)
                {
                    info = new double[] { 1, double.Parse(X_Translacao.Text), double.Parse(Y_Translacao.Text) };
                    listViewTransformacoes.Items.Add(new Functions.ObjectTransformacao { Transformacao = "Transladar(" + double.Parse(X_Translacao.Text) + ", " + double.Parse(Y_Translacao.Text) + ")" });
                }
                else if (button == tEscalonar)
                {
                    info = new double[] { 2, double.Parse(X_Escala.Text), double.Parse(Y_Escala.Text) };
                    listViewTransformacoes.Items.Add(new Functions.ObjectTransformacao { Transformacao = "Escalonar(" + X_Escala.Text + "," + Y_Escala.Text + ")" });
                }
                else if (button == tRotacionar)
                {
                    info = new double[] { 3, double.Parse(anguloRotacao.Text) };
                    listViewTransformacoes.Items.Add(new Functions.ObjectTransformacao { Transformacao = "Rotacionar(" + anguloRotacao.Text + ")" });
                }
                else if (button == tRefletir_1)
                {
                    info = new double[] { 4, 1 };
                    listViewTransformacoes.Items.Add(new Functions.ObjectTransformacao { Transformacao = "Refletir em X" });
                }
                else if (button == tRefletir_2)
                {
                    info = new double[] { 4, 2 };
                    listViewTransformacoes.Items.Add(new Functions.ObjectTransformacao { Transformacao = "Refletir em Y" });
                }
                else if (button == tRefletir_3)
                {
                    info = new double[] { 4, 3 };
                    listViewTransformacoes.Items.Add(new Functions.ObjectTransformacao { Transformacao = "Refletir em X e Y" });
                }
                else if (button == tCisalhar)
                {
                    info = new double[] { 5, double.Parse(X_Cisalhamento.Text), double.Parse(Y_Cisalhamento.Text) };
                    listViewTransformacoes.Items.Add(new Functions.ObjectTransformacao { Transformacao = "Cisalhar(" + X_Cisalhamento.Text + ", " + Y_Cisalhamento.Text + ")" });
                }

                listaTransformacoes.Insert(0, info);
            }
            catch (Exception) { MessageBox.Show("Preencha todos os campos!"); }
        }

        private List<double[]> listaRetas = new List<double[]>();

        // Cria um quadrado.
        private void btn_inserirQuadrado_Click(object sender, RoutedEventArgs e)
        {
            inserirQuadrado();
        }

        // Cria um triângulo.
        private void btn_inserirTriangulo_Click(object sender, RoutedEventArgs e)
        {
            inserirTriangulo();
        }

        // Insere um quadrado na interface.
        private void inserirQuadrado()
        {
            limparDados();
            resetarImagem();

            int tam = 100;

            // Matriz do quadrado.
            List<double[]> matrizQuadrado = new List<double[]>();
            matrizQuadrado.Add(new double[] { 0, 0, tam, tam });
            matrizQuadrado.Add(new double[] { 0, tam, tam, 0 });
            matrizQuadrado.Add(new double[] { 1, 1, 1, 1 });
            Referencias.matrizObjeto = matrizQuadrado;

            // Apresenta o triangulo na interface.
            apresentarObjetoNaInterface(matrizQuadrado);
        }

        // Insere um triângulo na interface.
        private void inserirTriangulo()
        {
            limparDados();
            resetarImagem();

            int tam = 100;

            // Matriz do quadrado.
            List<double[]> matrizTrangulo = new List<double[]>();
            matrizTrangulo.Add(new double[] { 0, tam / 2, tam });
            matrizTrangulo.Add(new double[] { 0, tam, 0 });
            matrizTrangulo.Add(new double[] { 1, 1, 1 });
            Referencias.matrizObjeto = matrizTrangulo;

            // Apresenta o triangulo na interface.
            apresentarObjetoNaInterface(matrizTrangulo);
        }

        // Apresenta o objeto na interface.
        private void apresentarObjetoNaInterface(List<double[]> matrizObjeto)
        {
            List<double[]> listaRetas = new List<double[]>();
            for (int i = 0; i < matrizObjeto[0].Length; i++)
            {
                listaRetas.Add(new double[] { matrizObjeto[0][i], matrizObjeto[1][i], 0 });
            }
            listaRetas.Add(new double[] { matrizObjeto[0][0], matrizObjeto[1][0], 0 });
            Referencias.listaRetas = listaRetas;

            Retas retas = new Retas(null);
            retas.desenharRetas_PontoMedio(listaRetas);
            retas.atualizarImagem();
        }

        // Insere os pontos na tabela de apresentação ao usuário.
        private void inserirPontosNaTabela(List<double[]> listaRetas)
        {
            listViewPontos.Items.Clear();
            for (int i = 0; i < listaRetas.Count; i++)
            {
                listViewPontos.Items.Add(new Functions.ObjectPonto2D { X = listaRetas[i][0] + "", Y = listaRetas[i][1] + "" });
            }
        }
    }
}
