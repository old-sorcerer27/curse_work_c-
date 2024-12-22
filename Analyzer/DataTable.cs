using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxAnalyzer
{
	public static class DataTable
	{
		public static List<string> GetServiceWords()
		{
			return new List<string>()
			{
				"program",
                "var",
                "begin",
				"end",
                "else",
                "for",
                "to",
                "step",
				"next",
                "readln",
                "writeln",
                "if",
				"while",
				"true",
				"false",
				"%",
                "$",
                "!",

            };

		}
		public static List<string> GetSeparators()
		{
			return new List<string>()
			{
				"!=",
				"==",
				"<",
				"<=",
				">",
				">=",
				":=",
				"+",
				"-",
				"||",
				"*",
				"/",
				"&&",
				"!",
				"{",
				"}",
				"(",
				")",
				",",
				";",
				":",
				"/*",
				"=",
				".",
				"*/",
				":="
			};

		}
	}
}
