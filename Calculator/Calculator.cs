using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
namespace Calculator
{
    public class Calculator
    {
        /// <summary>
        /// Подсчитывает рузультат выражений из файла, создаёт файл и записывает в него выражения из оригинального файла + результат.
        /// </summary>
        /// <param name="filePath">Путь к файлу с выражениями</param>
        /// <param name="destinationPath">Путь к файлу куда поместится результат выражений</param>
        public void CalculateFile(string filePath, string destinationPath)
        {
            string[] file = File.ReadAllLines(filePath);

            for (int i = 0; i < file.Length; i++) // Записываем результат в массив.
            {
                try
                {
                    file[i] += " = " + GetExpressionResult(file[i]);
                }
                catch (DivideByZeroException e)
                {
                    file[i] += " = " + e.Message;
                }
                catch (Exception)
                {
                    file[i] += " = " + "Входная строка имела неверный формат.";
                }
                
            }

            File.WriteAllLines(destinationPath, file,Encoding.Default);

        }

        /// <summary>
        /// Преобразует инфиксную запись в постфиксную.
        /// </summary>
        /// <param name="inputExpression">Строка с выражением</param>
        /// <returns>Возвращает результат выражения</returns>
        public double GetExpressionResult(string inputExpression)
        {
            List<string> sourceExpression = new List<string>();  // Выражение в List.
            List<string> reverceExpression = new List<string>(); // Выражение в польской нотации.
            Stack<string> operators = new Stack<string>();       // Стак для арифм. операций.

            //// Создаём пробелы между арифм. знаками.
            inputExpression = inputExpression.Replace("*", " * ").Replace("+", " + ").Replace("/", " / ").Replace("-(", " - (")
                                 .Replace("(", "( ").Replace(")", " )").Replace("1-", "1 - ").Replace("2-", "2 - ").Replace("3-", "3 - ")
                                 .Replace("4-", "4 - ").Replace("5-", "5 - ").Replace("6-", "6 - ").Replace("7-", "7 - ").Replace("8-", "8 - ")
                                 .Replace("9-", "9 - ").Replace("0-", "0 - ");

            // Разделяем строку по пробелам которые создали выше.
            sourceExpression = inputExpression.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();


            //sourceExpression = GetSplitExpression(inputExpression);


            for (int i = 0; i < sourceExpression.Count; i++) // Опачки, а тут алгоритм преобразующий инфиксную запись в постфиксную.
            {

                if (double.TryParse(sourceExpression[i], out double number)) // Добавляем число.
                {
                    reverceExpression.Add(sourceExpression[i]);
                }
                else if (sourceExpression[i] == "(") // Добавляем скобочку "(" в Stack.
                {
                    operators.Push(sourceExpression[i]);
                }
                else if (sourceExpression[i] == ")") // Достаём все арифм. знаки до скобочки "(" из Stack.
                {
                    while (operators.Peek() != "(")
                    {
                        reverceExpression.Add(operators.Pop());
                    }

                    operators.Pop(); // Выбрасываем скобочку "(".
                }
                // Логика добавления арифм. знаков.
                else if (sourceExpression[i] == "*" || sourceExpression[i] == "/" || sourceExpression[i] == "-" || sourceExpression[i] == "+")
                {
                    if (operators.Count > 0) // Если в Stack уже есть арифм. знаки то смотрим их приоритет.
                    {

                        if (GetPriority(sourceExpression[i]) > GetPriority(operators.Peek())) // Добавляем  в Stack если приоритет выше.
                        {
                            operators.Push(sourceExpression[i]);
                        }
                        else // Если приоритет нашего знака равен или ниже знаку на вершине стека то...
                        {
                            reverceExpression.Add(operators.Pop()); // Записываем знак из стека и...

                            operators.Push(sourceExpression[i]);    // Добавляем знак в Stack.
                        }

                    }
                    else // Если Stack пустой, записываем арифм. знак.
                    {
                        operators.Push(sourceExpression[i]);
                    }

                }

            }
            for (; operators.Count != 0;) // Достаём всё что осталось в стеке.
            {
                reverceExpression.Add(operators.Pop());
            }

            return ReverceExpressionResult(reverceExpression);
        }



        /// <summary>
        /// Подсчитывает результат из постфиксной нотации.
        /// </summary>
        /// <param name="reverceExpression">Put here expression in RPN</param>
        /// <returns>Результат выражения</returns>
        double ReverceExpressionResult(List<string> reverceExpression)
        {
            Stack<double> answer = new Stack<double>();

            foreach (var item in reverceExpression)
            {
                if (double.TryParse(item, out double number)) // Пушим число.
                {
                    answer.Push(number);
                }
                else // На основе арифм. знака выполняем логику.
                {
                    if (item == "/" && answer.Peek() == 0)
                    {
                        throw new DivideByZeroException("На ноль делить нельзя.");
                    }
                    answer.Push( Calculate( item, answer.Pop(), answer.Pop() ) );
                }
            }

            return answer.Pop();
        }



        /// <summary>
        /// Определяет приоритет арифм. знака.
        /// </summary>
        /// <param name="action">Арифметический знак</param>
        /// <returns>Приоритет арифм. знака.</returns>
        static byte GetPriority(string action)
        {
            switch (action)
            {
                case "(": return 0;
                case "+": return 1;
                case "-": return 1;
                case "*": return 2;
                case "/": return 2;
                default: return 3;
            }
        }


        /// <summary>
        /// Производит вычисления на основе арифм. знака.
        /// </summary>
        /// <param name="action">Арифметический знак.</param>
        /// <param name="secondNumber">Число справа от арифм. знака</param>
        /// <param name="firstNumber">Число слева от арифм. знака</param>
        /// <returns>Результат на основе арифм. знака</returns>
        double Calculate(string action, double secondNumber, double firstNumber)
        {
            switch (action)
            {
                case "*":
                    return firstNumber * secondNumber;
                case "/":
                    return firstNumber / secondNumber;
                case "+":
                    return firstNumber + secondNumber;
                case "-":
                    return firstNumber - secondNumber;
                default:
                    return 0;
            }
        }

        List<string> GetSplitExpression(string inputExpression)
        {
            List<string> expression = new List<string>();
            inputExpression += " "; // Костыль чтобы не споткнуться.

            for (int i = 0; i < inputExpression.Length; i++)
            {
                if (inputExpression[i] == '(' || inputExpression[i] == ')') // Добавляем скобочки.
                {
                    expression.Add(inputExpression[i].ToString());
                }
                else if (inputExpression[i] == '*' || inputExpression[i] == '/' || inputExpression[i] == '+' || inputExpression[i] == '-') // Добавялем арифм. знаки.
                {
                    // Проверка на отрицательное число в начале выражения -5*5.
                    string negativeNumber = "";
                    if (inputExpression[0] == '-' || inputExpression[i] == '-' && !char.IsNumber(inputExpression[i - 1]))
                    {
                        negativeNumber += inputExpression[i]; // Добавляем минус
                        ++i; // Переходим к числу.

                        for (; i < inputExpression.Length && char.IsNumber(inputExpression[i]) || inputExpression[i] == ','; i++)
                        {
                            negativeNumber += inputExpression[i];
                        }
                        --i; // Потому что главный цикл for плюсанёт ещё на 1.
                        expression.Add(negativeNumber);
                    }
                    else if (inputExpression[i] == '-' && char.IsNumber(inputExpression[i + 1]) && char.IsNumber(inputExpression[i - 1])) // Если слева и справа числа то это прост оминус - добавляем.
                    {
                        expression.Add(inputExpression[i].ToString()); // Добавляем просто минус.
                    }
                    else
                    {
                        expression.Add(inputExpression[i].ToString());
                    }
                }
                else if (char.IsNumber(inputExpression[i])) // Добавляем числа.
                {
                    string number = "";
                    
                    for (; i < inputExpression.Length; i++)
                    {
                        if (char.IsNumber(inputExpression[i]) || inputExpression[i] == ',')
                        {
                            number += inputExpression[i];
                        }
                        else
                        {
                            --i;
                            break;
                        }
                    }

                    expression.Add(number);
                }
            }
            return expression;
        }


    }
}
