using System;
using System.Collections.Generic;
using System.Linq;

namespace StoreInventory
{
    enum Category
    {
        Продукты,
        Электроника,
        Одежда,
        БытоваяХимия
    }

    class Product
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public bool InStock => Quantity > 0;
        public Category Category { get; set; }

        public override string ToString()
        {
            return $"Код: {Code} | Название: {Name} | Цена: {Price:0.00} | Количество: {Quantity} | " +
                $"В наличии: {(InStock ? "Да" : "Нет")} | Категория: {Category}";
        }
    }

    class Sale
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime Date { get; set; }

        public override string ToString()
        {
            return $"{Date:dd.MM.yyyy HH:mm} | {Product.Name} | Кол-во: {Quantity} | Сумма: {TotalPrice:0.00}";
        }
    }
    class Program
    {
        static List<Product> products = new List<Product>();
        static Stack<Sale> salesHistory = new Stack<Sale>();
        static int nextCode = 1;

        static void Main()
        {
            SeedTestData();

            bool running = true;
            while (running)
            {
                ShowMenu();
                string choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            AddProduct();
                            break;
                        case "2":
                            DeleteProduct();
                            break;
                        case "3":
                            OrderSupply();
                            break;
                        case "4":
                            SellProduct();
                            break;
                        case "5":
                            SearchProducts();
                            break;
                        case "6":
                            ShowAllProducts();
                            break;
                        case "7":
                            UndoLastSale();
                            break;
                        case "8":
                            ShowSalesReport();
                            break;
                        case "0":
                            running = false;
                            break;
                        default:
                            Console.WriteLine("Неизвестная команда. Попробуйте снова.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Произошла ошибка: {ex.Message}. Программа продолжает работу.");
                }

                if (running)
                {
                    Console.WriteLine("\nНажмите Enter для продолжения...");
                    Console.ReadLine();
                }
            }

            Console.WriteLine("Работа программы завершена.");
        }

        static void ShowMenu()
        {
            Console.Clear();
            Console.WriteLine("===== УЧЁТ ТОВАРОВ В МАГАЗИНЕ =====");
            Console.WriteLine("1. Добавить товар");
            Console.WriteLine("2. Удалить товар");
            Console.WriteLine("3. Заказать поставку товара");
            Console.WriteLine("4. Продать товар");
            Console.WriteLine("5. Поиск товара (по коду, названию, категории)");
            Console.WriteLine("6. Показать все товары");
            Console.WriteLine("7. Отменить последнюю продажу");
            Console.WriteLine("8. Отчёт о продажах");
            Console.WriteLine("0. Выход");
            Console.Write("Выберите пункт меню: ");
        }

        static void SeedTestData()
        {
            products.Add(new Product { Code = nextCode++, Name = "Хлеб белый", Price = 45.50m, Quantity = 30, Category = Category.Продукты });
            products.Add(new Product { Code = nextCode++, Name = "Наушники беспроводные", Price = 1999.00m, Quantity = 12, Category = Category.Электроника });
            products.Add(new Product { Code = nextCode++, Name = "Футболка мужская", Price = 899.00m, Quantity = 20, Category = Category.Одежда });
            products.Add(new Product { Code = nextCode++, Name = "Стиральный порошок", Price = 350.00m, Quantity = 0, Category = Category.БытоваяХимия });
            products.Add(new Product { Code = nextCode++, Name = "Молоко 1л", Price = 89.90m, Quantity = 15, Category = Category.Продукты });
        }