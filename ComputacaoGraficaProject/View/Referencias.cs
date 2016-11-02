﻿using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using ComputacaoGraficaProject.Model;

namespace ComputacaoGraficaProject.View
{
    public static class Referencias
    {
        public static Functions functions = new Functions();

        public static ImageBrush imagemDraw;
        public static ImageBrush imageDrawAbscissas;

        public static int sizeImageX;
        public static int sizeImageY;

        public static List<double[]> matrizObjeto;

        public static ListView listViewRetas;
        public static List<double[]> listaRetas;

        public static ListView listViewTransformacoes;
        public static List<double[]> listaTransformacoes;
    }
}
