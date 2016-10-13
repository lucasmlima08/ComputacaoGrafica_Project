﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using ComputacaoGraficaProject.View;

namespace ComputacaoGraficaProject.Sintese.Primitivas
{
    public class Retas
    {
        private Bitmap imagem;
        private Ponto ponto;

        public Retas(Bitmap imagem)
        {
            ponto = new Ponto();

            if (imagem == null)
            {
                this.imagem = new Bitmap(Referencias.sizeImageX, Referencias.sizeImageY);
            }
            else
            {
                this.imagem = imagem;
            }
        }

        public void desenharRetas_DDA(List<double[]> retas)
        {
            if (retas.Count == 1)
            {
                ponto.plotarPixel(retas[0][0], retas[0][1], Color.Red, imagem);
                imagem = ponto.getImage();
            }
            else
            {
                for (int i=1; i < retas.Count; i++)
                {
                    methodDDA(retas[i-1], retas[i]);
                }
            }
        }

        public void desenharRetas_PontoMedio(List<double[]> retas)
        {
            if (retas.Count == 1)
            {
                ponto.plotarPixel(retas[0][0], retas[0][1], Color.Red, imagem);
                imagem = ponto.getImage();
            }
            else
            {
                for (int i = 1; i < retas.Count; i++)
                {
                    methodPontoMedio(retas[i - 1], retas[i]);
                }
            }
        }

        /** Algoritmo para traçar uma reta:
         * 
         * Δx = ABS(x1 - x2) | Δy = ABS(y1 - y2)
         * length = maior(Δx, Δy)
         * Xinc = Δx / length | Yinc = Δy / length
         * 
         * Xn = menor(x1, x2) | Yn = menor(y1, y2)
         * While: Xn < maior (x1, x2) : Xn = Xn + Xinc
         * While: Yn < maior (y1, y2) : Yn = Yn + Yinc
         * While: plotarPixel (Xn, Yn)
         */

        private void methodDDA(double[] P1, double[] P2)
        {
            double x1 = P1[0], x2 = P2[0];
            double y1 = P1[1], y2 = P2[1];
            double z1 = P1[2], z2 = P2[2];

            // Só vai se alterar, se o objeto contiver a coordenada Z diferente de 0, ou seja, se for 3D.
            x1 = x1 + z1;
            y1 = y1 + z1;
            x2 = x2 + z2;
            y2 = y2 + z2;

            double length;
            double x, y, xInc, yInc;

            length = Math.Abs(x2 - x1);
            if (Math.Abs(y2 - y1) > length)
            {
                length = Math.Abs(y2 - y1);
            }

            // Define o incremento em X
            xInc = (float)(x2 - x1) / length;
            // Define o incremento em Y
            yInc = (float)(y2 - y1) / length;

            // Só vai se alterar, se o objeto contiver a coordenada Z diferente de 0, ou seja, se for 3D.
            x = x1;
            y = y1;

            // Desenha o primeiro Pixel
            ponto.plotarPixel((int)Math.Round(x), (int)Math.Round(y), Color.Red, imagem);
            imagem = ponto.getImage();

            for (int i = 0; i < length; i++)
            {
                x += xInc;
                y += yInc;

                ponto.plotarPixel((int)Math.Round(x), (int)Math.Round(y), Color.Red, imagem);
                imagem = ponto.getImage();
            }
        }

        private void methodPontoMedio(double[] P1, double[] P2)
        {
            double x1 = P1[0], x2 = P2[0];
            double y1 = P1[1], y2 = P2[1];
            double z1 = P1[2], z2 = P2[2];

            // Só vai se alterar, se o objeto contiver a coordenada Z diferente de 0, ou seja, se for 3D.
            x1 = x1 + z1;
            y1 = y1 + z1;
            x2 = x2 + z2;
            y2 = y2 + z2;

            double dx, dy, incE, incNE, d, x, y;

            dx = Math.Abs(x2 - x1);
            dy = Math.Abs(y2 - y1);
            
            x = x1;
            y = y1;

            ponto.plotarPixel(x, y, Color.Red, imagem);
            imagem = ponto.getImage();
            
            if ((x1 < x2) && (y1 <= y2)) // 1º Quadrante
            {
                if (dx >= dy)
                {
                    d = 2 * dy - dx;
                    incE = 2 * dy;
                    incNE = 2 * (dy - dx);

                    while (x < x2)
                    {
                        if (d <= 0)
                        {
                            d = d + incE;
                            x = x + 1;
                        }
                        else
                        {
                            d = d + incNE;
                            x = x + 1;
                            y = y + 1;
                        }
                        ponto.plotarPixel(x, y, Color.Red, imagem);
                    }
                }
                else if (dx < dy)
                {
                    d = dy - 2 * dx;
                    incE = 2 * (dy - dx);
                    incNE = 2 * (-dx);

                    while (y < y2)
                    {
                        if (d < 0)
                        {
                            d = d + incE;
                            x = x + 1;
                            y = y + 1;
                        }
                        else
                        {
                            d = d + incNE;
                            y = y + 1;
                        }
                        ponto.plotarPixel(x, y, Color.Red, imagem);
                    }
                }
            }
            else if ((x1 >= x2) && (y1 < y2)) // 2º Quadrante
            {
                if (dx <= dy)
                {
                    d = dy - 2 * dx;
                    incE = 2 * (dy - dx);
                    incNE = 2 * (-dx);

                    while (y < y2)
                    {
                        if (d < 0)
                        {
                            d = d + incE;
                            x = x - 1;
                            y = y + 1;
                        }
                        else
                        {
                            d = d + incNE;
                            y = y + 1;
                        }
                        ponto.plotarPixel(x, y, Color.Red, imagem);
                    }
                }
                else if (dx > dy)
                {
                    d = 2 * dy - dx;
                    incE = 2 * dy;
                    incNE = 2 * (dy - dx);

                    while (x > x2)
                    {
                        if (d <= 0)
                        {
                            d = d + incE;
                            x = x - 1;
                        }
                        else
                        {
                            d = d + incNE;
                            x = x - 1;
                            y = y + 1;
                        }
                        ponto.plotarPixel(x, y, Color.Red, imagem);
                    }
                }
            }
            else if ((x1 > x2) && (y1 >= y2)) // 3º Quadrante
            {
                if (dx >= dy)
                {
                    d = 2 * dy - dx;
                    incE = 2 * dy;
                    incNE = 2 * (dy - dx);

                    while (x > x2)
                    {
                        if (d <= 0)
                        {
                            d = d + incE;
                            x = x - 1;
                        }
                        else
                        {
                            d = d + incNE;
                            x = x - 1;
                            y = y - 1;
                        }
                        ponto.plotarPixel(x, y, Color.Red, imagem);
                    }

                }
                else if (dx < dy)
                {
                    d = dy - 2 * dx;
                    incE = 2 * (dy - dx);
                    incNE = 2 * (-dx);

                    while (y > y2)
                    {
                        if (d < 0)
                        {
                            d = d + incE;
                            x = x - 1;
                            y = y - 1;
                        }
                        else
                        {
                            d = d + incNE;
                            y = y - 1;
                        }
                        ponto.plotarPixel(x, y, Color.Red, imagem);
                    }

                }
            }
            else if ((x1 <= x2) && (y1 > y2)) // 4º Quadrante
            {
                if (dx <= dy)
                {
                    d = dy - 2 * dx;
                    incE = 2 * (dy - dx);
                    incNE = 2 * (-dx);

                    while (y > y2)
                    {
                        if (d < 0)
                        {
                            d = d + incE;
                            x = x + 1;
                            y = y - 1;
                        }
                        else
                        {
                            d = d + incNE;
                            y = y - 1;
                        }
                        ponto.plotarPixel(x, y, Color.Red, imagem);
                    }

                }
                else if (dx > dy)
                {
                    d = 2 * dy - dx;
                    incE = 2 * dy;
                    incNE = 2 * (dy - dx);

                    while (x < x2)
                    {
                        if (d <= 0)
                        {
                            d = d + incE;
                            x = x + 1;
                        }
                        else
                        {
                            d = d + incNE;
                            x = x + 1;
                            y = y - 1;
                        }
                        ponto.plotarPixel(x, y, Color.Red, imagem);
                    }
                }
                imagem = ponto.getImage();
            }
        }
        
        public void atualizarImagem()
        {
            Referencias.functions.atualizarImagem(imagem);
        }

        public Bitmap getImagem()
        {
            return imagem;
        }
    }
}
