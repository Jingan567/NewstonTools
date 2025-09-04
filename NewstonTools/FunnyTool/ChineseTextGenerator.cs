using System;
using System.Collections.Generic;
using System.Text;

namespace NewstonTools.FunnyTool
{
    public class ChineseTextGenerator
    {
        private static readonly Random _random = new Random();

        // 中文常用字范围（覆盖90%以上常用字）
        private const string ChineseChars = "的一是在不了有和人这中大来去他天也我生作地到不都子时道就于出而学们年动自分现后以成只" +
                                            "会同民得可小上然过能心种日用发本然家回同话问方看实说国什分然么经行然前然然然然然然然然然然然";

        // 数字符号库
        private const string Numbers = "0123456789";

        /// <summary>
        /// 生成随机中文短文本（含逗号和数字）
        /// </summary>
        /// <param name="minWords">最小词组数量（默认2）</param>
        /// <param name="maxWords">最大词组数量（默认5）</param>
        /// <param name="useFullComma">使用全角逗号（默认true）</param>
        /// <returns>生成的文本</returns>
        public static string Generate(int minWords = 2, int maxWords = 5, bool useFullComma = true)
        {
            var segments = new List<string>();
            int wordCount = _random.Next(minWords, maxWords + 1);

            for (int i = 0; i < wordCount; i++)
            {
                // 生成2-4字中文词
                string word = GenerateChineseWord(2, 4);
                // 生成1-3位随机数
                string number = GenerateRandomNumber(1, 3);
                segments.Add($"{word},{number}");
            }

            string comma = useFullComma ? "，" : ",";
            return string.Join(comma, segments);
        }

        // 生成指定长度的随机中文词
        private static string GenerateChineseWord(int minLength, int maxLength)
        {
            int length = _random.Next(minLength, maxLength + 1);
            var word = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                int index = _random.Next(ChineseChars.Length);
                word.Append(ChineseChars[index]);
            }

            return word.ToString();
        }

        // 生成指定位数的随机数字
        private static string GenerateRandomNumber(int minDigits, int maxDigits)
        {
            int digits = _random.Next(minDigits, maxDigits + 1);
            var number = new StringBuilder(digits);

            for (int i = 0; i < digits; i++)
            {
                int index = _random.Next(Numbers.Length);
                number.Append(Numbers[index]);
            }

            return number.ToString();
        }

        // 测试用例
        private static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine("示例1：默认配置生成");
            Console.WriteLine(Generate());

            Console.WriteLine("\n示例2：更多词组+半角逗号");
            Console.WriteLine(Generate(3, 6, false));

            Console.WriteLine("\n示例3：最小词组+全角逗号");
            Console.WriteLine(Generate(2, 2));

            Console.WriteLine("\n示例4：长数字+混合长度");
            Console.WriteLine(Generate(4, 4));
        }
    }
}
