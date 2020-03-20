using System;

namespace Dot.Core.Extension
{
    public static class StringExtensions
    {
        public static string[] ToNotEmptyLines(this string value)
        {
            return value.Split(new char[] { '\r','\n' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string[] ToLines(this string value)
        {
            return value.Split(new char[] { '\r', '\n' }, StringSplitOptions.None);
        }

        public unsafe static uint GetTime33(this string value)
        {
            uint hash = 5381;
            fixed (char* str = value)
            {
                int index = 0;
                while (*(str + index) != 0)
                {
                    hash += (hash << 5) + *(str + index);
                    index++;
                }
            }
            return (hash & 0x7FFFFFFF);
        }
    }
}
