using System;
using System.Collections.Generic;
using System.Text;

namespace NewstonTools
{
    public static class GlobalFunction
    {
        /// <summary>
        /// 判断输入的字符串数组中是否有空字符串或null值
        /// True,为有空字符串或null值；False,为没有空字符串或null值
        /// </summary>
        /// <param name="strings">字符串数组</param>
        /// <returns></returns>
        public static bool StringIsNullOrEmpty(params string[] strings)
        {
            for (int i = 0; i < strings.Length; i++)
            {
                if (string.IsNullOrEmpty(strings[i]))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
