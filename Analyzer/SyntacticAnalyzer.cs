using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace SyntaxAnalyzer
{
	class SyntacticAnalyzer
	{
        private List<string> operations = new List<string> { "!=", "==", "<", "<=", ">", ">=", "+", "-", "||", "*", "/", "&&", "!" };
        private Form1 _form;
        public SyntacticAnalyzer(Form1 form)
		{
            _form = form;
        }

		public void CheckProgram(string programStr)
		{
			if (!BracketCheck(programStr))
			{
				_form.CatchError($"Нарушена парность скобок");
			}

			string[] programStructure = ReferenceStrings.Program.Split(' ');
			string[] programStrArr = programStr.Split(' ');
			int p = 0;
			for (int i = 0; i < programStructure.Length; i++)
			{
				if (programStructure[i] == programStrArr[p])
				{
					p++;
					continue;
				}
				if (programStructure[i] == "{description}")
				{
					p = Description(programStrArr, p);
					if (p == -1)
					{
						_form.CatchError($"Неверное описание переменных в программе");
					}
				}

				if (programStructure[i] == "{body}")
				{
					Body(programStrArr, p);
				}
			}
		}

  
		static bool BracketCheck(string s)
		{
			string t = "[(])";
			string comment = "{";
			Stack<char> st = new Stack<char>();
			List<char> stcomment = new List<char>();

			foreach (var x in s)
			{
				int f = t.IndexOf(x);
				int com = comment.IndexOf(x);

				if (f >= 0 && f <= 1)
					st.Push(x);

				if (com > -1 && !stcomment.Contains(x))
					stcomment.Add(x);
				else if (stcomment.Contains(x))
					stcomment.Remove(x);

				if (f > 1)
				{
					if (st.Count == 0 || st.Pop() != t[f - 2])
						return false;
				}
			}

			if (st.Count != 0 || stcomment.Count != 0)
				return false;


			return true;
        }


        public int Description(string[] str, int p)
		{
			string[] descriptionStructure = ReferenceStrings.Description.Split(' ');
			List<string> tempInd = new List<string>();
			string type = "";
			for (int i = 0; i < descriptionStructure.Length; i++)
			{
				if (descriptionStructure[i] == "identifier" && _form.identificators.Contains(str[p]))
				{
					tempInd.Add(str[p]);
					p++;
				}
				else if (descriptionStructure[i] == "{,}")
				{
					if (str[p] == "," && _form.identificators.Contains(str[p + 1]))
					{
						p++;
						i -= 2;
					}
                }
				else if (descriptionStructure[i] == ":")
				{
                    if (str[p] == ":")
					{ 
						p++;
					}
                    else
                    {
                        _form.CatchError($"Неизвестный токен {str[p]}");
                        return -1;
                    }
                }
				else if (descriptionStructure[i] == "type")
				{

					if (str[p] == "%" || str[p] == "!" || str[p] == "$")
					{
						type = str[p];
						p++;
					}
					else
					{
						_form.CatchError($"Неизвестный токен {str[p]} , {i}");
						return -1;
					}
				}
				else if (descriptionStructure[i] == ";")
				{
                    while (tempInd.Count > 0)
                    {
                        _form._initializedVariables.Add(tempInd[tempInd.Count - 1], type);
                        tempInd.RemoveAt(tempInd.Count - 1);
                    }

                    p++;
                    p = Description(str, p);
                }

                if (str[p] == "begin"){
					break;
				}

            }

			return p;
		}


		public void Body(string[] str, int p)
		{
            string[] bodyStructure = ReferenceStrings.Body.Split(' ');
				p++;
				int pn = p;
				while (str[p] != "end")
				{
					while (str[p] == ";" || str[p] == "{") 
					{
						p++;
						pn++;
					}

					if (str[p] == "end")
					{
						break;
					}

					if (CheckOperator(str, ref pn))
					{
                        p = pn;
					}
					else
					{
						pn++;
						p = pn;
					}
				}
        }

		public bool CheckOperator(string[] str, ref int p)
		{
			return isAssignment(str, ref p) || isFor(str, ref p) || isIf(str, ref p) || isWhile(str, ref p) || isWriteln(str, ref p) || isReadLn(str, ref p);
		}

		public bool isIf(string[] str, ref int p)
		{
            if (str[p] == "if")
			{
				p++;
				if (str[p] == "(")
				{
					p++;
					if (isExpression(str, ref p, true))
					{
						if (str[p] == ")")
						{
							p++;
							if (CheckOperator(str, ref p))
							{
								if (str[p] == "else")
								{
									p++;
									return CheckOperator(str, ref p);
								}
								else
								{
									return true;
								}
							}
						}
					}
				}
			}
			return false;
		}

		private bool isReadLn(string[] str, ref int p)
		{
            if (str[p] == "readln")
			{
				{
					p++;
					while (str[p] != ";")
					{
                        if (_form.identificators.Contains(str[p]) || str[p] == ",")
						{
							p++;
						}
						else
						{
							return false;
						}
					}

					if (str[p] == ";")
					{
						p++;
						return true;
					}
				}
			}
			return false;
		}

		private bool isWriteln(string[] str, ref int p)
		{
            if (str[p] == "writeln")
			{
				{
					p++;
					while (str[p] != ";")
					{
						if (isExpression(str, ref p, true))
						{
							if (str[p] == ";")
							{
								p++;
								return true;
							}
							p++;
						}
						else
						{
							return false;
						}
					}
				}
			}
			return false;
		}

		private bool isWhile(string[] str, ref int p)
		{
            if (str[p] == "while")
			{
				p++;
				if (str[p] == "(")
				{
					p++;
					while (str[p] != ")")
					{
						p++;
						if (isExpression(str, ref p, true))
						{
							if (str[p] == ")")
							{
								p++;
								return (CheckOperator(str, ref p));
							}
						}
					}
				}
			}
			return false;
		}

		private bool isFor(string[] str, ref int p)
		{
            if (str[p] == "for")
			{
				p++;
				if (isAssignment(str, ref p))
				{
					if (str[p] == "to")
					{
						p++;
						if (isExpression(str, ref p, true))
						{ 
							if (str[p] == "next")
							{
								p++;
								return CheckOperator(str, ref p);
							}
						}
					}
				}
			}
			return false;
		}


		public bool isAssignment(string[] str, ref int p)
		{
			if (_form.identificators.Contains(str[p]))
			{
				int startIndex = p;
				p++;
				if (str[p] == ":=")
				{
					p++;
					if (isExpression(str, ref p))
					{
						_form.operationsAssignments.Add(string.Join(" ", str, startIndex, p - startIndex));
                        return true;
					}
					else
					{
						return false;
					}
				}
				else
				{
					return false;
				}
			}
			return false;
		}


		public bool isExpression(string[] str, ref int p, bool addExpression = false) 
		{
            if (_form.identificators.Contains(str[p]) || _form.numbers.Contains(str[p]) || str[p] == "true" || str[p] == "false")
			{
				int startIndex = p;
				p++;
				bool operation = false;
				while (str[p] != ";" && str[p] != ")" && str[p] != "(" && str[p] != "do" && str[p] != "then" && str[p] != "else" && str[p] != "to" && str[p] != "," && str[p] != "step" && str[p] != "next")
				{
                    if ((_form.identificators.Contains(str[p]) || _form.numbers.Contains(str[p])) && operation)
					{
						operation = false;
						p++;
					}
					else if (operations.Contains(str[p]) && !operation)
					{
						operation = true;
						p++;
					}
					else
					{
						return false;
					}
				}
				if (addExpression)
				{
					_form.expression.Add(string.Join(" ", str, startIndex, p - startIndex));
                }

				return true;
			}
			else
			{
				return false;
			}
			
		}

	}

	public static class ReferenceStrings
	{
		public static string Program = "program var {description} {body} end .";
        public static string Description = "identifier {,} : type ;";
        public static string Body = "begin {operator} ; end/{operator}";
	}
}
