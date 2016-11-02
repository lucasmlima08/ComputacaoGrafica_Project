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
using ComputacaoGraficaProject.Sintese.Viewporte;

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

            listaRetas = new List<double[]>();
            imagemIniciada = false;

            Referencias.imagemDraw = imagemDraw;
            Referencias.imageDrawAbscissas = imagemDrawAbscissas;

            Referencias.listaRetas = listaRetas;
            Referencias.listViewRetas = listViewPontos;
        }

        private List<double[]> listaRetas;

        private Imagem imagem;
        
        private Boolean imagemIniciada;
        private Ponto ponto;

        private int sizeImageX = 0;
        private int sizeImageY = 0;
        
        private void apagarCamposSintese()
        {
            X_Reta.Text = ""; Y_Reta.Text = "";
            raioCircunferencia.Text = "";

            listViewPontos.Items.Clear();
            listaRetas.Clear();
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
                imagem = new Imagem();
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
            apagarCamposViewport();
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
                    imagem = new Imagem(retas.getImagem());
                }
                else if (button == bDesenharRetaPontoMedio) // Desenha a Reta por Ponto Médio
                {
                    retas.desenharRetas_PontoMedio(listaRetas);
                    imagem = new Imagem(retas.getImagem());
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

        private void adicionarCoordenadaReta_Click(object sender, RoutedEventArgs e)
        {
            if (validacaoImagem() && validacaoCamposReta())
            {
                double[] coordenadas = new double[] { double.Parse(X_Reta.Text), double.Parse(Y_Reta.Text), 0 };
                listaRetas.Add(coordenadas);

                listViewPontos.Items.Add(new Functions.ObjectPonto2D { X = X_Reta.Text, Y = Y_Reta.Text });
                X_Reta.Text = ""; Y_Reta.Text = "";
            }
        }

        /* ------------------- FINAL: PARTE DE DESENHO DA RETA ------------------------------- */

        /* ------------------- INÍCIO: PARTE DE RECORTE DA VIEWPORT -------------------------- */

        private Boolean validacaoCamposViewport()
        {
            if (vp_X1.Equals("") || vp_X2.Equals("") || vp_Y1.Equals("") || vp_Y2.Equals(""))
            {
                MessageBox.Show("Preencha todas as coordenadas.");
                return false;
            }
            if (int.Parse(vp_X1.Text) < 0 || int.Parse(vp_X2.Text) > sizeImageX / 2 ||
                int.Parse(vp_Y1.Text) < 0 || int.Parse(vp_Y2.Text) > sizeImageY / 2)
            {
                MessageBox.Show("As coordenadas ultrapassam os limites da imagem.");
                return false;
            }
            if (int.Parse(vp_X1.Text) > int.Parse(vp_X2.Text) ||
                int.Parse(vp_Y1.Text) > int.Parse(vp_Y2.Text))
            {
                MessageBox.Show("A primeira coordenada deve ser menor do que a segunda.");
                return false;
            }
            return true;
        }

        private bool viewportDesenhada = false;

        // Evento de desenho da viewport.
        private void desenharViewport_Click(object sender, RoutedEventArgs e)
        {
            if (validacaoImagem() && validacaoCamposViewport())
            {
                desenharViewport();
            }
        }

        private void desenharViewport()
        {
            // Matriz de viewport.
            List<double[]> viewport = new List<double[]>();
            viewport.Add(new double[] { double.Parse(vp_X1.Text), double.Parse(vp_Y1.Text) });
            viewport.Add(new double[] { double.Parse(vp_X1.Text), double.Parse(vp_Y2.Text) });
            viewport.Add(new double[] { double.Parse(vp_X2.Text), double.Parse(vp_Y2.Text) });
            viewport.Add(new double[] { double.Parse(vp_X2.Text), double.Parse(vp_Y1.Text) });

            apresentaViewportNaInterface(viewport);
        }

        private void recortarParaViewport_Click(object sender, RoutedEventArgs e)
        {
            if (Referencias.listaRetas.Count < 1)
            {
                MessageBox.Show("Você não desenhou uma reta.");
            }
            else if (!viewportDesenhada)
            {
                MessageBox.Show("Você não desenhou a viewport.");
            }
            else
            {
                List<double[]> listaDeRetasRecortadas = new List<double[]>();
                listaDeRetasRecortadas.AddRange(listaRetas);

                Recortar recortar = new Recortar();
                
                for (int i=1; i < listaRetas.Count; i++)
                {
                    recortar.recorte(
                        listaRetas[i - 1][0], listaRetas[i - 1][1], 
                        listaRetas[i][0], listaRetas[i][1],
                        double.Parse(vp_X1.Text), double.Parse(vp_X2.Text),
                        double.Parse(vp_Y1.Text), double.Parse(vp_Y2.Text));

                    // Caso as reta esteja na zona de recorte.
                    if (recortar.getNewX1() != 0.0 && recortar.getNewY1() != 0.0 && recortar.getNewX2() != 0 && recortar.getNewY2() != 0)
                    {
                        // Atualiza as coordenadas recortadas.
                        listaDeRetasRecortadas[i - 1][0] = Math.Round(recortar.getNewX1());
                        listaDeRetasRecortadas[i - 1][1] = Math.Round(recortar.getNewY1());
                        listaDeRetasRecortadas[i][0] = Math.Round(recortar.getNewX2());
                        listaDeRetasRecortadas[i][1] = Math.Round(recortar.getNewY2());
                    }
                }

                // Desenha as retas na interface.
                Retas retas = new Retas(null);
                retas.desenharRetas_PontoMedio(listaDeRetasRecortadas);
                imagem = new Imagem(retas.getImagem());
                new Functions().listaParaViewDeCoordenadasDeRetas2D();

                // Desenha a viewport na interface.
                desenharViewport();
            }
        }

        // Sequência para desenho da viewport na interface.
        private void apresentaViewportNaInterface(List<double[]> viewport)
        {
            List<double[]> listaRetasVP = new List<double[]>();
            listaRetasVP.Add(new double[] { viewport[0][0], viewport[0][1], 0 });
            listaRetasVP.Add(new double[] { viewport[1][0], viewport[1][1], 0 });
            listaRetasVP.Add(new double[] { viewport[2][0], viewport[2][1], 0 });
            listaRetasVP.Add(new double[] { viewport[3][0], viewport[3][1], 0 });
            listaRetasVP.Add(new double[] { viewport[0][0], viewport[0][1], 0 });

            Retas retas = new Retas(imagem.getBitmap());
            retas.desenharRetas_PontoMedio(listaRetasVP);
            imagem = new Imagem(retas.getImagem());
            retas.atualizarImagem();
            viewportDesenhada = true;
        }

        private void apagarCamposViewport()
        {
            vp_X1.Text = "";
            vp_X2.Text = "";
            vp_Y1.Text = "";
            vp_Y2.Text = "";
            viewportDesenhada = false;
        }

        /* ------------------- FINAL: PARTE DE RECORTE DA VIEWPORT --------------------------- */
    }
}
