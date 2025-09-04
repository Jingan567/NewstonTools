using System;
using System.Collections.Generic;
using System.Text;

namespace NewstonTools.FunnyTool
{
    public class ChineseNameGenerator
    {
        // 静态随机实例（避免重复实例化导致模式重复）
        private static readonly Random Random = new Random();

        // 常见姓氏库（精选百家姓高频姓氏）
        private static readonly string[] LastNames =
        {
        "李", "王", "张", "刘", "陈", "杨", "赵", "黄", "周", "吴",
        "徐", "孙", "胡", "朱", "高", "林", "何", "郭", "马", "罗"
        };

        // 名字用字库（按使用频率精选80字）
        private static readonly string[] FirstNameChars =
        {
        "伟", "芳", "丽", "强", "华", "敏", "静", "杰", "涛", "明",
        "艳", "磊", "军", "娜", "帅", "丹", "超", "平", "红", "波",
        "霞", "辉", "宇", "婷", "鹏", "玉", "兰", "杰", "宁", "阳",
        "菲", "斌", "倩", "健", "雪", "洋", "欣", "妍", "浩", "琪",
        "昊", "晨", "鑫", "悦", "梓", "涵", "诗", "雅", "梦", "梓",
        "萱", "子", "轩", "睿", "博", "思", "哲", "俊", "嘉", "怡",
        "然", "文", "武", "龙", "凤", "燕", "梅", "松", "柏", "竹",
        "菊", "春", "夏", "秋", "冬"
        };

        /// <summary>
        /// 生成随机中文名
        /// </summary>
        /// <param name="gender">性别倾向（0=中性，1=男性，2=女性）</param>
        /// <returns>完整中文名</returns>
        public static string Generate(int gender = 0)
        {
            // 选择姓氏
            string lastName = LastNames[Random.Next(LastNames.Length)];

            // 决定名字长度（70%概率双字名，30%单字名）
            int nameLength = Random.NextDouble() < 0.7 ? 2 : 1;

            // 构建名字
            string firstName = BuildFirstName(nameLength, gender);

            return $"{lastName}{firstName}";
        }

        // 构建名字部分（支持性别倾向）
        private static string BuildFirstName(int length, int gender)
        {
            var name = "";
            var useMaleChars = gender == 1;
            var useFemaleChars = gender == 2;

            // 性别筛选逻辑
            var validChars = new List<string>(FirstNameChars);
            if (gender == 1) validChars.RemoveAll(c => c == "芳" || c == "丽" || c == "娜");
            if (gender == 2) validChars.RemoveAll(c => c == "伟" || c == "强" || c == "军");

            for (int i = 0; i < length; i++)
            {
                name += validChars[Random.Next(validChars.Count)];
            }

            return name;
        }
    }
}
