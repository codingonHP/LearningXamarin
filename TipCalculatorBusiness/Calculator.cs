using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TipCalculatorBusiness
{
    public class Calculator
    {
        public double CalculateTotalPayable(double billTotalAmount, int percentage)
        {
            double tipAmount = CalculateTipAmount(billTotalAmount, percentage);
            return billTotalAmount + tipAmount;
        }

        public double CalculateTipAmount(double billTotalAmount, int percentage)
        {
            return billTotalAmount * percentage * 0.01;
        }
    }
}
