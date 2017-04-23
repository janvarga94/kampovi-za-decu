using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KampoviZaDecu.helpers
{
    public static class TypeChecks
    {
        public static bool IsNumber(this Type value)
        {
            return value == typeof(sbyte)
                || value == typeof(byte)
                || value == typeof(short)
                || value == typeof(ushort)
                || value == typeof(int)
                || value == typeof(uint)
                || value == typeof(long)
                || value == typeof(ulong)
                || value == typeof(float)
                || value == typeof(double)
                || value == typeof(decimal)
                || value ==typeof(Double)
                ;
        }
    }
}
