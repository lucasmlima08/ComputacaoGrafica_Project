using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using ComputacaoGraficaProject.View;

namespace ComputacaoGraficaProject.Model
{
    public class Functions
    {
        public void atualizarImagem(Bitmap imagem)
        {
            BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    imagem.GetHbitmap(),
                    IntPtr.Zero,
                    Int32Rect.Empty,
            BitmapSizeOptions.FromEmptyOptions());

            Referencias.imagemDraw.ImageSource = bitmapSource;
        }

        public void listaParaViewDeCoordenadasDeRetas2D()
        {
            //References.coordenadasDeRetas.Items.Clear();
            for (int i=0; i < Referencias.listaRetas.Count; i++)
            {
                Referencias.listViewRetas.Items.Add(new ObjectPonto2D
                {
                    X = Referencias.listaRetas[i][0] + "",
                    Y = Referencias.listaRetas[i][1] + ""
                });
            }
        }

        public void listaParaViewDeCoordenadasDeRetas3D()
        {
            //References.coordenadasDeRetas.Items.Clear();
            for (int i = 0; i < Referencias.listaRetas.Count; i++)
            {
                Referencias.listViewRetas.Items.Add(new ObjectPonto3D
                {
                    X = Referencias.listaRetas[i][0] + "",
                    Y = Referencias.listaRetas[i][1] + "",
                    Z = Referencias.listaRetas[i][2] + ""
                });
            }
        }

        public class ObjectPonto2D
        {
            public string X { get; set; }
            public string Y { get; set; }
        }

        public class ObjectPonto3D
        {
            public string X { get; set; }
            public string Y { get; set; }
            public string Z { get; set; }
        }

        public class ObjectTransformacao
        {
            public string Transformacao { get; set; }
        }
    }
}
