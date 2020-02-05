using System;


class Program
{
    static void Main()
    {
        int firstNumber = 5;
        int secondNumber = 10;
        firstNumber += secondNumber;
        Console.WriteLine(firstNumber); //this adds the second number to the first and sets that to the value of the first
        secondNumber *= firstNumber;
        Console.WriteLine(secondNumber); //this multiplies the first and second number and assigns that value to the second number
        Console.WriteLine("Emery loves Little Ceasers Pizza");
    }
}
