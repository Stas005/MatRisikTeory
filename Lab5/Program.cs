class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        double[,] matrix =
        {
            { 6.0, 6.2, 5.5, 5.4, 5.0 },
            { 7.5, 7.1, 7.0, 6.8, 6.0 },
            { 7.4, 7.5, 8.0, 7.7, 5.0 },
            { 7.0, 5.8, 6.0, 6.2, 6.4 }
        };

        double[] p = { 0.1, 0.3, 0.4, 0.15, 0.05 };

        var bayes = BayesCriterion(matrix, p);
        Console.WriteLine("Баєс:");
        for (int i = 0; i < bayes.Length; i++)
            Console.WriteLine($"x{i + 1}: {bayes[i]:F3}");
        Console.WriteLine($"Оптимальний: x{Array.IndexOf(bayes, bayes.Max()) + 1}\n");

        var loss = MinExpectedLossCriterion(matrix, p);
        Console.WriteLine("Мінімальне очікуване несприятливе відхилення:");
        for (int i = 0; i < loss.Length; i++)
            Console.WriteLine($"x{i + 1}: {loss[i]:F3}");
        Console.WriteLine($"Оптимальний: x{Array.IndexOf(loss, loss.Min()) + 1}");
    }

    static double[] BayesCriterion(double[,] matrix, double[] p)
    {
        int a = matrix.GetLength(0);
        int s = matrix.GetLength(1);
        double[] result = new double[a];

        for (int i = 0; i < a; i++)
        {
            double sum = 0;
            for (int j = 0; j < s; j++)
                sum += matrix[i, j] * p[j];
            result[i] = sum;
        }

        return result;
    }

    static double[] MinExpectedLossCriterion(double[,] matrix, double[] p)
    {
        int a = matrix.GetLength(0);
        int s = matrix.GetLength(1);

        double[] bestInState = new double[s];
        for (int j = 0; j < s; j++)
        {
            double maxVal = matrix[0, j];
            for (int i = 1; i < a; i++)
                if (matrix[i, j] > maxVal)
                    maxVal = matrix[i, j];

            bestInState[j] = maxVal;
        }

        double[] result = new double[a];
        for (int i = 0; i < a; i++)
        {
            double expected = 0;
            for (int j = 0; j < s; j++)
            {
                double loss = bestInState[j] - matrix[i, j];
                expected += loss * p[j];
            }
            result[i] = expected;
        }

        return result;
    }
}