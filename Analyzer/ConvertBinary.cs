using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxAnalyzer
{
	public static class ConvertBinary
	{
        
        public static string ConvertImplicitDecimal(string num)
        {
            return Convert.ToString(Convert.ToInt32(num), 2);
        }
        public static string ConvertDecimal(string num)
		{
			return Convert.ToString(Convert.ToInt32(num), 2);
		}
		public static string ConvertOctal(string num)
		{
			return Convert.ToString(Convert.ToInt32(num, 8), 2);
		}
		public static string ConvertHEX(string num)
		{
			return Convert.ToString(Convert.ToInt32(num, 16), 2);
		}
		public static string ConvertReal(string num)
		{
            string res = "";
            foreach (var item in FloatToBin((float)Double.Parse(num, new CultureInfo("en-us"))))
            {
                res += item;
            }
            return res;
		}

        public static string ConvertExponential(string num)
        {
            decimal d = Decimal.Parse(num, System.Globalization.NumberStyles.Float, new CultureInfo("en-us"));
            string res = "";
            foreach (var item in FloatToBin((float) d))
            {
                res += item;
            }
            return res;
        }


        private static List<string> FloatToBin(float num)
        {
            List<string> list = new List<string>();

            int intPart = (int)num;
            string s1 = Convert.ToString(intPart, 2);
            list.Add(s1);

            float floatPart = num - intPart;
            list.Add(".");
            string s2 = string.Empty;
            for (int i = 0; i < 8; i++)
            {
                floatPart = floatPart * 2;
                if (floatPart >= 1)
                {
                    s2 += 1;
                    floatPart = floatPart - 1;
                }
                else
                {
                    s2 += 0;
                }
            }
            list.Add(s2);

            return list;
        }
    }
}
