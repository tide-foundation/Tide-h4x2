using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H4x2_TinySDK.Tools
{
    public static class Utils
    {
        public static T[] PadLeft<T>(this T[] data, int length, T padding = default) where T : struct
        {
            if (data.Length >= length)
                return data;

            var newArray = new T[length];
            Array.Copy(data, 0, newArray, length - data.Length, data.Length);

            return newArray;
        }
    }
}
