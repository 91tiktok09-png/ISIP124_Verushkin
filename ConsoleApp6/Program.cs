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