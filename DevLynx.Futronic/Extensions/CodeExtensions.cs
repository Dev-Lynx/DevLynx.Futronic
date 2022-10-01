using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevLynx.Futronic.Extensions
{
    internal static class CodeExtensions
    {
        public static void Try(this Action action)
        {
            try
            {
                action?.Invoke();
            }
            catch
            {
            }
        }

        public static void Try(this Action action, out Exception ex)
        {
            ex = null;

            try
            {
                action?.Invoke();
            }
            catch (Exception exception)
            {
                ex = exception;
            }
        }

        public static T Try<T>(this Func<T> action, out Exception ex)
        {
            ex = null;

            try
            {
                return action.Invoke();
            }
            catch (Exception exception)
            {
                ex = exception;
                return default;
            }
        }

        public static int GenerateHashCode(object property)
        {
            var hash = 0;
            CalculateHash(ref hash, property);

            return hash;
        }

        public static int GenerateHashCode(object prop1, object prop2)
        {
            var hash = 0;

            CalculateHash(ref hash, prop1);
            CalculateHash(ref hash, prop2);

            return hash;
        }

        public static int GenerateHashCode(object prop1, object prop2, object prop3)
        {
            var hash = 0;

            CalculateHash(ref hash, prop1);
            CalculateHash(ref hash, prop2);
            CalculateHash(ref hash, prop3);

            return hash;
        }

        public static int GenerateHashCode(object prop1, object prop2, object prop3, object prop4)
        {
            var hash = 0;

            CalculateHash(ref hash, prop1);
            CalculateHash(ref hash, prop2);
            CalculateHash(ref hash, prop3);
            CalculateHash(ref hash, prop4);

            return hash;
        }

        public static int GenerateHashCode(object prop1, object prop2, object prop3, object prop4, object prop5)
        {
            var hash = 0;

            CalculateHash(ref hash, prop1);
            CalculateHash(ref hash, prop2);
            CalculateHash(ref hash, prop3);
            CalculateHash(ref hash, prop4);
            CalculateHash(ref hash, prop5);

            return hash;
        }

        public static int GenerateHashCode(object prop1, object prop2, object prop3, object prop4, object prop5, object prop6)
        {
            var hash = 0;

            CalculateHash(ref hash, prop1);
            CalculateHash(ref hash, prop2);
            CalculateHash(ref hash, prop3);
            CalculateHash(ref hash, prop4);
            CalculateHash(ref hash, prop5);
            CalculateHash(ref hash, prop6);

            return hash;
        }

        public static int GenerateHashCode(object prop1, object prop2, object prop3, object prop4, object prop5, object prop6, object prop7)
        {
            var hash = 0;

            CalculateHash(ref hash, prop1);
            CalculateHash(ref hash, prop2);
            CalculateHash(ref hash, prop3);
            CalculateHash(ref hash, prop4);
            CalculateHash(ref hash, prop5);
            CalculateHash(ref hash, prop6);
            CalculateHash(ref hash, prop7);

            return hash;
        }

        public static int GenerateHashCode(params object[] properties)
        {
            var hash = 0;

            foreach (var prop in properties)
                CalculateHash(ref hash, prop);

            return hash;
        }

        private static void CalculateHash(ref int hash, object property)
        {
            unchecked
            {
                if (property != null)
                    hash = (hash * 397) ^ property.GetHashCode();
            }
        }
    }
}
