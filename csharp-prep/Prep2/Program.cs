using System;

class Program
{
    static void Main(string[] args)
    {
        Console.Write("What is your grade? ");
        string answer = Console.ReadLine();
        int grade = int.Parse(answer);

        string letter ="";
        if (grade >= 90)
        {
            letter = "A";
        }
        else if (grade >= 80)
        {
            letter = "B";
        }
        else if (grade >= 70)
        {
            letter = "C";
        }
        else if (grade >= 60)
        {
            letter = "D";
        }
        else
        {
            letter = "F";
        }
        Console.WriteLine($"Your grade is: {letter}" );

         string sign = "";
        int lastDigit = grade % 10;

        if (letter == "A")
        {
            if (lastDigit < 3) sign = "-";  // Only A- is allowed for "A" grades
        }
        else if (letter == "F")
        {
            sign = "";  // No modifiers for F grades
        }
        else
        {
            if (lastDigit >= 7) sign = "+";
            else if (lastDigit < 3) sign = "-";
        }

        // Display the final grade with sign
        Console.WriteLine($"The final grade is: {letter}{sign}");
        
        if (grade >= 70)
        {
            Console.WriteLine("You passed!");
        }
        else
        {
            Console.WriteLine("Best Of luck, next time!");
        }
    }
}