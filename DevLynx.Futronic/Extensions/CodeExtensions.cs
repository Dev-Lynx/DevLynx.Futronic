using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevLynx.Futronic.Extensions
{
    public static class CodeExtensions
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
    }
}
