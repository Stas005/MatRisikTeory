class Lab1 {
    static void Main() {
        int rainyHomeMood, sunnyHomeMood, rainyForestMood, sunnyForestMood;
        double pRain;

        MoodInput("How do you feel at home when it rains.Rate it on a 10-point scale", out rainyHomeMood);
        MoodInput("How do you feel at home when the sun is shining.Rate it on a 10-point scale", out sunnyHomeMood);
        MoodInput("How do you feel in the forest when it rains.Rate it on a 10-point scale", out rainyForestMood);
        MoodInput("How do you feel in the forest when the sun is shining.Rate it on a 10-point scale", out sunnyForestMood);

        Console.WriteLine("Enter the probability of rain");

        while(true)
        {
            bool isTrue = double.TryParse(Console.ReadLine(), out pRain);

            if(isTrue && pRain >= 0 && pRain <= 1)
                break;

            Console.WriteLine("You entered the probability of rain incorrectly.Try again");
        }

        double pSun = 1 - pRain;

        double wHome = (pRain * rainyHomeMood + pSun * sunnyHomeMood);
        double wForest = (pRain * rainyForestMood + pSun * sunnyForestMood);

        Console.WriteLine("Benefit of the decision to stay at home: " + wHome);
        Console.WriteLine("Benefit of the decision to go to the forest: " + wForest);

        if(wHome >= wForest)
            Console.WriteLine("Today it's better to stay at home!!");
        else
            Console.WriteLine("Today it's better to go to the forest!!");
    }

    static void MoodInput(string inputText, out int mood)
    {
        while (true)
        {
            Console.WriteLine(inputText);
            bool isValid = int.TryParse(Console.ReadLine(), out mood);
            if (isValid && mood >= 0 && mood <= 10)
                break;
            Console.WriteLine("You entered the number incorrectly. Try again.");
        }
    }
}