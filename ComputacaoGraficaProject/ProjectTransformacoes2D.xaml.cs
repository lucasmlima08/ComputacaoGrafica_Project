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
            imagemIniciada = false;
            validacaoImagem();
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
            Button button = sender as Button;

            double[] info = null;

            if (button == tTransladar)
            {
                info = new double[] { 1, Double.Parse(X_Translacao.Text), Double.Parse(Y_Translacao.Text) };
                listViewTransformacoes.Items.Add(new Functions.ObjectTransformacao { Transformacao = "Transladar(" + Double.Parse(X_Translacao.Text) + ", " + Double.Parse(Y_Translacao.Text) + ")" });
            }
            else if (button == tEscalonar)
            {
                info = new double[] { 2, Double.Parse(X_Escala.Text), Double.Parse(Y_Escala.Text) };
                listViewTransformacoes.Items.Add(new Functions.ObjectTransformacao { Transformacao = "Escalonar(" + X_Escala.Text + "," + Y_Escala.Text + ")" });
            }
            else if (button == tRotacionar)
            {
                info = new double[] { 3, Double.Parse(anguloRotacao.Text) };
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
                info = new double[] { 5, Double.Parse(X_Cisalhamento.Text), Double.Parse(Y_Cisalhamento.Text) };
                listViewTransformacoes.Items.Add(new Functions.ObjectTransformacao { Transformacao = "Cisalhar(" + X_Cisalhamento.Text + ", " + Y_Cisalhamento.Text + ")" });
            }

            listaTransformacoes.Insert(0, info);
        }

        private List<double[]> listaRetas = new List<double[]>();

        // Cria um quadrado.
        private void btn_inserirQuadrado_Click(object sender, RoutedEventArgs e)
        {
            limparDados();
            resetarImagem();

            int tam = 100;

            Referencias.listaRetas.Add(new double[] { 0, 0, 0 });
            Referencias.listaRetas.Add(new double[] { 0, tam, 0 });
            Referencias.listaRetas.Add(new double[] { tam, tam, 0 });
            Referencias.listaRetas.Add(new double[] { tam, 0, 0 });
            Referencias.listaRetas.Add(new double[] { 0, 0, 0 });

            inserirPontosNaTabela(Referencias.listaRetas);

            Retas retas = new Retas(null);
            retas.desenharRetas_PontoMedio(Referencias.listaRetas);
            retas.atualizarImagem();
        }

        // Cria um triângulo.
        private void btn_inserirTriangulo_Click(object sender, RoutedEventArgs e)
        {
            limparDados();
            resetarImagem();

            int tam = 100;

            Referencias.listaRetas.Add(new double[] { 0, 0, 0 });
            Referencias.listaRetas.Add(new double[] { tam / 2, tam, 0 });
            Referencias.listaRetas.Add(new double[] { tam, 0, 0 });
            Referencias.listaRetas.Add(new double[] { 0, 0, 0 });

            inserirPontosNaTabela(Referencias.listaRetas);

            Retas retas = new Retas(null);
            retas.desenharRetas_PontoMedio(Referencias.listaRetas);
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
