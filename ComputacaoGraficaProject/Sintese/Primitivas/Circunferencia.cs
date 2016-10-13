using System;
using System.Windows;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using ComputacaoGraficaProject.View;

namespace ComputacaoGraficaProject.Sintese.Primitivas
{
    class Circunferencia
    {
        private Bitmap imagem;
        private Ponto ponto;

        public Circunferencia()
        {
            ponto = new Ponto();
            imagem = new Bitmap(Referencias.sizeImageX, Referencias.sizeImageY);
            
        }

        
        //-------------------CIRCUNFERENCIA PELO MÉTODO DO PONTO MÉDIO ------------------//
        public void methodPontoMedio(int raio)
        {
            int x = 0;
            int y = raio;
            double d = ((double)1 - (double)raio);

            simetriade8(x, y);

            while (y > x)
            {
                if (d < 0)
                {
                    double IncE = (2 * x) + 3;
                    d += IncE;
                }
                else
                {
                    double IncSE = (2 * (x - y)) + 5;
                    d += IncSE;
                    y--;
                }
                x++;
                simetriade8(x, y);
            }
            Referencias.functions.atualizarImagem(imagem);
        }


        //-------------------CIRCUNFERENCIA PELO MÉTODO EQUAÇÃO EXPLICITA ------------------//
        public void methodEquacaoExplicita (int raio)
        {
            int y;
            for (int x= -raio; x <= raio; x++)
            {
                y = Convert.ToInt32(Math.Sqrt((raio * raio) - (x * x)));
                imagem.SetPixel(ponto.X_MundoParaDispositivo(x), ponto.Y_MundoParaDispositivo(y), Color.Blue);
                imagem.SetPixel(ponto.X_MundoParaDispositivo(x), ponto.Y_MundoParaDispositivo(-y), Color.Blue);
                
            }
            Referencias.functions.atualizarImagem(imagem);
        }

        //-------------------CIRCUNFERENCIA PELO MÉTODO TRIGONOMETRICO ------------------//
        public void methodTrigonometrico (int raio)
        {
            double teta1 = Math.PI / 4;
            double teta0 = 0;
            int x, y;

            while (teta0 <= teta1)
            {
                if (teta0 > teta1) { break; }

                x = Convert.ToInt32(raio * Math.Cos(teta0));
                y = Convert.ToInt32(raio * Math.Sin(teta0));
                simetriade8(x, y);
                teta0 += Math.PI / 180;
            }

            Referencias.functions.atualizarImagem(imagem);
        }

        //-------------------DESENHAR ELIPSE POR PONTO MEDIO ------------------//
        public void methodElipse(int a, int b)
        {
            int x, y;
            double d1, d2;

            x = 0;
            y = b;
            d1 = b * b - a * a * b + a * a / 4.0;

            simetriade4(x, y);

            //Região 1
            while (a * a * (y - 0.5) > b * b * (x + 1))
            {
                if (d1 < 0)
                {
                    d1 = d1 + b * b * (2 * x + 3);
                    x++;
                }
                else
                {
                    d1 = d1 + b * b * (2 * x + 3) + a * a * (-2 * y + 2);
                    x++;
                    y--;
                }
                simetriade4(x, y);
            }

            d2 = b * b * (x + 0.5) * (x + 0.5) + a * a * (y - 1) * (y - 1) - a * a * b * b;
            
            //Região 2
            while (y > 0)
            {
                if (d2 < 0)
                {
                    d2 = d2 + b * b * (2 * x + 2) + a * a * (-2 * y + 3);
                    x++;
                    y--;
                }
                else
                {
                    d2 = d2 + a * a * (-2 * y + 3);
                    y--;
                }
                simetriade4(x, y);
            }

            Referencias.functions.atualizarImagem(imagem);
        }



        private void simetriade8(int x, int y)
        {
            ponto.plotarPixel(x, y, Color.Blue, imagem);
            ponto.plotarPixel(y, x, Color.Blue, imagem);
            ponto.plotarPixel(y, -x, Color.Blue, imagem);
            ponto.plotarPixel(x, -y, Color.Blue, imagem);
            ponto.plotarPixel(-x, -y, Color.Blue, imagem);
            ponto.plotarPixel(-y, -x, Color.Blue, imagem);
            ponto.plotarPixel(-y, x, Color.Blue, imagem);
            ponto.plotarPixel(-x, y, Color.Blue, imagem);
        }

        private void simetriade4(int x, int y)
        {
            ponto.plotarPixel(x, y, Color.Blue, imagem);
            ponto.plotarPixel(-x, y, Color.Blue, imagem);
            ponto.plotarPixel(-x, -y, Color.Blue, imagem);
            ponto.plotarPixel(x, -y, Color.Blue, imagem);
        }
    }
}
