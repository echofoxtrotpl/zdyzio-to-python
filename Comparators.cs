using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdyzioToPython
{
    internal class Comparators
    {
        public string LessThanOrEqual(object? left, object? right)
        {
            if (left is int l && right is int r)
            {
                return (l <= r).ToString();
            }

            if (left is float lf && right is float rf)
            {
                return (lf <= rf).ToString();
            }

            if (left is int lInt && right is float rFloat)
            {
                return (lInt <= rFloat).ToString();
            }

            if (left is float lFloat && right is int rInt)
            {
                return (lFloat <= rInt).ToString();
            }

            throw new Exception($"cannot compareLessThan values of types {left?.GetType()} and {right?.GetType()}.");
        }

        public string GreaterThanOrEqual(object? left, object? right)
        {
            if (left is int l && right is int r)
            {
                return (l >= r).ToString();
            }

            if (left is float lf && right is float rf)
            {
                return (lf >= rf).ToString();
            }

            if (left is int lInt && right is float rFloat)
            {
                return (lInt >= rFloat).ToString();
            }

            if (left is float lFloat && right is int rInt)
            {
                return (lFloat >= rInt).ToString();
            }

            throw new Exception($"cannot compareLessThan values of types {left?.GetType()} and {right?.GetType()}.");
        }

        public string GreaterThan(object? left, object? right)
        {
            if (left is int l && right is int r)
            {
                return (l > r).ToString();
            }

            if (left is float lf && right is float rf)
            {
                return (lf > rf).ToString();
            }

            if (left is int lInt && right is float rFloat)
            {
                return (lInt > rFloat).ToString();
            }

            if (left is float lFloat && right is int rInt)
            {
                return (lFloat > rInt).ToString();
            }

            throw new Exception($"cannot compareLessThan values of types {left?.GetType()} and {right?.GetType()}.");
        }

        public string LessThan(object? left, object? right)
        {
            if (left is int l && right is int r)
            {
                return (l < r).ToString();
            }

            if (left is float lf && right is float rf)
            {
                return (lf < rf).ToString();
            }

            if (left is int lInt && right is float rFloat)
            {
                return (lInt < rFloat).ToString();
            }

            if (left is float lFloat && right is int rInt)
            {
                return (lFloat < rInt).ToString();
            }

            throw new Exception($"cannot compareLessThan values of types {left?.GetType()} and {right?.GetType()}.");
        }

        public string Equal(object? left, object? right)
        {
            if (left is int l && right is int r)
            {
                return (l == r).ToString();
            }

            if (left is float lf && right is float rf)
            {
                return (lf == rf).ToString();
            }

            if (left is int lInt && right is float rFloat)
            {
                return (lInt == rFloat).ToString();
            }

            if (left is float lFloat && right is int rInt)
            {
                return (lFloat == rInt).ToString();
            }

            throw new Exception($"cannot compareLessThan values of types {left?.GetType()} and {right?.GetType()}.");
        }

        public string NotEqual(object? left, object? right)
        {
            if (left is int l && right is int r)
            {
                return (l != r).ToString();
            }

            if (left is float lf && right is float rf)
            {
                return (lf != rf).ToString();
            }

            if (left is int lInt && right is float rFloat)
            {
                return (lInt != rFloat).ToString();
            }

            if (left is float lFloat && right is int rInt)
            {
                return (lFloat != rInt).ToString();
            }
            //tak było
            throw new Exception($"cannot compareLessThan values of types {left?.GetType()} and {right?.GetType()}.");
        }
    }
}
