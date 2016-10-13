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
        }

        // Limpar dados.
        private void limparDados()
        {
            Referencias.listaRetas.Clear();
            Referencias.listViewRetas.Items.Clear();
            X_Translacao.Text = ""; Y_Translacao.Text = ""; Z_Translacao.Text = "";
            X_Escala.Text = ""; Y_Escala.Text = ""; Z_Escala.Text = "";
            anguloRotacao.Text = "";
            X_Cisalhamento.Text = ""; Y_Cisalhamento.Text = ""; Z_Cisalhamento.Text = "";
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
                if (eixoRotacao.Content.Equals("X"))
                {
                    tipo = 1;
                    tipoString = "X";
                }
                else if (eixoRotacao.Content.Equals("Y"))
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
                info = new double[] { 5, double.Parse(X_Cisalhamento.Text), double.Parse(Y_Cisalhamento.Text), double.Parse(Z_Cisalhamento.Text) };
                listViewTransformacoes.Items.Add(new Functions.ObjectTransformacao { Transformacao = "Cisalhar(" + double.Parse(X_Cisalhamento.Text) + ", " + double.Parse(Y_Cisalhamento.Text) + ", " + double.Parse(Z_Cisalhamento.Text) + ")" });
            }

            listaTransformacoes.Insert(0, info);
        }

        private List<double[]> listaRetas = new List<double[]>();

        // Desenha um cubo na imagem de transformação.
        private void btn_inserirCubo_Click(object sender, RoutedEventArgs e)
        {
            limparDados();
            resetarImagem();

            int tamXY = 100;
            int tamZ = 50;
            
            Bitmap bitmap = new Bitmap(sizeImageX, sizeImageY);
            
            Referencias.listaRetas.Add(new double[] { 0, 0, 0 });
            Referencias.listaRetas.Add(new double[] { 0, tamXY, 0 });
            Referencias.listaRetas.Add(new double[] { tamXY, tamXY, 0 });
            Referencias.listaRetas.Add(new double[] { tamXY, 0, 0 });
            Referencias.listaRetas.Add(new double[] { 0, 0, 0 });
            Referencias.listaRetas.Add(new double[] { 0, 0, tamZ });
            Referencias.listaRetas.Add(new double[] { 0, tamXY, tamZ });
            Referencias.listaRetas.Add(new double[] { 0, tamXY, 0 });
            Referencias.listaRetas.Add(new double[] { 0, tamXY, tamZ });
            Referencias.listaRetas.Add(new double[] { tamXY, tamXY, tamZ });
            Referencias.listaRetas.Add(new double[] { tamXY, tamXY, 0 });
            Referencias.listaRetas.Add(new double[] { tamXY, tamXY, tamZ });
            Referencias.listaRetas.Add(new double[] { tamXY, 0, tamZ });
            Referencias.listaRetas.Add(new double[] { tamXY, 0, 0 });
            Referencias.listaRetas.Add(new double[] { tamXY, 0, tamZ });
            Referencias.listaRetas.Add(new double[] { 0, 0, tamZ });
            
            bitmap = desenharSequencia(bitmap, Referencias.listaRetas);
            inserirCoordenadasNaTabela(Referencias.listaRetas);

            Retas retas = new Retas(bitmap);
            retas.atualizarImagem();
        }

        // Desenha um cubo na imagem de transformação.
        private void btn_inserirRetangulo_Click(object sender, RoutedEventArgs e)
        {
            limparDados();
            resetarImagem();

            int tamX = 100;
            int tamY = 50;
            int tamZ = 20;

            Bitmap bitmap = new Bitmap(sizeImageX, sizeImageY);

            Referencias.listaRetas.Add(new double[] { 0, 0, 0 });
            Referencias.listaRetas.Add(new double[] { 0, tamY, 0 });
            Referencias.listaRetas.Add(new double[] { tamX, tamY, 0 });
            Referencias.listaRetas.Add(new double[] { tamX, 0, 0 });
            Referencias.listaRetas.Add(new double[] { 0, 0, 0 });
            Referencias.listaRetas.Add(new double[] { 0, 0, tamZ });
            Referencias.listaRetas.Add(new double[] { 0, tamY, tamZ });
            Referencias.listaRetas.Add(new double[] { 0, tamY, 0 });
            Referencias.listaRetas.Add(new double[] { 0, tamY, tamZ });
            Referencias.listaRetas.Add(new double[] { tamX, tamY, tamZ });
            Referencias.listaRetas.Add(new double[] { tamX, tamY, 0 });
            Referencias.listaRetas.Add(new double[] { tamX, tamY, tamZ });
            Referencias.listaRetas.Add(new double[] { tamX, 0, tamZ });
            Referencias.listaRetas.Add(new double[] { tamX, 0, 0 });
            Referencias.listaRetas.Add(new double[] { tamX, 0, tamZ });
            Referencias.listaRetas.Add(new double[] { 0, 0, tamZ });

            bitmap = desenharSequencia(bitmap, Referencias.listaRetas);
            inserirCoordenadasNaTabela(Referencias.listaRetas);

            Retas retas = new Retas(bitmap);
            retas.atualizarImagem();
        }

        // Desenha a sequência de retas.
        private Bitmap desenharSequencia(Bitmap bitmap, List<double[]> listaRetas)
        {
            Retas retas = new Retas(bitmap);
            retas.desenharRetas_PontoMedio(Referencias.listaRetas);
            bitmap = retas.getImagem();
            return bitmap;
        }

        // Insere as coordenadas do objeto na tabela de apresentação ao usuário.
        private void inserirCoordenadasNaTabela(List<double[]> listaRetas)
        {
            listViewPontos.Items.Clear();
            for (int i = 0; i < listaRetas.Count; i++)
            {
                listViewPontos.Items.Add(new Functions.ObjectPonto3D { X = listaRetas[i][0] + "", Y = listaRetas[i][1] + "", Z = listaRetas[i][2] + "" });
            }
        }
    }
}
