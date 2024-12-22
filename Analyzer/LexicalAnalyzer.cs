using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace SyntaxAnalyzer
{
	public class LexicalAnalyzer
	{
		private List<string> _arrResult = new List<string>();
        private Form1 _form;
        public LexicalAnalyzer(Form1 form)
		{
			_form = form;
		}

		public string StartLexicalAnalyzer(string programStr)
		{
			string result = "";
			bool hasEnd = false;
			string[] programStrArr = programStr.Split(' ');
			for (int i = 0; i < programStrArr.Length; i++)
			{
				if (programStrArr[i] == "end" && programStrArr[i + 1] == ".")
				{
                    _arrResult.Add($"[0, {CheckServiceWord(programStrArr[i])}]");
                    _arrResult.Add($"[0, {CheckSeparator(programStrArr[i + 1])}]");
                    hasEnd = true;
					break;
				}

				int serviceWordId = CheckServiceWord(programStrArr[i]);
				int separatorId = CheckSeparator(programStrArr[i]);
				int identificatorId = CheckIdentificator(programStrArr[i]);
				int numberId = CheckNumber(programStrArr[i]);

				if (serviceWordId > -1)
				{
					_arrResult.Add($"[0, {serviceWordId}]");
				}
				else if (separatorId > -1)
				{
					_arrResult.Add($"[1, {separatorId}]");
				}
				else if (identificatorId > -1)
				{
					_arrResult.Add($"[3, {identificatorId}]");
				}
				else if (numberId > -1)
				{
					_arrResult.Add($"[2, {numberId}]");
				}
				else
				{
					_form.CatchError($"Неопределенная лексема {programStrArr[i]}");
				}
			}

			if (!hasEnd)
			{
				_form.CatchError("нет окончания программы");
			}

			foreach (var item in _arrResult)
			{
				result += $"{item}\n";
			}
			return result;
		}

		public int CheckServiceWord(string str)
		{
            return _form.serviceWords.FindIndex(x => x.word == str);
        }
		public int CheckSeparator(string str)
		{
            return _form.separators.FindIndex(x => x.separator == str);
		}		

		public int CheckIdentificator(string str)
		{
			int identificatorId = _form._identificators.FindIndex(x => x.identificator == str);
			if (identificatorId > -1)
			{
				return identificatorId;
			}
			else
			{
				if (CheckWord(str) && _form.separators.FindIndex(x => x.separator == str) < 0 && _form.serviceWords.FindIndex(x => x.word == str) < 0)
				{
                    _form.identificators.Add(str);
					if (_form._identificators.Count == 0)
					{
                        _form._identificators.Add(new Identificator(0 , str));
						return _form._identificators.Last().id;
					}
					else
					{
						int lastIndex = _form._identificators.Last().id;
                        _form._identificators.Add(new Identificator(lastIndex + 1, str));
						return _form._identificators.Last().id;
					}					
				}
				else
				{
					return -1;
				}
			}
		}

        public bool CheckWord(string str)
		{
			for (int i = 0; i < str.Length; i++)
			{
				if ((str[i] < 'A' || str[i] > 'Z') && (str[i] < 'a' || str[i] >'z'))
				{
					return false;
				}
			}
			return true;
		}

        public int CheckNumber(string str)
		{
			if (CheckNumeral(str))
			{
				char NumCC = ValidateNum(str);
				if (NumCC != '-')
				{
					_form.numbers.Add(str); // выводим на форму лист чисел
					string num = str;
					if (NumCC != 'i')
					{
                        num = str.Remove(str.Length - 1);
                    } 
					string binaryNum = ConvertNum(num, NumCC);
					int numberId = _form._numbers.FindIndex(x => x.number == binaryNum);
					if (numberId == -1)
					{
						if (NumCC == 'r' || NumCC == 'e') // создаем словарь с переменными и их типами.
							_form._numberWithType.Add(str, "$");
						else
                            _form._numberWithType.Add(str, "%");

						if (_form._numbers.Count == 0)
						{
                            _form._numbers.Add(new Number(0, binaryNum));
							return _form._numbers.Last().id;
						}
						else
						{
							int lastIndex = _form._numbers.Last().id;
                            _form._numbers.Add(new Number(lastIndex + 1, binaryNum));
							return _form._numbers.Last().id;
						}
					}
					else
					{
						return numberId;
					}
				}
			}
			return -1;
		}

        public string ConvertNum(string num, char numCC)
		{
			switch (numCC)
			{
				case 'b':
					return num;
				case 'd':
					return ConvertBinary.ConvertDecimal(num);
				case 'o':
					return ConvertBinary.ConvertOctal(num);
				case 'h':
					return ConvertBinary.ConvertHEX(num);
				case 'r':
					return ConvertBinary.ConvertReal(num);
				case 'e':
					return ConvertBinary.ConvertExponential(num);
				default:
                    //return "";
                    return ConvertBinary.ConvertImplicitDecimal(num);
            }
		}

		/// <summary>
		/// Есть ли число в строке.
		/// </summary>
		public bool CheckNumeral(string str)
		{
			for (int i = 0; i < str.Length; i++)
			{
				if (str[i] >= '0' && str[i] <= '9')
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Проверка валидности числа.
		/// </summary>
		public char ValidateNum(string str)
		{
			if (str.Last()=='b' || str.Last() == 'B')
			{
				return CheckBinatyNumber(str, str.Last()) ? 'b' : '-';
			}
			else if (str.Last() == 'o' || str.Last() == 'O')
			{
				return CheckOctalNumber(str, str.Last()) ? 'o' : '-';
			}
			else if (str.Last() == 'd' || str.Last() == 'D')
			{
				return CheckDecimalNumber(str, str.Last()) ? 'd' : '-';
			}
			else if (str.Last() == 'h' || str.Last() == 'H')
			{
				return CheckHEXNumber(str, str.Last()) ? 'h' : '-';
			}
			else if (str.Last() >= '0' && str.Last() <= '9')
			{
				if (CheckRealNumber(str))
				{
					return 'r';
				}
				else if (CheckExponentialFormNumber(str))
				{
					return 'e';
				}
				else
				{
					return 'i';
				}
			} 
			else
			{
				return '-';
            }
		}

		/// <summary>
		/// Проверка Exp формы.
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public bool CheckExponentialFormNumber(string str)
		{
			if (((str[0] >= '0' && str[0] <= '9') || str[0] == '.') && (str.Contains('e') || str.Contains('E')))
			{
				char firstSymbol = str[0];
				bool indexE = false;
				bool indexpPoin = false;
				bool indexPlusOrMin = false;
				if (firstSymbol == '.' && (str[1] == 'e' || str[1] == 'E'))
				{
					return false;
				}

				for (int i = 1; i < str.Length; i++)
				{
					if ((str[i] < '0' || str[i] > '9') && str[i] != 'e' && str[i] != 'E' && str[i] != '+' && str[i] != '-' && str[i] != '.')
					{
						return false;
					}

					if (str[i] == '.')
					{
						if (indexE || indexpPoin || indexPlusOrMin)
						{
							return false;
						}
						else
						{
							indexpPoin = true;
						}
					}

					if (str[i] == 'e' || str[i] == 'E')
					{
						if (indexE || indexPlusOrMin)
						{
							return false;
						}
						else
						{
							indexE = true;
						}
					}

					if (str[i] == '+' || str[i] == '-')
					{
						if (str[i-1] != 'e' && str[i - 1] != 'E')
						{
							return false;
						}
						else
						{
							indexPlusOrMin = true;
						}
					}
				}
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Проверка на вещественное число
		/// </summary>
		public bool CheckRealNumber(string str)
		{
			bool point = false;

			for (int i = 0; i < str.Length; i++)
			{
				if (str[i] == '.')
				{
					point = true;
					continue;
				}

				if (((str[i] < '0' || str[i] > '9') && str[i] != '.') || (point && str[i] == '.'))
				{
					return false;
				}
			}
			return point;
		}

		/// <summary>
		/// Проверка на двоичное число
		/// </summary>
		public bool CheckBinatyNumber(string str, char lastChar)
		{
			int i = 0;
			while (str[i] != lastChar)
			{
				if (str[i] != '0' && str[i] != '1')
				{
					return false;
				}
				i++;
			}
			return true;
		}

		/// <summary>
		/// Проверка на восьмеричное число
		/// </summary>
		public bool CheckOctalNumber(string str, char lastChar)
		{
			int i = 0;
			while (str[i] != lastChar)
			{
				if (str[i] < '0' && str[i] > '7')
				{
					return false;
				}
				i++;
			}
			return true;
		}

		/// <summary>
		/// Проверка на десятичное число
		/// </summary>
		public bool CheckDecimalNumber(string str, char lastChar)
		{
			int i = 0;
			while (str[i] != lastChar )
			{
				if (str[i] < '0' || str[i] > '9')
				{
					return false;
				}
				i++;
			}
			return true;
		}

		/// <summary>
		/// Проверка на шестнадцатеричное число
		/// </summary>
		public bool CheckHEXNumber(string str, char lastChar)
		{
			if (str[0] < '0' || str[0] > '9')
			{
				return false;
			}
			int i = 1;
			while (str[i] != lastChar)
			{
				if ((str[i] < '0' || str[i] > '9') && (str[i] < 'A' || str[i] > 'F') && (str[i] < 'a' || str[i] > 'f'))
				{
					return false;
				}
				i++;
			}
			return true;
		}

	}

	public enum TypeTable
	{
		ServiceWord,
		Separators,
		Number,
		Identifier,
		Non
	}
}
