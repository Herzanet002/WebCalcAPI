﻿using WebCalcAPI.Contracts.Services;
using WebCalcAPI.Models;

namespace WebCalcAPI
{
    public class CalculationService : ICalculationService
    {
        public CalculationModel TwoOperandCalculate(double left, double right, string operation)
        {
            var result = HandleResult(left, right, operation);
            return new CalculationModel
            {
                LeftOperand = left,
                RightOperand = right,
                Result = result,
                Operator = operation
            };
        }

        private double HandleResult(double left, double right, string operation)
        {
            return operation switch
            {
                Operators.ADD => left + right,
                Operators.SUB => left - right,
                Operators.MUL => left * right,
                Operators.DIV => left / right,
                _ => .0
            };
        }
    }
}
