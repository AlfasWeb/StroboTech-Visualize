using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;
using MathNet.Numerics.IntegralTransforms;
using System.Numerics;

namespace StroboTech_Visualize
{
    public partial class Form1 : Form
    {
        private List<Dado> dadosAtuais = new List<Dado>();
        private bool mostrarX = true;
        private bool mostrarY = true;
        private bool mostrarZ = true;


        public Form1()
        {
            InitializeComponent();
        }

        public class Dado
        {
            public double Tempo { get; set; }
            public double Acel { get; set; }
            public double X { get; set; }
            public double Y { get; set; }
            public double Z { get; set; }
        }

        public class VibroRMS
        {
            private double sumSq = 0;
            private int count = 0;
            public void AddSample(double value)
            {
                sumSq += value * value;
                count++;
            }
            public double GetRMS()
            {
                if (count == 0) return 0.0;
                return Math.Sqrt(sumSq / count);
            }
            public void Reset()
            {
                sumSq = 0;
                count = 0;
            }
        }

        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Arquivos CSV (*.csv)|*.csv|Todos os arquivos (*.*)|*.*";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var linhas = File.ReadAllLines(ofd.FileName);

                    dadosAtuais = linhas.Skip(1) // pula cabeçalho
                                 .Select(l => l.Split(','))
                                 .Select(v => new Dado
                                 {
                                     // Ajuste para novo formato CSV
                                     Tempo = double.Parse(v[1].Replace('.', ','))*10,
                                     Acel = double.Parse(v[2].Replace('.', ',')),
                                     X = double.Parse(v[3].Replace('.', ',')),
                                     Y = double.Parse(v[4].Replace('.', ',')),
                                     Z = double.Parse(v[5].Replace('.', ','))
                                 }).ToList();

                    // Pega o SampleRate da primeira linha (ou converte de cada linha se variar)
                    double sampleRate = double.Parse(linhas[1].Split(',')[0].Replace('.', ','));

                    PlotarGraficoUnico(dadosAtuais);
                    AtualizarInformacoes(dadosAtuais, sampleRate);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao ler o arquivo: " + ex.Message);
                }
            }
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            dadosAtuais.Clear();

            // Cria um novo PlotModel limpo
            plotGrafico.Model = new OxyPlot.PlotModel { Title = "Aceleração X, Y e Z vs Tempo" };

            // Resetar labels
            lblPeakAcc.Text = "Pico de Aceleração: ";
            lblRmsAcc.Text = "RMS de Aceleração: ";
            lblStdDev.Text = "Desvio padrão: ";
            lblVRms.Text = "Velocidade RMS: ";
            lblFreqDom.Text = "Freq. dominante: ";

            // Resetar flags de visibilidade, se estiver usando toggle
            mostrarX = mostrarY = mostrarZ = true;

            plotGrafico.InvalidatePlot(true); // força atualizar a tela
        }
        private void BtnResetZoom_Click(object sender, EventArgs e)
        {
            if (plotGrafico.Model == null) return;

            foreach (var axis in plotGrafico.Model.Axes)
            {
                axis.Reset(); // redefine para Minimum/Maximum originais
            }

            plotGrafico.InvalidatePlot(false);
        }
        private void FiltrarEixo(string eixo)
        {
            if (dadosAtuais == null || dadosAtuais.Count == 0) return;

            switch (eixo)
            {
                case "X": mostrarX = !mostrarX; break;
                case "Y": mostrarY = !mostrarY; break;
                case "Z": mostrarZ = !mostrarZ; break;
            }

            var modelo = plotGrafico.Model ?? new OxyPlot.PlotModel { Title = "Aceleração vs Tempo" };
            modelo.Series.Clear();
            modelo.Annotations.Clear();

            // Linha zero
            modelo.Annotations.Add(new OxyPlot.Annotations.LineAnnotation
            {
                Type = OxyPlot.Annotations.LineAnnotationType.Horizontal,
                Y = 0,
                Color = OxyColors.Gray,
                LineStyle = OxyPlot.LineStyle.Dash,
                StrokeThickness = 1
            });

            // Adiciona séries visíveis
            if (mostrarX)
                modelo.Series.Add(CriarSerie("X", OxyColors.Red, d => d.X));
            if (mostrarY)
                modelo.Series.Add(CriarSerie("Y", OxyColors.Green, d => d.Y));
            if (mostrarZ)
                modelo.Series.Add(CriarSerie("Z", OxyColors.Blue, d => d.Z));

            plotGrafico.Model = modelo;
            plotGrafico.InvalidatePlot(true);
        }

        // Função auxiliar para criar série
        private OxyPlot.Series.LineSeries CriarSerie(string titulo, OxyPlot.OxyColor cor, Func<Dado, double> selector)
        {
            var serie = new OxyPlot.Series.LineSeries
            {
                Title = $"Eixo {titulo}",
                Color = cor,
                StrokeThickness = 2,
                MarkerType = MarkerType.None
            };
            foreach (var d in dadosAtuais) serie.Points.Add(new OxyPlot.DataPoint(d.Tempo, selector(d)));
            return serie;
        }


        public void AtualizarInformacoes(List<Dado> dados, double sampleRate)
        {
            if (dados == null || dados.Count == 0)
                return;

            // === Instâncias RMS por eixo ===
            VibroRMS rmsX = new VibroRMS();
            VibroRMS rmsY = new VibroRMS();
            VibroRMS rmsZ = new VibroRMS();

            // === Buffers ===
            int N = dados.Count;
            double[] acc = new double[N];
            double[] velX = new double[N];
            double[] velY = new double[N];
            double[] velZ = new double[N];
            double dt = 1.0 / sampleRate;

            // === Integração e RMS incremental ===
            for (int i = 0; i < N; i++)
            {
                double x = dados[i].X;
                double y = dados[i].Y;
                double z = dados[i].Z;

                rmsX.AddSample(x);
                rmsY.AddSample(y);
                rmsZ.AddSample(z);

                acc[i] = Math.Sqrt(x * x + y * y + z * z);

                if (i == 0)
                {
                    velX[i] = 0;
                    velY[i] = 0;
                    velZ[i] = 0;
                }
                else
                {
                    velX[i] = velX[i - 1] + x * dt;
                    velY[i] = velY[i - 1] + y * dt;
                    velZ[i] = velZ[i - 1] + z * dt;
                }
            }

            // === Cálculo RMS ===
            double rmsXval = rmsX.GetRMS();
            double rmsYval = rmsY.GetRMS();
            double rmsZval = rmsZ.GetRMS();
            double aRMS = Math.Sqrt(rmsXval * rmsXval + rmsYval * rmsYval + rmsZval * rmsZval);

            double aPeak = acc.Max();

            // === Remoção de DC ===
            double mean = acc.Average();
            double[] accNoDC = acc.Select(a => a - mean).ToArray();

            // === Desvio padrão ===
            double stdDev = Math.Sqrt(accNoDC.Select(a => a * a).Average());

            // === Velocidade RMS ===
            double vRMS = Math.Sqrt(Enumerable.Range(0, N).Select(i =>
            {
                double vMag = Math.Sqrt(velX[i] * velX[i] + velY[i] * velY[i] + velZ[i] * velZ[i]);
                return vMag * vMag;
            }).Average());

            // === FFT para frequência dominante ===
            Complex[] fftData = accNoDC.Select(a => new Complex(a, 0)).ToArray();
            Fourier.Forward(fftData, FourierOptions.Matlab);

            double[] magnitudes = fftData.Take(N / 2).Select(c => c.Magnitude).ToArray();
            int idxMax = Array.IndexOf(magnitudes, magnitudes.Max());
            double freqDom = idxMax * sampleRate / N;

            // === Exibir resultados (labels ou console) ===
            lblPeakAcc.Text = $"Pico de Aceleração: {aPeak:F3} mm/s²";
            lblRmsAcc.Text = $"RMS de Aceleração: {aRMS:F3} mm/s²";
            lblStdDev.Text = $"Desvio padrão: {stdDev:F3} mm/s²";
            lblVRms.Text = $"Velocidade RMS: {vRMS:F3} mm/s";
            lblFreqDom.Text = $"Freq. dominante: {freqDom:F3} Hz";
        }
        private void PlotarGraficoUnico(List<Dado> dados)
        {
            if (dados == null || dados.Count == 0)
                return;

            var modelo = new OxyPlot.PlotModel { Title = "Aceleração X, Y e Z vs Tempo" };

            // -------------------- Eixo X --------------------
            double tempoMin = 0;
            double tempoMax = dados.Max(d => d.Tempo);
            var eixoX = new OxyPlot.Axes.LinearAxis
            {
                Position = OxyPlot.Axes.AxisPosition.Bottom,
                Minimum = tempoMin,
                Maximum = tempoMax,
                Title = "Tempo (s)",
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                MajorGridlineColor = OxyColor.FromRgb(230, 230, 230),
                MinorGridlineColor = OxyColor.FromRgb(245, 245, 245),
                IsZoomEnabled = true,
                IsPanEnabled = true
            };
            // Evento para travar apenas eixo X
            eixoX.AxisChanged += (s, e) =>
            {
                var ax = (OxyPlot.Axes.LinearAxis)s;
                if (ax.ActualMinimum < tempoMin) ax.Zoom(tempoMin, ax.ActualMaximum);
                if (ax.ActualMaximum > tempoMax) ax.Zoom(ax.ActualMinimum, tempoMax);
            };
            modelo.Axes.Add(eixoX);

            // -------------------- Eixo Y --------------------
            double yMin = dados.Min(d => Math.Min(d.X, Math.Min(d.Y, d.Z)));
            double yMax = dados.Max(d => Math.Max(d.X, Math.Max(d.Y, d.Z)));
            var eixoY = new OxyPlot.Axes.LinearAxis
            {
                Position = OxyPlot.Axes.AxisPosition.Left,
                Minimum = yMin * 1.1, // um pouco de folga abaixo do menor valor
                Maximum = yMax * 1.1, // um pouco de folga acima do maior valor
                Title = "Aceleração (mm/s²)",
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                MajorGridlineColor = OxyColor.FromRgb(230, 230, 230),
                MinorGridlineColor = OxyColor.FromRgb(245, 245, 245),
                IsZoomEnabled = true,
                IsPanEnabled = true
            };
            modelo.Axes.Add(eixoY);

            // -------------------- Séries X/Y/Z --------------------
            var serieX = new OxyPlot.Series.LineSeries
            {
                Title = "Eixo X",
                Color = OxyColors.Red,
                StrokeThickness = 2,
                MarkerType = MarkerType.None,
                TrackerFormatString = "Eixo X\nTempo: {2:0.000}s\nValor: {4:0.000} mm/s²"
            };
            var serieY = new OxyPlot.Series.LineSeries
            {
                Title = "Eixo Y",
                Color = OxyColors.Green,
                StrokeThickness = 2,
                MarkerType = MarkerType.None,
                TrackerFormatString = "Eixo Y\nTempo: {2:0.000}s\nValor: {4:0.000} mm/s²"
            };
            var serieZ = new OxyPlot.Series.LineSeries
            {
                Title = "Eixo Z",
                Color = OxyColors.Blue,
                StrokeThickness = 2,
                MarkerType = MarkerType.None,
                TrackerFormatString = "Eixo Z\nTempo: {2:0.000}s\nValor: {4:0.000} mm/s²"
            };

            foreach (var d in dados)
            {
                serieX.Points.Add(new OxyPlot.DataPoint(d.Tempo, d.X));
                serieY.Points.Add(new OxyPlot.DataPoint(d.Tempo, d.Y));
                serieZ.Points.Add(new OxyPlot.DataPoint(d.Tempo, d.Z));
            }

            modelo.Series.Add(serieX);
            modelo.Series.Add(serieY);
            modelo.Series.Add(serieZ);

            modelo.IsLegendVisible = true;

            // Remove linhas antigas, se houver
            plotGrafico.Model.Annotations.Clear();

            plotGrafico.Model = modelo;
            plotGrafico.InvalidatePlot(true);
        }

    }
}
