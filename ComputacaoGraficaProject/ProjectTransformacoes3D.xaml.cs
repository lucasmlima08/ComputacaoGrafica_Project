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
            
            //apagarCamposSintese();

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

        /* -----------------  BOTAO DE LIMPAR A TELA ----------------------------------------- */
        private void limparTela_Click(object sender, EventArgs e)
        {
            imagemIniciada = false;
            validacaoImagem();
            Referencias.listaRetas.Clear();
        }

        /* ------------------- INÍCIO: PARTE DE TRANSFORMAÇÕE 3D ------------------------------- */

        private List<double[]> listaTransformacoes = new List<double[]>();

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
                info = new double[] { 1, Double.Parse(X_Translacao.Text), Double.Parse(Y_Translacao.Text), Double.Parse(Z_Translacao.Text) };
                listViewTransformacoes.Items.Add(new Functions.ObjectTransformacao { Transformacao = "Transladar(" + Double.Parse(X_Translacao.Text) + ", " + Double.Parse(Y_Translacao.Text) + ", " + Double.Parse(Z_Translacao.Text) + ")" });
            }
            else if (button == tEscalonar)
            {
                info = new double[] { 2, Double.Parse(X_Escala.Text), Double.Parse(Y_Escala.Text), Double.Parse(Z_Escala.Text) };
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
                info = new double[] { 3, Double.Parse(anguloRotacao.Text), tipo };
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
                info = new double[] { 5, Double.Parse(X_Cisalhamento.Text), Double.Parse(Y_Cisalhamento.Text), Double.Parse(Z_Cisalhamento.Text) };
                listViewTransformacoes.Items.Add(new Functions.ObjectTransformacao { Transformacao = "Cisalhar(" + X_Cisalhamento.Text + ", " + Y_Cisalhamento.Text + ", " + Z_Cisalhamento.Text + ")" });
            }

            listaTransformacoes.Insert(0, info);
        }

        private List<double[]> listaRetas = new List<double[]>();

        // Desenha um cubo.
        private void btn_inserirCubo_Click(object sender, RoutedEventArgs e)
        {
            imagemIniciada = false;
            validacaoImagem();

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
            
            bitmap = desenharQuadrado(bitmap, Referencias.listaRetas);
            inserirPontosNaTabela(Referencias.listaRetas);

            Retas retas = new Retas(bitmap);
            retas.atualizarImagem();
        }

        private Bitmap desenharQuadrado(Bitmap bitmap, List<double[]> listaRetas)
        {
            Retas retas = new Retas(bitmap);
            retas.desenharRetas_PontoMedio(Referencias.listaRetas);
            bitmap = retas.getImagem();
            return bitmap;
        }

        // Insere os pontos na tabela de apresentação ao usuário.
        private void inserirPontosNaTabela(List<double[]> listaRetas)
        {
            listViewPontos.Items.Clear();
            for (int i = 0; i < listaRetas.Count; i++)
            {
                listViewPontos.Items.Add(new Functions.ObjectPonto3D { X = listaRetas[i][0] + "", Y = listaRetas[i][1] + "", Z = listaRetas[i][2] + "" });
            }
        }

        /* ------------------- FINAL: PARTE DE TRANSFORMAÇÕES 3D ------------------------------- */
    }
}
