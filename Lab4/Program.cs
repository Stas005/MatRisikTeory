using ScottPlot;

public class PortfolioAnalysis
{
    static double m_rf = 0.10;
    static double s_rf = 0.0;

    static double m2 = 0.30;
    static double s2 = 0.10;
    static double s2_sq = s2 * s2;

    static double m3 = 0.45;
    static double s3 = 0.15;
    static double s3_sq = s3 * s3;

    static double rho23 = -0.8;
    static double cov23 = rho23 * s2 * s3;

    static (double m, double s) GetRiskyPortfolioMetrics(double w2)
    {
        double w3 = 1.0 - w2;
        double m_p = w2 * m2 + w3 * m3;
        double var_p = (w2 * w2 * s2_sq) + (w3 * w3 * s3_sq) + (2 * w2 * w3 * cov23);
        double s_p = Math.Sqrt(var_p);
        return (m_p, s_p);
    }

    static (double w2_mvp, double w3_mvp, double m_mvp, double s_mvp) GetMVP()
    {
        double mvp_num = s3_sq - cov23;
        double mvp_den = s2_sq + s3_sq - (2 * cov23);
        double w2_mvp = mvp_num / mvp_den;
        double w3_mvp = 1.0 - w2_mvp;
        (double m_mvp, double s_mvp) = GetRiskyPortfolioMetrics(w2_mvp);
        return (w2_mvp, w3_mvp, m_mvp, s_mvp);
    }

    public static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        (double w2_mvp, double w3_mvp, double m_mvp, double s_mvp) = GetMVP();

        Console.WriteLine("Структура ПЦП з мінімальним ризиком (MVP)");
        Console.WriteLine($"Частка A1 (x1): {0} %  (не враховується у MVP)");
        Console.WriteLine($"Частка A2 (x2): {w2_mvp} %");
        Console.WriteLine($"Частка A3 (x3): {w3_mvp} %");
        Console.WriteLine("(MVP):");
        Console.WriteLine($"Сподівана дохідність: {m_mvp} %");
        Console.WriteLine($"Ризик (σ): {s_mvp} %");

        List<double> riskPoints = new List<double>();
        List<double> returnPoints = new List<double>();
        List<double> weights_w2_list = new List<double>();

        for (double w2 = -0.5; w2 <= 1.5; w2 += 0.01)
        {
            (double m, double s) = GetRiskyPortfolioMetrics(w2);
            weights_w2_list.Add(w2);
            riskPoints.Add(s);
            returnPoints.Add(m);
        }

        double maxSharpeRatio = -double.MaxValue;
        double m_tangency = 0;
        double s_tangency = 0;

        for (int i = 0; i < riskPoints.Count; i++)
        {
            if (returnPoints[i] >= m_mvp)
            {
                double sharpeRatio = (returnPoints[i] - m_rf) / riskPoints[i];
                if (sharpeRatio > maxSharpeRatio)
                {
                    maxSharpeRatio = sharpeRatio;
                    m_tangency = returnPoints[i];
                    s_tangency = riskPoints[i];
                }
            }
        }

        var plt = new Plot();

        plt.Title("Множини допустимих та ефективних ПЦП");
        plt.XLabel("Ризик");
        plt.YLabel("Сподівана дохідність");

        plt.Legend.IsVisible = true;
        plt.Legend.Location = Alignment.LowerRight;
        plt.Grid.IsVisible = true;

        var riskyFrontier = plt.Add.Scatter(riskPoints.ToArray(), returnPoints.ToArray());
        riskyFrontier.Color = Colors.Gray;
        riskyFrontier.LineWidth = 2;
        riskyFrontier.Label = "(A2+A3)";

        double cml_x1 = s_tangency * 1.5;
        double cml_y1 = m_rf + maxSharpeRatio * cml_x1;
        var cml = plt.Add.Line(0, m_rf, cml_x1, cml_y1);
        cml.Color = Colors.Blue;
        cml.LineWidth = 3;
        cml.Label = "Ефективна множина";

        var markerA1 = plt.Add.Marker(s_rf, m_rf);
        markerA1.Label = $"A1 (Безризиковий)\nm={m_rf:P0}, s={s_rf:P0}";
        markerA1.Color = Colors.Green;
        markerA1.Size = 10;

        var markerA2 = plt.Add.Marker(s2, m2);
        markerA2.Label = $"A2\nm={m2:P0}, s={s2:P0}";
        markerA2.Color = Colors.Red.WithAlpha(200);
        markerA2.Size = 7;

        var markerA3 = plt.Add.Marker(s3, m3);
        markerA3.Label = $"A3\nm={m3:P0}, s={s3:P0}";
        markerA3.Color = Colors.Red.WithAlpha(200);
        markerA3.Size = 7;

        var markerMVP = plt.Add.Marker(s_mvp, m_mvp);
        markerMVP.Label = $"MVP\nm={m_mvp:P2}, s={s_mvp:P2}";
        markerMVP.Color = Colors.Orange;
        markerMVP.Size = 10;

        var markerTP = plt.Add.Marker(s_tangency, m_tangency);
        markerTP.Label = $"Портфель дотику \nm={m_tangency:P2}, s={s_tangency:P2}";
        markerTP.Color = Colors.Blue;
        markerTP.Size = 10;

        plt.Axes.AutoScale();
        plt.Axes.Margins(horizontal: 0.1, vertical: 0.1);

        string savePath = "Task4.png";
        string fullPath = Path.GetFullPath(savePath);
        plt.SavePng(savePath, 900, 700);
        
        Console.WriteLine("Графік успішно збережено у файл:");
        Console.WriteLine($"{fullPath}");
    }
}