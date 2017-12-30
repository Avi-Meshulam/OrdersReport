using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Orders
{
    static class Utils
    {
        public static IEnumerable<DependencyObject> FindLogicalChildren(DependencyObject depObj) 
        {
            if (depObj != null)
            {
                foreach (object child in LogicalTreeHelper.GetChildren(depObj))
                {
                    if (child is DependencyObject)
                    {
                        yield return child as DependencyObject;

                        foreach (var childOfChild in FindLogicalChildren(child as DependencyObject))
                        {
                            yield return childOfChild;
                        }
                    }
                }
            }
        }

        public static string GetInnerExceptions(this Exception ex)
        {
            if (ex == null)
            {
                throw new ArgumentNullException("ex");
            }

            StringBuilder sb = new StringBuilder();
            do
            {
                if (sb.ToString().Contains(ex.Message))
                    break;
                sb.AppendFormat($"{ex.Message}\n");
                ex = ex.InnerException;
            }
            while (ex != null);

            return sb.ToString();
        }
    }
}
