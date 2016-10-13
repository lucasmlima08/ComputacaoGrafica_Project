using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using ComputacaoGraficaProject.Model;
using ComputacaoGraficaProject.View;
using ComputacaoGraficaProject.Sintese.Primitivas;

namespace ComputacaoGraficaProject.Sintese.Transformacoes
{
    public class Transformacoes3D
    {
        private Bitmap imagem;
        private Ponto ponto;
        private List<List<double[]>> transformacoes = new List<List<double[]>>();
        private List<double[]> matrizTransformada;

        public Transformacoes3D()
        {
            ponto = new Ponto();
            imagem = new Bitmap(Referencias.sizeImageX, Referencias.sizeImageY);
        }

        public void method_Translacao(double translacao_X, double translacao_Y, double translacao_Z)
        {
            // Matriz da translacao
            List<double[]> matrizTranslacao = new List<double[]>();
            matrizTranslacao.Add(new double[] {1, 0, 0, translacao_X});
            matrizTranslacao.Add(new double[] { 0, 1, 0, translacao_Y });
            matrizTranslacao.Add(new double[] { 0, 0, 1 , translacao_Z });
            matrizTranslacao.Add(new double[] { 0, 0, 0, 1 });

            matrizTransformada = multiplicar(matrizTransformada, matrizTranslacao);
        }

        public void method_Escala(double x_escala, double y_escala, double z_escala)
        {
            // Matriz do Escala
            List<double[]> matrizEscala = new List<double[]>();
            matrizEscala.Add(new double[] { x_escala, 0, 0, 0});
            matrizEscala.Add(new double[] { 0, y_escala, 0, 0});
            matrizEscala.Add(new double[] { 0, 0, z_escala, 0});
            matrizEscala.Add(new double[] { 0, 0, 0, 1});

            matrizTransformada = multiplicar(matrizTransformada, matrizEscala);
        }

        public void method_Rotacao(double angulo, double eixo)
        {
            double anguloRadianos = Math.PI * angulo / 180;

            // Matriz do Rotacao
            List<double[]> matrizRotacao = new List<double[]>();

            // Se eixo = 1, rotaciona em X.
            // Se eixo = 2, rotaciona em Y.
            // Se eixo = 3, rotaciona em Z.

            if (eixo == 1) // Eixo X
            {
                matrizRotacao.Add(new double[] { 1, 0, 0, 0 });
                matrizRotacao.Add(new double[] { 0, Math.Cos(anguloRadianos), -Math.Sin(anguloRadianos), 0 });
                matrizRotacao.Add(new double[] { 0, Math.Sin(anguloRadianos), Math.Cos(anguloRadianos), 0 });
                matrizRotacao.Add(new double[] { 0, 0, 0, 1 });
            }
            else if (eixo == 2) // Eixo Y
            {
                matrizRotacao.Add(new double[] { Math.Cos(anguloRadianos), 0, Math.Sin(anguloRadianos), 0 });
                matrizRotacao.Add(new double[] { 0, 1, 0, 0 });
                matrizRotacao.Add(new double[] { -Math.Sin(anguloRadianos), 0, Math.Cos(anguloRadianos), 0 });
                matrizRotacao.Add(new double[] { 0, 0, 0, 1 });
            }
            else // Eixo Z
            {
                matrizRotacao.Add(new double[] { Math.Cos(anguloRadianos), -Math.Sin(anguloRadianos), 0, 0 });
                matrizRotacao.Add(new double[] { Math.Sin(anguloRadianos), Math.Cos(anguloRadianos), 0, 0 });
                matrizRotacao.Add(new double[] { 0, 0, 1, 0 });
                matrizRotacao.Add(new double[] { 0, 0, 0, 1 });
            }

            matrizTransformada = multiplicar(matrizTransformada, matrizRotacao);
        }

        public void method_Reflexao(double tipo)
        {
            // Se tipo = 1, reflete em XY.
            // Se tipo = 2, reflete em YZ.
            // Se tipo = 3, reflete em XZ.

            List<double[]> matrizReflexao = new List<double[]>();

            if (tipo == 1)
            {
                matrizReflexao.Add(new double[] { 1, 0, 0, 0 });
                matrizReflexao.Add(new double[] { 0, 1, 0, 0 });
                matrizReflexao.Add(new double[] { 0, 0, -1, 0 });
                matrizReflexao.Add(new double[] { 0, 0, 0, 1 });
            }
            else if (tipo == 2)
            {
                matrizReflexao.Add(new double[] { -1, 0, 0, 0 });
                matrizReflexao.Add(new double[] { 0, 1, 0, 0 });
                matrizReflexao.Add(new double[] { 0, 0, 1, 0 });
                matrizReflexao.Add(new double[] { 0, 0, 0, 1 });
            }
            else if (tipo == 3)
            {
                matrizReflexao.Add(new double[] { 1, 0, 0, 0 });
                matrizReflexao.Add(new double[] { 0, -1, 0, 0 });
                matrizReflexao.Add(new double[] { 0, 0, 1, 0 });
                matrizReflexao.Add(new double[] { 0, 0, 0, 1 });
            }

            matrizTransformada = multiplicar(matrizTransformada, matrizReflexao);
        }

        public void method_Cisalhamento(double cisalhamento_X, double cisalhamento_Y)
        {
            // Matriz do cisalhamento
            List<double[]> matrizCisalhamento = new List<double[]>();
            matrizCisalhamento.Add(new double[] { 1, 0, cisalhamento_X, 0 });
            matrizCisalhamento.Add(new double[] { 0, 1, cisalhamento_Y, 0 });
            matrizCisalhamento.Add(new double[] { 0, 0, 1, 0});
            matrizCisalhamento.Add(new double[] { 0, 0, 0, 1 });

            matrizTransformada = multiplicar(matrizTransformada, matrizCisalhamento);
        }

        // Retorna a multiplicação de duas matrizes.
        public List<double[]> multiplicar(List<double[]> matriz1, List<double[]> matriz2)
        {
            List<double[]> novaMatriz = new List<double[]>();

            int a = matriz1.Count;
            int b = matriz2[0].Length;
            double[,] array = new double[a, b];

            for (int i = 0; i < matriz1.Count; i++) // Linhas da matriz 1.
            {
                for (int j = 0; j < matriz2[0].Length; j++) // Colunas da matriz 2.
                {
                    for (int k = 0; k < matriz1[0].Length; k++)
                    { // Linhas da matriz 2. 
                        array[i, j] += (matriz1[i][k] * matriz2[k][j]);
                    }
                }
            }

            // Passando para a lista.
            for (int i = 0; i < a; i++)
            {
                double[] linha = new double[b];
                for (int j = 0; j < b; j++)
                {
                    linha[j] = array[i, j];
                }
                novaMatriz.Add(linha);
            }

            return novaMatriz;
        }

        // Retorna a matriz identidade.
        public List<double[]> matriz_identidade()
        {
            List<double[]> matrizIdentidade = new List<double[]>();
            matrizIdentidade.Add(new double[] { 1, 0, 0, 0 });
            matrizIdentidade.Add(new double[] { 0, 1, 0, 0 });
            matrizIdentidade.Add(new double[] { 0, 0, 1, 0 });
            matrizIdentidade.Add(new double[] { 0, 0, 0, 1 });

            return matrizIdentidade;
        }
        
        /* Percorre o conjunto de transformações e realiza todas em sequênca. */
        public void conjuntoDeTransformacoes(List<double[]> transformacoes)
        {
            // Explicação:
            // O array de números inteiros indica:
            // Posição 0: O número que indica qual será a transformação (1, 2, 3, 4 ou 5).
            // Posição n: Os parâmetros solicitados de acordo com a transformação.

            // Inicia a matriz transformada.
            matrizTransformada = matriz_identidade();

            for (int i=0; i < transformacoes.Count; i++)
            {
                if (transformacoes[i][0] == 1)
                {
                    method_Translacao(transformacoes[i][1], transformacoes[i][2], transformacoes[i][3]);
                }
                else if (transformacoes[i][0] == 2)
                {
                    method_Escala(transformacoes[i][1], transformacoes[i][2], transformacoes[i][2]);
                }
                else if (transformacoes[i][0] == 3)
                {
                    method_Rotacao(transformacoes[i][1], transformacoes[i][2]);
                }
                else if (transformacoes[i][0] == 4)
                {
                    method_Reflexao(transformacoes[i][1]);
                }
                else if (transformacoes[i][0] == 5)
                {
                    method_Cisalhamento(transformacoes[i][1], transformacoes[i][2]);
                }
            }

            // Atualiza a interface com as transformações.
            Referencias.listaRetas = multiplicar(Referencias.listaRetas, matrizTransformada);
            atualizarInterface();

            // Apaga as transformações.
            transformacoes.Clear();
            matrizTransformada.Clear();
        }

        // Atualiza a lista de coordenadas do objeto na imagem.
        private void atualizarInterface()
        {
            Retas retas = new Retas(imagem);
            retas.desenharRetas_PontoMedio(Referencias.listaRetas);
            retas.atualizarImagem();
            new Functions().listaParaViewDeCoordenadasDeRetas3D();
        }
    }
}
