using System;
using System.Text.RegularExpressions;

namespace Dot.Core.Extension
{
    public static class StringExtensions
    {
        /// <summary>
        /// 替换部分特殊字符为指定的字符串
        /// </summary>
        /// <param name="input"></param>
        /// <param name="replacement"></param>
        /// <returns></returns>
        public static string ReplaceSpecialCharacter(this string input, string replacement)
        {
            return Regex.Replace(input, "[ \\[ \\] \\^ \\-_*×――(^)|'$%~!@#$…&%￥—+=<>《》!！??？:：•`·、。，；,.;\"‘’“”-]", replacement);
        }

        public static string[] SplitToNotEmptyLines(this string value)
        {
            return value.Split(new char[] { '\r','\n' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string[] SplitToLines(this string value)
        {
            return value.Split(new char[] { '\r', '\n' }, StringSplitOptions.None);
        }

        public static string[] SplitToArray(this string value,char splitChar)
        {
            return value.Split(new char[] { splitChar }, StringSplitOptions.None);
        }

        public static string[] SplitToNotEmptyArray(this string value, char splitChar)
        {
            return value.Split(new char[] { splitChar }, StringSplitOptions.RemoveEmptyEntries);
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
