using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ExpenseTracker
{
    class Expense
    {
        public string Name { get; set; }
        public double Amount { get; set; }

        public Expense(string name, double amount)
        {
            Name = name;
            Amount = amount;
        }
    }



    ///
    class Program
    {
        static List<Expense> expenses = new List<Expense>();

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("Учет потраченных за день средств");
            Console.WriteLine("--------------------------------");

            int count = ReadOperationsCount();
            ReadExpenses(count);

            bool running = true;
            while (running)
            {
                ShowMenu();
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        PrintData();
                        break;
                    case "2":
                        ShowStatistics();
                        break;
                    case "3":
                        BubbleSortByPrice();
                        break;
                    case "4":
                        ConvertCurrency();
                        break;
                    case "5":
                        SearchByName();
                        break;
                    case "0":
                        running = false;
                        Console.WriteLine("Выход из программы.");
                        break;
                    default:
                        Console.WriteLine("Неверный пункт меню. Попробуйте снова.");
                        break;
                }
            }
        }
        static void SearchByName()
        {
            if (expenses.Count == 0)
            {
                Console.WriteLine("Список трат пуст.");
                return;
            }

            Console.Write("\nВведите название или часть названия для поиска: ");
            string input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Пустой запрос.");
                return;
            }

            string query = input.Trim().ToLower();
            bool found = false;

            for (int i = 0; i < expenses.Count; i++)
            {
                string nameLower = expenses[i].Name.ToLower();

                if (nameLower.Contains(query))
                {
                    if (!found)
                    {
                        Console.WriteLine("--- Результаты поиска ---");
                        found = true;
                    }
                    Console.WriteLine((i + 1) + ". " + expenses[i].Name
                                      + " — " + expenses[i].Amount.ToString("F2") + " руб.");
                }
            }

            if (!found)
            {
                Console.WriteLine("Ничего не найдено.");
            }
        }
        static int ReadOperationsCount()
        {
            int count;
            while (true)
            {
                Console.Write("Введите количество операций (от 2 до 40): ");
                string input = Console.ReadLine();
                if (int.TryParse(input, out count) && count >= 2 && count <= 40)
                {
                    return count;
                }
                Console.WriteLine("Некорректное значение. Введите целое число от 2 до 40.");
            }
        }

        static void ReadExpenses(int count)
        {
            Console.WriteLine("Введите траты в формате: Название услуги или товара; Количество денег");
            Console.WriteLine("Пример: Влажные салфетки \"Лента\"; 235");

            for (int i = 0; i < count; i++)
            {
                while (true)
                {
                    Console.Write("Трата " + (i + 1) + ": ");
                    string line = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(line))
                    {
                        Console.WriteLine("Строка не может быть пустой. Повторите ввод.");
                        continue;
                    }

                    line = line.Trim().TrimStart('(').TrimEnd(')');

                    int sepIndex = line.LastIndexOf(';');
                    if (sepIndex == -1)
                    {
                        Console.WriteLine("Неверный формат. Используйте разделитель ';' между названием и суммой.");
                        continue;
                    }

                    string name = line.Substring(0, sepIndex).Trim();
                    string amountStr = line.Substring(sepIndex + 1).Trim();

                    if (string.IsNullOrWhiteSpace(name))
                    {
                        Console.WriteLine("Название не может быть пустым.");
                        continue;
                    }

                    double amount;
                    bool parsed = double.TryParse(amountStr, NumberStyles.Any, CultureInfo.InvariantCulture, out amount);
                    if (!parsed)
                    {
                        parsed = double.TryParse(amountStr, NumberStyles.Any, CultureInfo.CurrentCulture, out amount);
                    }

                    if (!parsed)
                    {
                        Console.WriteLine("Сумма указана неверно. Введите число.");
                        continue;
                    }

                    if (amount < 0)
                    {
                        Console.WriteLine("Сумма не может быть отрицательной.");
                        continue;
                    }

                    expenses.Add(new Expense(name, amount));
                    break;
                }
            }

            Console.WriteLine("Все траты успешно внесены.");
            Console.WriteLine("");
        }
        static void ShowMenu()
        {
            Console.WriteLine("");
            Console.WriteLine("Меню:");
            Console.WriteLine("1. Вывод данных");
            Console.WriteLine("2. Статистика (среднее, максимальное, минимальное, сумма)");
            Console.WriteLine("3. Сортировка по цене (пузырьковая сортировка)");
            Console.WriteLine("4. Конвертация валюты");
            Console.WriteLine("5. Поиск по названию");
            Console.WriteLine("0. Выход");
            Console.Write("Выберите пункт меню: ");
        }
        static void PrintData()
        {
            Console.WriteLine("");
            Console.WriteLine("Список трат:");
            for (int i = 0; i < expenses.Count; i++)
            {
                Console.WriteLine((i + 1) + ". " + expenses[i].Name + " - " + expenses[i].Amount.ToString("F2") + " руб.");
            }
        }

        static void ShowStatistics()
        {
            if (expenses.Count == 0)
            {
                Console.WriteLine("Список трат пуст.");
                return;
            }

            double sum = expenses.Sum(e => e.Amount);
            double avg = sum / expenses.Count;
            double max = expenses.Max(e => e.Amount);
            double min = expenses.Min(e => e.Amount);

            Console.WriteLine("");
            Console.WriteLine("Статистика:");
            Console.WriteLine("Сумма: " + sum.ToString("F2") + " руб.");
            Console.WriteLine("Среднее: " + avg.ToString("F2") + " руб.");
            Console.WriteLine("Максимум: " + max.ToString("F2") + " руб.");
            Console.WriteLine("Минимум: " + min.ToString("F2") + " руб.");
        }
        static void BubbleSortByPrice()
        {
            if (expenses.Count == 0)
            {
                Console.WriteLine("Список трат пуст.");
                return;
            }

            Console.Write("Сортировать по возрастанию (1) или убыванию (2)? ");
            string dirChoice = Console.ReadLine();
            bool ascending = dirChoice != "2";

            int n = expenses.Count;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    bool needSwap;
                    if (ascending)
                    {
                        needSwap = expenses[j].Amount > expenses[j + 1].Amount;
                    }
                    else
                    {
                        needSwap = expenses[j].Amount < expenses[j + 1].Amount;
                    }

                    if (needSwap)
                    {
                        Expense temp = expenses[j];
                        expenses[j] = expenses[j + 1];
                        expenses[j + 1] = temp;
                    }
                }
            }

            Console.WriteLine("Список отсортирован.");
            PrintData();
        }

