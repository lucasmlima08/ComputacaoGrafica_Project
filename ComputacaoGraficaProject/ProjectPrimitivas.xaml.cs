using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ComputacaoGraficaProject.View;
using ComputacaoGraficaProject.Model;
using ComputacaoGraficaProject.Sintese.Primitivas;

namespace ComputacaoGraficaProject
{
    /// <summary>
    /// Interaction logic for ProjectPrimitivas.xaml
    /// </summary>
    public partial class ProjectPrimitivas : Window
    {
        public ProjectPrimitivas()
        {
            InitializeComponent();
            
            //apagarCamposSintese();

            Referencias.imagemDraw = imagemDraw;
            Referencias.imageDrawAbscissas = imagemDrawAbscissas;

            Referencias.listaRetas = listaRetas;
            Referencias.listViewRetas = listViewPontos;
        }
        
        private Boolean imagemIniciada = false;
        private Ponto ponto;

        private int sizeImageX = 0;
        private int sizeImageY = 0;
        
        private void apagarCamposSintese()
        {
            X_Reta.Text = ""; Y_Reta.Text = "";
            raioCircunferencia.Text = "";

            listViewPontos.Items.Clear();
            listaRetas = new List<double[]>();
        }
        
        private Boolean validacaoCamposCircunferencia()
        {
            if (raioCircunferencia.Text.Equals(""))
            {
                MessageBox.Show("Preencha o tamanho do raio.");
                return false;
            }
            if (int.Parse(raioCircunferencia.Text) > sizeImageX / 2
                || int.Parse(raioCircunferencia.Text) > sizeImageY / 2)
            {
                MessageBox.Show("O raio está fora do intervalo da imagem.");
                return false;
            }
            return true;
        }

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
        
        private void imagemDraw_MouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Cross;
        }

        private void imagemDraw_MouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void imagemDraw_MouseMove(object sender, MouseEventArgs e)
        {
            if (imagemIniciada)
            {
                int xAux, yAux;
                double xNorm, yNorm;
                xAux = ponto.X_ParaMundo((int)e.GetPosition(imagemLabel).X);
                yAux = ponto.Y_ParaDispositivo((int)e.GetPosition(imagemLabel).Y);
                yAux = ponto.Y_ParaMundo(yAux);
                pontoCoordenadaMundo.Content = "Coordenadas de Mundo: " + xAux + "; " + yAux;

                xNorm = ponto.NormalizacaoX((int)e.GetPosition(imagemLabel).X);
                yNorm = ponto.Y_ParaDispositivo((int)e.GetPosition(imagemLabel).Y);
                yNorm = ponto.NormalizacaoY((int)yNorm);
                pontoNormalizacao.Content = "Normalização: " + String.Format("{0:0.000}", xNorm)
                                            + "; " + String.Format("{0:0.000}", yNorm);

                yAux = ponto.Y_ParaDispositivo((int)e.GetPosition(imagemLabel).Y);
                pontoCoordenadaDispositivo.Content = "Coordenadas de Dispositivo: " + 
                                                    (int)e.GetPosition(imagemLabel).X + "; " + yAux;
            }
        }

        private void imagemDraw_MouseDown(object sender, MouseButtonEventArgs e)
        {
            /*if (validacaoImagem())
            {
                int[] coordenadas = new int[] { (int)e.GetPosition(imagemLabel).X, (int)e.GetPosition(imagemLabel).Y };
                listaRetas.Add(coordenadas);

                listViewRetas.Items.Add(new Functions.ObjectReta { X = e.GetPosition(imagemLabel).X + "", Y = e.GetPosition(imagemLabel).Y + "" });
            }*/
        }

        /* -----------------  BOTAO DE LIMPAR A TELA ----------------------------------------- */
        private void limparTela_Click(object sender, EventArgs e)
        {
            imagemIniciada = false;
            validacaoImagem();
            apagarCamposSintese();
            Referencias.listaRetas.Clear();
        }

        /* ----------------- INÍCIO: PARTE DE DESENHO DA CIRCUNFERÊNCIA ------------------------ */

        private void CircunferenciaPontoMedio_Click(object sender, RoutedEventArgs e)
        {
            if (validacaoImagem() && !raioCircunferencia.Text.Equals(""))
            {
                Circunferencia c = new Circunferencia();
                c.methodPontoMedio(int.Parse(raioCircunferencia.Text));
            }
            else
            {
                MessageBox.Show("Preencha o raio.");
            }
        }


        private void circunferenciaEquacaoExplicita_Click(object sender, RoutedEventArgs e)
        {
            if (validacaoImagem() && !raioCircunferencia.Text.Equals(""))
            {
                Circunferencia c = new Circunferencia();
                c.methodEquacaoExplicita(int.Parse(raioCircunferencia.Text));
            }
            else
            {
                MessageBox.Show("Preencha o raio.");
            }
        }

        private void circunferenciaTrigonometrico_Click(object sender, RoutedEventArgs e)
        {
            if (validacaoImagem() && !raioCircunferencia.Text.Equals(""))
            {
                Circunferencia c = new Circunferencia();
                c.methodTrigonometrico(int.Parse(raioCircunferencia.Text));
            }
            else
            {
                MessageBox.Show("Preencha o raio.");
            }
        }
        
        /* ----------------- FINAL: PARTE DE DESENHO DA CIRCUNFERÊNCIA ------------------------- */

        /* ----------------- INICIO: PARTE DE DESENHO DA ELIPSE -------------------------------- */
        private void btn_DesenharElipse_Click(object sender, RoutedEventArgs e)
        {
            if(validacaoImagem() && !a_elipse.Text.Equals("") && !b_elipse.Text.Equals(""))
            {
                Circunferencia c = new Circunferencia();
                c.methodElipse(int.Parse(a_elipse.Text), int.Parse(b_elipse.Text));
            }
            else
            {
                MessageBox.Show("Preencha o A e B para desenhar a elipse.");
            }
        }

        /* ----------------- FINAL: PARTE DE DESENHO DA ELIPSE ------------------------------- -*/

        /* ------------------- INÍCIO: PARTE DE DESENHO DA RETA ------------------------------- */

        private void desenharReta_Click(object sender, RoutedEventArgs e)
        {
            if (listaRetas.Count > 0)
            {
                Button button = sender as Button;

                Retas retas = new Retas(null);

                if (button == bDesenharRetaDDA) // Desenha a Reta por DDA
                {
                    retas.desenharRetas_DDA(listaRetas);
                }
                else if (button == bDesenharRetaPontoMedio) // Desenha a Reta por Ponto Médio
                {
                    retas.desenharRetas_PontoMedio(listaRetas);
                }
                retas.atualizarImagem();
            }
            else
            {
                MessageBox.Show("Preencha os pontos.");
            }
        }
        
        private Boolean validacaoCamposReta()
        {
            if (X_Reta.Text.Equals("") || Y_Reta.Text.Equals(""))
            {
                MessageBox.Show("Preencha as coordenadas.");
                return false;
            }
            if (int.Parse(X_Reta.Text) > sizeImageX / 2
                || int.Parse(Y_Reta.Text) > sizeImageY / 2)
            {
                MessageBox.Show("As coordenadas estão fora do intervalo da imagem.");
                return false;
            }
            return true;
        }
        
        private List<double[]> listaRetas = new List<double[]>();

        private void adicionarCoordenadaReta_Click(object sender, RoutedEventArgs e)
        {
            if (validacaoImagem() && validacaoCamposReta())
            {
                double[] coordenadas = new double[] { Double.Parse(X_Reta.Text), Double.Parse(Y_Reta.Text), 0 };
                listaRetas.Add(coordenadas);

                listViewPontos.Items.Add(new Functions.ObjectPonto2D { X = X_Reta.Text, Y = Y_Reta.Text });
                X_Reta.Text = ""; Y_Reta.Text = "";
            }
        }

        /* ------------------- FINAL: PARTE DE DESENHO DA RETA ------------------------------- */
    }
}
