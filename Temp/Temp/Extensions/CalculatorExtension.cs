using Microsoft.AspNetCore.Mvc.Rendering;

namespace Temp.Extensions;

public class CalculatorExtension
{
    public static double Calculate(string first, string operation, string second)
    {
        double result = 0;
        var firstNumber = Double.Parse(first);
        var secondNumber = Double.Parse(second);
        switch (operation)
        {
            case "+":
                result = firstNumber + secondNumber;
                break;
            case "-":
                result = firstNumber - secondNumber;
                break;
            case "*":
                result = firstNumber * secondNumber;
                break;
            case "/":
                result = firstNumber / secondNumber;
                break;
        }

        return result;
    }
}