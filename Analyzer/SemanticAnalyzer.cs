using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SyntaxAnalyzer
{
    class SemanticAnalyzer
    {
        private List<string> operations = new List<string> { "!=", "==", "<", "<=", ">", ">=", "+", "-", "||", "*", "/", "&&", "!" };
        private Form1 _form;

        public SemanticAnalyzer(Form1 form)
        {
            _form = form;
        }

       public void StartSemanticAnalyzer()
        {
            if (!CheckInitialized())
            {
                _form.CatchError($"Не обьявленая переменная");
            }
            if (!CheckDiv())
            {
                _form.CatchError($"Нельзя присвоить интовой переменной вещественный результат деления");
            }
            if (!CheckAssignment())
			{
                _form.CatchError($"Переменной задается не верный тип");
            }
        }
        public bool CheckAssignment()
        {
            foreach (var item in _form.operationsAssignments)
            {
                string[] itemArr = item.Split(' ');
                string type = "";
                string id = itemArr[0];
                if (_form._initializedVariables.ContainsKey(id))
                {
                    type = _form._initializedVariables[id];
                }
               

                for (int i = 1; i < itemArr.Length; i++)
                {
                    if (_form._numberWithType.ContainsKey(itemArr[i]) || itemArr[i] == "true" || itemArr[i] == "false")
                    {
                        if (((itemArr[i] == "true" || itemArr[i] == "false") && type != "!") || (_form._numberWithType[itemArr[i]] != type))
                        {
                            return false;
                        }
                    }
                    else if (_form._initializedVariables.ContainsKey(itemArr[i]))
                    {
                        if (_form._initializedVariables[itemArr[i]] != type)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        public bool CheckInitialized()
        {
    
            foreach (var item in _form.operationsAssignments)
            {
                string[] itemArr = item.Split(' ');
                for (int i = 0; i < itemArr.Length; i++)
                {
                   
                    if (!_form._initializedVariables.ContainsKey(itemArr[i])
                        && !_form._numberWithType.ContainsKey(itemArr[i]) 
                        && itemArr[i] != ":="
                        && !operations.Contains(itemArr[i])
                        && itemArr[i] != "true"
                        && itemArr[i] != "false")
                    {
                        return false;
                    }
                }
            }

            foreach (var item in _form.expression)
            {
                string[] itemArr = item.Split(' ');
                for (int i = 0; i < itemArr.Length; i++)
                {
                   
                    if (!_form._initializedVariables.ContainsKey(itemArr[i])
                        && !_form._numberWithType.ContainsKey(itemArr[i])
                        && !operations.Contains(itemArr[i])
                        && itemArr[i] != "true"
                        && itemArr[i] != "false")
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public bool CheckDiv()
        {
            foreach (var item in _form.operationsAssignments)
            {
                
                string[] itemArr = item.Split(' ');
                string type = "";
                string id = itemArr[0];
                if (_form._initializedVariables.ContainsKey(id))
                {
                    type = _form._initializedVariables[id];
                }

                for (int i = 1; i < itemArr.Length; i++)
                {
                    if (itemArr[i] == "/" && (type == "%" || type == "!"))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
