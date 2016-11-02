using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Drawing;
using ComputacaoGraficaProject.View;
using ComputacaoGraficaProject.Model;
using ComputacaoGraficaProject.Sintese.Primitivas;
using ComputacaoGraficaProject.Sintese.Transformacoes;

namespace ComputacaoGraficaProject
{
    /// <summary>
    /// Interaction logic for ProjectTransformacoes3D.xaml
    /// </summary>
    public partial class ProjectTransformacoes3D : Window
    {
        public ProjectTransformacoes3D()
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
                imagem.desenharAbscissas3D();
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

        // Evento de limpar a tela.
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
            X_Translacao.Text = ""; Y_Translacao.Text = ""; Z_Translacao.Text = "";
            X_Escala.Text = ""; Y_Escala.Text = ""; Z_Escala.Text = "";
            anguloRotacao.Text = "";
            A_Cisalhamento.Text = ""; B_Cisalhamento.Text = "";
        }
        
        private List<double[]> listaTransformacoes = new List<double[]>();

        // Evento de envio das transformações definidas.
        private void transformar_Click(object sender, RoutedEventArgs e)
        {
            if (validacaoImagem())
            {
                Transformacoes3D transformacoes = new Transformacoes3D();
                transformacoes.conjuntoDeTransformacoes(listaTransformacoes);
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

                // info[0] = tipo de transformação.
                // info[n] = parâmetros da transformação.

                if (button == tTransladar)
                {
                    info = new double[] { 1, double.Parse(X_Translacao.Text), double.Parse(Y_Translacao.Text), double.Parse(Z_Translacao.Text) };
                    listViewTransformacoes.Items.Add(new Functions.ObjectTransformacao { Transformacao = "Transladar(" + double.Parse(X_Translacao.Text) + ", " + double.Parse(Y_Translacao.Text) + ", " + double.Parse(Z_Translacao.Text) + ")" });
                }
                else if (button == tEscalonar)
                {
                    info = new double[] { 2, double.Parse(X_Escala.Text), double.Parse(Y_Escala.Text), double.Parse(Z_Escala.Text) };
                    listViewTransformacoes.Items.Add(new Functions.ObjectTransformacao { Transformacao = "Escalonar(" + X_Escala.Text + "," + Y_Escala.Text + "," + Z_Escala.Text + ")" });
                }
                else if (button == tRotacionar)
                {
                    int tipo = 1;
                    String tipoString = "";
                    if (eixoRotacao.SelectedIndex == 0)
                    {
                        tipo = 1;
                        tipoString = "X";
                    }
                    else if (eixoRotacao.SelectedIndex == 1)
                    {
                        tipo = 2;
                        tipoString = "Y";
                    }
                    else
                    {
                        tipo = 3;
                        tipoString = "Z";
                    }
                    info = new double[] { 3, double.Parse(anguloRotacao.Text), tipo };
                    listViewTransformacoes.Items.Add(new Functions.ObjectTransformacao { Transformacao = "Rotacionar(" + anguloRotacao.Text + " Em " + tipoString + ")" });
                }
                else if (button == tRefletir_1)
                {
                    info = new double[] { 4, 1 };
                    listViewTransformacoes.Items.Add(new Functions.ObjectTransformacao { Transformacao = "Refletir em XY" });
                }
                else if (button == tRefletir_2)
                {
                    info = new double[] { 4, 2 };
                    listViewTransformacoes.Items.Add(new Functions.ObjectTransformacao { Transformacao = "Refletir em YZ" });
                }
                else if (button == tRefletir_3)
                {
                    info = new double[] { 4, 3 };
                    listViewTransformacoes.Items.Add(new Functions.ObjectTransformacao { Transformacao = "Refletir em XZ" });
                }
                else if (button == tCisalhar)
                {
                    int tipo = 1;
                    String tipoString = "";
                    if (eixoCisalhamento.SelectedIndex == 0)
                    {
                        tipo = 1;
                        tipoString = "X";
                    }
                    else if (eixoCisalhamento.SelectedIndex == 1)
                    {
                        tipo = 2;
                        tipoString = "Y";
                    }
                    else
                    {
                        tipo = 3;
                        tipoString = "Z";
                    }
                    info = new double[] { 5, double.Parse(A_Cisalhamento.Text), double.Parse(B_Cisalhamento.Text), tipo };
                    listViewTransformacoes.Items.Add(new Functions.ObjectTransformacao { Transformacao = "Cisalhar( Em " + tipoString + ": A = " + double.Parse(A_Cisalhamento.Text) + ", B = " + double.Parse(B_Cisalhamento.Text) + ")" });
                }

                listaTransformacoes.Insert(0, info);
            } catch (Exception) { MessageBox.Show("Preencha todos os campos!"); };
        }

        private List<double[]> listaRetas = new List<double[]>();

        // Desenha um cubo na imagem de transformação.
        private void btn_inserirCubo_Click(object sender, RoutedEventArgs e)
        {
            inserirCubo();
        }
        
        // Desenha um cubo na imagem de transformação.
        private void btn_inserirRetangulo_Click(object sender, RoutedEventArgs e)
        {
            inserirRetangulo();
        }

        // Insere um cubo na interface.
        private void inserirCubo()
        {
            limparDados();
            resetarImagem();

            int tamX = 100;
            int tamY = 100;
            int tamZ = 50;

            // Matriz do retângulo.
            List<double[]> matrizQuadrado = new List<double[]>();
            matrizQuadrado.Add(new double[] { 0, 0, tamX, tamX, 0, 0, tamX, tamX });
            matrizQuadrado.Add(new double[] { 0, tamY, tamY, 0, 0, tamY, 0, tamY });
            matrizQuadrado.Add(new double[] { 0, 0, 0, 0, tamZ, tamZ, tamZ, tamZ });
            matrizQuadrado.Add(new double[] { 1, 1, 1, 1, 1, 1, 1, 1 });
            Referencias.matrizObjeto = matrizQuadrado;

            // Apresenta o retângulo na interface.
            apresentarObjetoNaInterface(matrizQuadrado);
        }

        // Insere um retângulo na interface.
        private void inserirRetangulo()
        {
            limparDados();
            resetarImagem();

            int tamX = 100;
            int tamY = 50;
            int tamZ = 20;

            // Matriz do retângulo.
            List<double[]> matrizRetangulo = new List<double[]>();
            matrizRetangulo.Add(new double[] { 0, 0, tamX, tamX, 0, 0, tamX, tamX });
            matrizRetangulo.Add(new double[] { 0, tamY, tamY, 0, 0, tamY, 0, tamY });
            matrizRetangulo.Add(new double[] { 0, 0, 0, 0, tamZ, tamZ, tamZ, tamZ });
            matrizRetangulo.Add(new double[] { 1, 1, 1, 1, 1, 1, 1, 1 });
            Referencias.matrizObjeto = matrizRetangulo;

            // Apresenta o retângulo na interface.
            apresentarObjetoNaInterface(matrizRetangulo);
        }

        // Apresenta o objeto na interface.
        private void apresentarObjetoNaInterface(List<double[]> matrizObjeto)
        {
            List<double[]> listaRetas = new List<double[]>();
            listaRetas.Add(new double[] { matrizObjeto[0][0], matrizObjeto[1][0], matrizObjeto[2][0] });
            listaRetas.Add(new double[] { matrizObjeto[0][1], matrizObjeto[1][1], matrizObjeto[2][1] });
            listaRetas.Add(new double[] { matrizObjeto[0][2], matrizObjeto[1][2], matrizObjeto[2][2] });
            listaRetas.Add(new double[] { matrizObjeto[0][3], matrizObjeto[1][3], matrizObjeto[2][3] });
            listaRetas.Add(new double[] { matrizObjeto[0][0], matrizObjeto[1][0], matrizObjeto[2][0] });
            listaRetas.Add(new double[] { matrizObjeto[0][4], matrizObjeto[1][4], matrizObjeto[2][4] });
            listaRetas.Add(new double[] { matrizObjeto[0][5], matrizObjeto[1][5], matrizObjeto[2][5] });
            listaRetas.Add(new double[] { matrizObjeto[0][1], matrizObjeto[1][1], matrizObjeto[2][1] });
            listaRetas.Add(new double[] { matrizObjeto[0][5], matrizObjeto[1][5], matrizObjeto[2][5] });
            listaRetas.Add(new double[] { matrizObjeto[0][7], matrizObjeto[1][7], matrizObjeto[2][7] });
            listaRetas.Add(new double[] { matrizObjeto[0][2], matrizObjeto[1][2], matrizObjeto[2][2] });
            listaRetas.Add(new double[] { matrizObjeto[0][7], matrizObjeto[1][7], matrizObjeto[2][7] });
            listaRetas.Add(new double[] { matrizObjeto[0][6], matrizObjeto[1][6], matrizObjeto[2][6] });
            listaRetas.Add(new double[] { matrizObjeto[0][3], matrizObjeto[1][3], matrizObjeto[2][3] });
            listaRetas.Add(new double[] { matrizObjeto[0][6], matrizObjeto[1][6], matrizObjeto[2][6] });
            listaRetas.Add(new double[] { matrizObjeto[0][4], matrizObjeto[1][4], matrizObjeto[2][4] });
            Referencias.listaRetas = listaRetas;

            Retas retas = new Retas(null);
            retas.desenharRetas_PontoMedio(listaRetas);
            retas.atualizarImagem();
        }
    }
}
