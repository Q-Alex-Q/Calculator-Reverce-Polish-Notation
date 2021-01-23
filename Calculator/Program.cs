using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;

namespace Calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Вы запустили калькулят\n" +
            "Если вы хотите посчитать выражения в файле введите F.\n" +
            "Посчитать выражение - введите любой другой символ.");

            if (Console.ReadLine().ToLower() == "f") // Посчитать результат выражений из файла.
            {
                Console.WriteLine("Вы выбрали посчитать выражения в файле");

                string filePath = "";
                while (!File.Exists(filePath))
                {
                    filePath = GetFilePath("Введите путь к файлу с выражениями");
                }

                string destinationPath = GetFilePath("Введите путь к файлу назначения.");

                if (!File.Exists(destinationPath))
                {
                    destinationPath = Path.GetPathRoot(filePath) + Path.GetFileNameWithoutExtension(filePath)
                                      + "Result" + Path.GetExtension(filePath);
                }

                Calculator calc = new Calculator();
                calc.CalculateFile(filePath, destinationPath);

            }
            else // Посчитать результат одного выражения.
            {
                while (true)
                {
                    try
                    {
                        Console.WriteLine("Вы выбрали подсчитать выражение\n" +
                                      "Введите выражение.");

                        string userExpression = Console.ReadLine();

                        Calculator calc = new Calculator();

                        Console.WriteLine("{0} = {1}", userExpression, calc.GetExpressionResult(userExpression));

                    }
                    catch (Exception e)
                    {
                        if (e.Message == "Стек пуст.")
                        {
                            Console.WriteLine("Входная строка имела неверный формат");
                        }
                        else
                        {
                            Console.WriteLine(e.Message);
                        }

                    }

                    Console.WriteLine("Если хотите посчитать ещё одно выражение нажмите Y");

                    if (Console.ReadLine().ToLower() != "y")
                    {
                        break;
                    }
                }
                
                
            }

            string GetFilePath(string message)
            {
                Console.WriteLine(message);
                return Console.ReadLine();
            }

        }
    }
}
//"Подлый" способ решить задачу.
//Console.WriteLine("{0} = {1}", userInput, new DataTable().Compute(userInput, ""));

// Про польскую нотацию нормальными словами - https://habr.com/ru/post/282379/
// И немного видео - https://www.youtube.com/watch?v=LQ-iW8jm6Mk