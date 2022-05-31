using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdyzioToPython
{
    internal class Operators
    {
        public string Multiply(object? left, object? right)
        {
            if (left is int l && right is int r)
            {
                return (l * r).ToString();
            }

            if (left is float lf && right is float rf)
            {
                return (lf * rf).ToString();
            }

            if (left is int lInt && right is float rFloat)
            {
                return (lInt * rFloat).ToString();
            }

            if (left is float lFloat && right is int rInt)
            {
                return (lFloat * rInt).ToString();
            }

            throw new Exception($"cannot add values of types {left?.GetType()} and {right?.GetType()}.");
        }

        public string Divide(object? left, object? right)
        {
            if (left is int l && right is int r)
            {
                return (l / r).ToString();
            }

            if (left is float lf && right is float rf)
            {
                return (lf / rf).ToString();
            }

            if (left is int lInt && right is float rFloat)
            {
                return (lInt / rFloat).ToString();
            }

            if (left is float lFloat && right is int rInt)
            {
                return (lFloat / rInt).ToString();
            }

            throw new Exception($"cannot add values of types {left?.GetType()} and {right?.GetType()}.");
        }

        public string Modulo(object? left, object? right)
        {
            if (left is int l && right is int r)
            {
                return (l % r).ToString();
            }

            throw new Exception($"cannot add values of types {left?.GetType()} and {right?.GetType()}.");
        }

        public string Add(object? left, object? right)
        {
            if (left is int l && right is int r)
            {
                return (l + r).ToString();
            }

            if (left is float lf && right is float rf)
            {
                return (lf + rf).ToString();
            }

            if (left is int lInt && right is float rFloat)
            {
                return (lInt + rFloat).ToString();
            }

            if (left is float lFloat && right is int rInt)
            {
                return (lFloat + rInt).ToString();
            }

            if (left is string || right is string)
            {
                return $"{left}{right}";
            }

            throw new Exception($"cannot add values of types {left?.GetType()} and {right?.GetType()}.");
        }

        public string Substract(object? left, object? right)
        {
            if (left is int l && right is int r)
            {
                return (l - r).ToString();
            }

            if (left is float lf && right is float rf)
            {
                return (lf - rf).ToString();
            }

            if (left is int lInt && right is float rFloat)
            {
                return (lInt - rFloat).ToString();
            }

            if (left is float lFloat && right is int rInt)
            {
                return (lFloat - rInt).ToString();
            }

            throw new Exception($"cannot add values of types {left?.GetType()} and {right?.GetType()}.");
        }

        public string Exponent(object? left, object? right)
        {
            if (left is int l && right is int r)
            {
                return Math.Pow(l, r).ToString();
            }

            if (left is float lf && right is float rf)
            {
                return Math.Pow(lf, rf).ToString();
            }

            if (left is int lInt && right is float rFloat)
            {
                return Math.Pow(lInt, rFloat).ToString();
            }

            if (left is float lFloat && right is int rInt)
            {
                return Math.Pow(lFloat, rInt).ToString();
            }

            throw new Exception($"cannot add values of types {left?.GetType()} and {right?.GetType()}.");
        }
    }
}
