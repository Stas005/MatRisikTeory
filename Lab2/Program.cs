class Lab2
{
    static void Main()
    {
        int losses1 = 300, losses2 = 100, losses3 = 200, losses4 = 400;
        double p1 = 0.2, p2 = 0.3, p3 = 0.1, p4 = 0.4;
        double wl1, wl2, wl3, wl4, wl;
        double v1, v2, v3, v4, v;
        double s;

        wl1 = losses1 * p1;
        wl2 = losses2 * p2;
        wl3 = losses3 * p3;
        wl4 = losses4 * p4;

        wl = wl1 + wl2 + wl3 + wl4;

        Console.WriteLine($"Waited loss 1:{wl1}, Waited loss 2:{wl2}, Waited loss 3:{wl3}, Waited loss 4:{wl4}, Waited loss :{wl}.");

        v1 = p1 * (wl1 - wl) * (wl1 - wl);
        v2 = p2 * (wl2 - wl) * (wl2 - wl);
        v3 = p3 * (wl3 - wl) * (wl3 - wl);
        v4 = p4 * (wl4 - wl) * (wl4 - wl);

        v = v1 + v2 + v3 + v4;

        Console.WriteLine($"Variation 1:{v1}, Variation 2:{v2}, Variation 3:{v3}, Variation 4:{v4}, Variation:{v}");

        s = Math.Sqrt(v);

        Console.WriteLine($"Sigma:{s}");
    }
}