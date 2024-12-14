using System;

public class Fraction
{
    private int Numerator;
    private int Denominator;

    public Fraction()
    {
        Numerator = 1:
        Denominator = 1:
    }
    public Fraction(int wholeNumber)
    {
        Numerator = wholeNumber;
        Denominator = 1;
    }
    public Fraction(int Numerator, int Denominator)
    {
        this.Numerator = Numerator;
        this.Denominator = Denominator;
    }
    public string GetFractionString()
    {
        return $"{Numerator}/{Denominator}";
    }
    public double GetDecimalValue()
    {
        return (double)Numerator / Denominator;
    }
}