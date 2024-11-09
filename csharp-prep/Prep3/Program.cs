using System;

class Program
{
    static void Main(string[] args)
    {
        //Console.Write("Pick a magic number? ");
        //int magicNumber = int.Parse(Console.ReadLine());

        Random randomGenerator = new Random();
        int magicNumber = randomGenerator.Next(1, 101);

        int guess = 2;
        while (guess != magicNumber)
        {
            Console.Write("Can you tell us your guess? ");
            guess = int.Parse(Console.ReadLine());
            if (guess < magicNumber)
            {
                Console.WriteLine("Too low!");
            }
            else if (guess > magicNumber)
            {
                Console.WriteLine("Too high!");
            }
            else
            {
                Console.WriteLine("You got it!");
            }
        }
    }
}