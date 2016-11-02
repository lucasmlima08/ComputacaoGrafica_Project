using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using ComputacaoGraficaProject.Model;
using ComputacaoGraficaProject.View;
using ComputacaoGraficaProject.Sintese.Primitivas;

namespace ComputacaoGraficaProject.Sintese.Transformacoes
{
    public class Transformacoes2D
    {
        private Bitmap imagem;
        private Ponto ponto;

        public Transformacoes2D()
        {
            ponto = new Ponto();
            imagem = new Bitmap(Referencias.sizeImageX, Referencias.sizeImageY);
        }

        // Retorna a matriz de translação.
        public List<double[]> transladar(double X, double Y)
        {
            List<double[]> matrizTranslacao = new List<double[]>();
            matrizTranslacao.Add(new double[] { 1, 0, X });
            matrizTranslacao.Add(new double[] { 0, 1, Y });
            matrizTranslacao.Add(new double[] { 0, 0, 1 });

            return matrizTranslacao;
        }

        // Retorna a matriz de escala.
        public List<double[]> escalonar(double X, double Y)
        {
            List<double[]> matrizEscala = new List<double[]>();
            matrizEscala.Add(new double[] { X, 0, 0 });
            matrizEscala.Add(new double[] { 0, Y, 0 });
            matrizEscala.Add(new double[] { 0, 0, 1 });
            
            return matrizEscala;
        }

        // Retorna a matriz de rotação.
        public List<double[]> rotacionar(double angulo)
        {
            double anguloRadianos = Math.PI * angulo / 180;
            
            List<double[]> matrizRotacao = new List<double[]>();
            matrizRotacao.Add(new double[] { Math.Cos(anguloRadianos), -Math.Sin(anguloRadianos), 0});
            matrizRotacao.Add(new double[] { Math.Sin(anguloRadianos), Math.Cos(anguloRadianos), 0});
            matrizRotacao.Add(new double[] { 0, 0, 1 });

            return matrizRotacao;
        }

        // Retorna a matriz de reflexão.
        public List<double[]> refletir(double eixo)
        {
            // Se tipo = 1, rotaciona em X.
            // Se tipo = 2, rotaciona em Y.
            // Se tipo = 3, rotaciona em X e Y.

            List<double[]> matrizReflexao = new List<double[]>();

            if (eixo == 1) // Eixo X
            {
                matrizReflexao.Add(new double[] { 1, 0, 0 });
                matrizReflexao.Add(new double[] { 0, -1, 0 });
                matrizReflexao.Add(new double[] { 0, 0, 1 });
            }
            else if (eixo == 2) // Eixo Y
            {
                matrizReflexao.Add(new double[] { -1, 0, 0 });
                matrizReflexao.Add(new double[] { 0, 1, 0 });
                matrizReflexao.Add(new double[] { 0, 0, 1 });
            }
            else if (eixo == 3) // Eixo X e Y
            {
                matrizReflexao.Add(new double[] { -1, 0, 0 });
                matrizReflexao.Add(new double[] { 0, -1, 0 });
                matrizReflexao.Add(new double[] { 0, 0, 1 });
            }

            return matrizReflexao;
        }

        // Retorna a matriz de cisalhamento.
        public List<double[]> cisalhar(double X, double Y)
        {
            List<double[]> matrizCisalhamento = new List<double[]>();
            matrizCisalhamento.Add(new double[] { 1, X, 0 });
            matrizCisalhamento.Add(new double[] { Y, 1, 0 });
            matrizCisalhamento.Add(new double[] { 0, 0, 1 });

            return matrizCisalhamento;
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
                        array[i,j] += (matriz1[i][k] * matriz2[k][j]);
                    }
                }
            }
            // Passando para a lista.
            for (int i=0; i < a; i++)
            {
                double[] linha = new double[b];
                for (int j=0; j < b; j++)
                {
                    linha[j] = array[i,j];
                }
                novaMatriz.Add(linha);
            }

            return novaMatriz;
        }
        
        // Retorna a matriz identidade.
        public List<double[]> matrizIdentidade()
        {
            List<double[]> matrizIdentidade = new List<double[]>();
            matrizIdentidade.Add(new double[] { 1, 0, 0 });
            matrizIdentidade.Add(new double[] { 0, 1, 0 });
            matrizIdentidade.Add(new double[] { 0, 0, 1 });

            return matrizIdentidade;
        }

        // Percorre o conjunto de transformações e realiza todas em sequênca.
        public void conjuntoDeTransformacoes(List<double[]> transformacoes)
        {
            // Inicia a matriz de transformação.
            List<double[]> matrizTransformada = matrizIdentidade();

            // Pega as menores coordenadas das retas.
            double X_original = 0, Y_original = 0;
            for (int i = 0; i < Referencias.listaRetas.Count; i++)
            {
                if (Referencias.listaRetas[i][0] < X_original)
                {
                    X_original = Referencias.listaRetas[i][0];
                }
                if (Referencias.listaRetas[i][1] < Y_original)
                {
                    Y_original = Referencias.listaRetas[i][1];
                }
            }

            // Translada para a origem.
            matrizTransformada = multiplicar(matrizTransformada, transladar(-X_original, -Y_original));

            // Aplica as transformações
            for (int i = 0; i < transformacoes.Count; i++)
            {
                List<double[]> transformacao = matrizIdentidade();

                // Explicação:
                // O array de números inteiros indica:
                // Posição 0: O número que indica qual será a transformação (1, 2, 3, 4 ou 5).
                // Posição n: Os parâmetros solicitados de acordo com a transformação.

                // Realiza a transformação.
                if (transformacoes[i][0] == 1)
                {
                    transformacao = transladar(transformacoes[i][1], transformacoes[i][2]);
                }
                else if (transformacoes[i][0] == 2)
                {
                    transformacao = escalonar(transformacoes[i][1], transformacoes[i][2]);
                }
                else if (transformacoes[i][0] == 3)
                {
                    transformacao = rotacionar(transformacoes[i][1]);
                }
                else if (transformacoes[i][0] == 4)
                {
                    transformacao = refletir(transformacoes[i][1]);
                }
                else if (transformacoes[i][0] == 5)
                {
                    transformacao = cisalhar(transformacoes[i][1], transformacoes[i][2]);
                }

                // Atualiza a transformação.
                matrizTransformada = multiplicar(matrizTransformada, transformacao);
            }

            // Realiza a translação inversa (Voltando para a posição original).
            matrizTransformada = multiplicar(matrizTransformada, transladar(X_original, Y_original));

            // Aplica as transformações.
            matrizTransformada = multiplicar(matrizTransformada, Referencias.matrizObjeto);

            // apresenta na interface.
            apresentarObjetoNaInterface(matrizTransformada);
            transformacoes.Clear();
        }

        // Apresenta o objeto na interface.
        private void apresentarObjetoNaInterface(List<double[]> matrizTransformada)
        {
            Referencias.matrizObjeto = matrizTransformada;

            // Lista de retas para apresentar na interface.
            List <double[]> listaRetas = new List<double[]>();
            for (int i = 0; i < Referencias.matrizObjeto[0].Length; i++)
            {
                listaRetas.Add(new double[] { Referencias.matrizObjeto[0][i], Referencias.matrizObjeto[1][i], 0 });
            }
            listaRetas.Add(new double[] { Referencias.matrizObjeto[0][0], Referencias.matrizObjeto[1][0], 0 });
            Referencias.listaRetas = listaRetas;

            // Atualiza a interface com as transformações.
            atualizarInterface();

            // Apaga as transformações.
            matrizTransformada.Clear();
        }
        
        // Atualiza a lista de coordenadas e a imagem na tela.
        private void atualizarInterface()
        {
            Retas retas = new Retas(imagem);
            retas.desenharRetas_PontoMedio(Referencias.listaRetas);
            retas.atualizarImagem();
            new Functions().listaParaViewDeCoordenadasDeRetas2D();
        }
    }
}
