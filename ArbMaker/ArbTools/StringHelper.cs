using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbTools
{
    public static class StringHelper
    {
        public static void Replace(this string[] array, string lookUp, string text)
        {
            for (int i = 0; i < array.Length; i++)
                array[i] = array[i].Replace(lookUp, text);
        }

        public static void Trim(this string[] array)
        {
            for (int i = 0; i < array.Length; i++)
                array[i] = array[i].Trim();
        }
    }
}
