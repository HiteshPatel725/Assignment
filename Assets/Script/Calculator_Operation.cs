using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public interface ICalculate
{
    double Apply(double left, double right);
}
public class Addition : ICalculate
{
    public double Apply(double left, double right)
    {
        return left + right;
    }
}

public class Subtraction : ICalculate
{
    public double Apply(double left, double right)
    {
        return left - right;
    }
}

public class Multiplication : ICalculate
{
    public double Apply(double left, double right)
    {
        return left * right;
    }
}

public class Division : ICalculate
{
    public double Apply(double left, double right)
    {
        if (right == 0)
        {
            Debug.Log("Not Divide by 0");
        }
        return left / right;
    }
}

public class Calculator_Operation : MonoBehaviour
{
    private List<string> DivideFormulaList = new List<string>();
    private List<string> DivideFormulaStep2 = new List<string>();
    public TextMeshProUGUI resultText,enterValueText;
    public string currentUserValue;
    public double result;
    ICalculate add = new Addition();
    ICalculate sub = new Subtraction();
    ICalculate mul = new Multiplication();
    ICalculate div = new Division();

    private void Start()
    {

    }

    public void OnClickClearButton()
    {
        //Remove last character
        resultText.text = "";
        if (currentUserValue.Length != 0)
        {
            currentUserValue = currentUserValue.Substring(0, currentUserValue.Length - 1);
            enterValueText.text = currentUserValue;
        }
    }
    public void OnClickResetButton()
    {
        //Reset Calculator
        DivideFormulaList.Clear();
        DivideFormulaStep2.Clear();
        resultText.text = "";
        enterValueText.text = "";
        result = 0;
        currentUserValue = "";
    }
    public void CalCulation()
    {
        //User value divided and store into list (Each value and operators in sequence)
        char lastValue = currentUserValue[currentUserValue.Length - 1];
        if (lastValue == '+' || lastValue == '-' || lastValue == '*' || lastValue == '/')
            return;
        DivideFormulaList.Clear();
        DivideFormulaStep2.Clear();
        string number = "";

        for (int i = 0; i < currentUserValue.Length; i++)
        {
            char c = currentUserValue[i];

            if (char.IsDigit(c) || c == '.')
            {
                number += c;
            }
            else if (c == '+' || c == '-' || c == '*' || c == '/')
            {
                if (number != "")
                {
                    DivideFormulaList.Add(number);
                    number = "";
                }
                DivideFormulaList.Add(c.ToString());
            }
        }
        if (number != "")
        {
            DivideFormulaList.Add(number);
        }

        //call main operation in try and catch if any operation break then get error
        try
        {
            double result = MainOperation();
            resultText.text = result.ToString();
        }
        catch
        {
            resultText.text = "Error";
        }

        enterValueText.text = "";
        
        currentUserValue = result.ToString();

    }

    double MainOperation()
    {
        //First check division and multiplication if available then solve first
        double temp = 0;
        for (int i = 0; i < DivideFormulaList.Count; i++)
        {
            string value = DivideFormulaList[i];

            if (value == "*" || value == "/")
            {
                double left = temp;
                double right = double.Parse(DivideFormulaList[i + 1]);
                if (value == "*")
                    temp = mul.Apply(left, right);
                else if (value == "/")
                    temp = div.Apply(left,right);

                i++;
            }
            else if (value == "+" || value == "-")
            {
                DivideFormulaStep2.Add(temp.ToString());
                DivideFormulaStep2.Add(value);
                temp = double.Parse(DivideFormulaList[i + 1]);
                i++;
            }
            else
            {
                temp = double.Parse(value);
            }
        }
        //After solve all division and multiplication solve addition and subtraction
        DivideFormulaStep2.Add(temp.ToString());
        string currentOperator = "+";
        for (int i = 0; i < DivideFormulaStep2.Count; i++)
        {
            string tempValue = DivideFormulaStep2[i];

            if (tempValue == "+" || tempValue == "-")
            {
                currentOperator = tempValue;
            }
            else
            {
                double value = double.Parse(tempValue);
                if (currentOperator == "+")
                    result = add.Apply(result, value);
                else if (currentOperator == "-")
                    result = sub.Apply(result, value);
            }
        }
        return result;

    }

    public void OnClickNumbersOperators(string i)
    {
        //Add numbers in main formula and user value
        result = 0;
        resultText.text = "";
        currentUserValue += i;
        enterValueText.text = currentUserValue;
    }
    public void OnClickOperators(string i)
    {
        //click operators check last character is also operator if yes then remove last charecter and add new operator
        result = 0;
        resultText.text = "";
        if (currentUserValue.Length != 0)
        {
            char lastValue = currentUserValue[currentUserValue.Length - 1];
            Debug.Log("Last Value=" + lastValue);
            if (lastValue != '+' && lastValue != '-' && lastValue != '*' && lastValue != '/')
            {
                currentUserValue += i;
                enterValueText.text = currentUserValue;
            }
            else
            {
                currentUserValue = currentUserValue.Substring(0, currentUserValue.Length - 1);
                currentUserValue += i;
                enterValueText.text = currentUserValue;
            }
        }
    }
    public void OnClickQuitButton()
    {
        Application.Quit();
    }

    
}
