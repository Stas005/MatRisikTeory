class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        double[,] F =
        {
            {250, 350, 150},
            {750, 200, 350},
            {255, 880, 250},
            {800, 550, 450}
        };

        double[] P = {0.1, 0.5, 0.4};
        double[] lambdas = {0.1, 0.2, 0.3};

        int n = F.GetLength(0);
        int m = F.GetLength(1);

        double[] expected = new double[n];
        for (int i = 0; i < n; i++)
        {
            double sum = 0;
            for (int j = 0; j < m; j++)
                sum += F[i, j] * P[j];
            expected[i] = sum;
        }

        double minE = expected.Min();
        double maxE = expected.Max();
        double[] normE = expected.Select(e => (e - minE) / (maxE - minE)).ToArray();

        double[] avgH = new double[n];

        foreach (var lambda in lambdas)
        {
            for (int i = 0; i < n; i++)
            {
                double min = F[i, 0];
                double max = F[i, 0];
                for (int j = 1; j < m; j++)
                {
                    min = Math.Min(min, F[i, j]);
                    max = Math.Max(max, F[i, j]);
                }
                avgH[i] += lambda * min + (1 - lambda) * max;
            }
        }

        for (int i = 0; i < n; i++)
            avgH[i] /= lambdas.Length;

        double minAvgH = avgH.Min();
        double maxAvgH = avgH.Max();
        double[] normAvgH = avgH.Select(h => (h - minAvgH) / (maxAvgH - minAvgH)).ToArray();

        double[] finalScore = new double[n];
        for (int i = 0; i < n; i++)
            finalScore[i] = 0.5 * normE[i] + 0.5 * normAvgH[i];

        int best = Array.IndexOf(finalScore, finalScore.Max());

        Console.WriteLine("Комбінована оцінка:");
        for (int i = 0; i < n; i++)
            Console.WriteLine($"X{i+1}: {finalScore[i]:F4}");

        Console.WriteLine($"\nНайкраща альтернатива: X{best + 1}");
    }
}