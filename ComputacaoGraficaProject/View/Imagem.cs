using System;
using System.Windows;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Media.Imaging;
using ComputacaoGraficaProject.View;

namespace ComputacaoGraficaProject.View
{
    class Imagem
    {
        private Bitmap imagem;

        public Imagem()
        {
            imagem = new Bitmap(Referencias.sizeImageX, Referencias.sizeImageY);

            using (Graphics graph = Graphics.FromImage(imagem))
            {
                Rectangle ImageSize = new Rectangle(0, 0, 1024, 1024);
                graph.FillRectangle(Brushes.White, ImageSize);
            }
        }

        // Desenhar as abscissas 2D.
        public void desenharAbscissas2D()
        {
            int x = Convert.ToInt32(imagem.Width / 2);
            int y = Convert.ToInt32(imagem.Height / 2);

            // Abscissa X
            for (int i = 0; i < imagem.Width; i++)
            {
                imagem.SetPixel(i, y, Color.Pink);
            }

            // Abscissa Y
            for (int j = 0; j < imagem.Height; j++)
            {
                imagem.SetPixel(x, j, Color.Pink);
            }

            // Atualiza a imagem.
            atualizarImagem();
        }

        // Desenhar as abscissas 3D.
        public void desenharAbscissas3D()
        {
            int x = Convert.ToInt32(imagem.Width / 2);
            int y = Convert.ToInt32(imagem.Height / 2);

            // Abscissa X
            for (int i = 0; i < imagem.Width; i++)
            {
                imagem.SetPixel(i, y, Color.Pink);
            }
            
            // Abscissa Y
            for (int i = 0; i < imagem.Height; i++)
            {
                imagem.SetPixel(x, i, Color.Pink);
            }

            // Abscissa Z
            int xAux = 0;
            int yAux = 0;
            int acm = 0;
            while (acm < x && acm < y)
            {
                xAux = x + acm;
                yAux = y - acm;
                imagem.SetPixel(xAux, yAux, Color.Pink);

                xAux = x - acm;
                yAux = y + acm;
                imagem.SetPixel(xAux, yAux, Color.Pink);

                acm++;
            }

            // Atualiza a imagem.
            atualizarImagem();
        }

        public void atualizarImagem()
        {
            BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    imagem.GetHbitmap(),
                    IntPtr.Zero,
                    Int32Rect.Empty,
            BitmapSizeOptions.FromEmptyOptions());

            Referencias.imageDrawAbscissas.ImageSource = bitmapSource;
        }
    }
}
