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
        static string ReadNonEmptyString(string prompt)
        {
            string input;
            do
            {
                Console.Write(prompt);
                input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                    Console.WriteLine("Значение не может быть пустым. Повторите ввод.");
            } while (string.IsNullOrWhiteSpace(input));
            return input.Trim();
        }

        static decimal ReadNonNegativeDecimal(string prompt)
        {
            decimal value;
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                if (decimal.TryParse(input, out value) && value >= 0)
                    return value;
                Console.WriteLine("Некорректное значение. Введите число, большее или равное нулю.");
            }
        }

        static int ReadNonNegativeInt(string prompt)
        {
            int value;
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                if (int.TryParse(input, out value) && value >= 0)
                    return value;
                Console.WriteLine("Некорректное значение. Введите целое число, большее или равное нулю.");
            }
        }

        static Category ReadCategory()
        {
            var categories = Enum.GetValues(typeof(Category)).Cast<Category>().ToList();
            while (true)
            {
                Console.WriteLine("Выберите категорию:");
                for (int i = 0; i < categories.Count; i++)
                    Console.WriteLine($"{i + 1}. {categories[i]}");
                Console.Write("Номер категории: ");
                string input = Console.ReadLine();
                if (int.TryParse(input, out int index) && index >= 1 && index <= categories.Count)
                    return categories[index - 1];
                Console.WriteLine("Некорректный номер категории. Повторите ввод.");
            }
        }
        static void AddProduct()
        {
            Console.WriteLine("--- Добавление товара ---");
            string name = ReadNonEmptyString("Название товара: ");
            decimal price = ReadNonNegativeDecimal("Цена товара: ");
            int quantity = ReadNonNegativeInt("Количество товара: ");
            Category category = ReadCategory();

            var product = new Product
            {
                Code = nextCode++,
                Name = name,
                Price = price,
                Quantity = quantity,
                Category = category
            };
            products.Add(product);
            Console.WriteLine($"Товар добавлен с кодом {product.Code}.");
        }

        static void DeleteProduct()
        {
            Console.WriteLine("--- Удаление товара ---");
            int code = ReadNonNegativeInt("Введите код товара для удаления: ");
            var product = products.FirstOrDefault(p => p.Code == code);
            if (product == null)
            {
                Console.WriteLine("Товар с таким кодом не найден.");
                return;
            }
            products.Remove(product);
            Console.WriteLine($"Товар \"{product.Name}\" удалён.");
        }

        static void OrderSupply()
        {
            Console.WriteLine("--- Заказ поставки товара ---");
            int code = ReadNonNegativeInt("Введите код товара: ");
            var product = products.FirstOrDefault(p => p.Code == code);
            if (product == null)
            {
                Console.WriteLine("Товар с таким кодом не найден.");
                return;
            }
            int amount = ReadNonNegativeInt("Количество для поставки: ");
            product.Quantity += amount;
            Console.WriteLine($"Поставка выполнена. Новое количество \"{product.Name}\": {product.Quantity}.");
        }
        static void SellProduct()
        {
            Console.WriteLine("--- Продажа товара ---");
            int code = ReadNonNegativeInt("Введите код товара: ");
            var product = products.FirstOrDefault(p => p.Code == code);
            if (product == null)
            {
                Console.WriteLine("Товар с таким кодом не найден.");
                return;
            }
            if (!product.InStock)
            {
                Console.WriteLine($"Товара \"{product.Name}\" нет на складе.");
                return;
            }
            int amount = ReadNonNegativeInt("Количество для продажи: ");
            if (amount == 0)
            {
                Console.WriteLine("Количество продажи должно быть больше нуля.");
                return;
            }
            if (amount > product.Quantity)
            {
                Console.WriteLine($"Недостаточно товара на складе. Доступно: {product.Quantity}.");
                return;
            }
            product.Quantity -= amount;
            decimal total = product.Price * amount;
            var sale = new Sale { Product = product, Quantity = amount, TotalPrice = total, Date = DateTime.Now };
            salesHistory.Push(sale);
            Console.WriteLine($"Продано {amount} шт. \"{product.Name}\" на сумму {total:0.00}.");
        }
        static void SearchProducts()
        {
            Console.WriteLine("--- Поиск товара ---");
            Console.WriteLine("1. По коду");
            Console.WriteLine("2. По названию");
            Console.WriteLine("3. По категории");
            Console.Write("Выберите способ поиска: ");
            string choice = Console.ReadLine();

            List<Product> results = new List<Product>();

            switch (choice)
            {
                case "1":
                    int code = ReadNonNegativeInt("Введите код товара: ");
                    results = products.Where(p => p.Code == code).ToList();
                    break;
                case "2":
                    string name = ReadNonEmptyString("Введите название (или часть названия): ");
                    results = products.Where(p => p.Name.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                    break;
                case "3":
                    Category category = ReadCategory();
                    results = products.Where(p => p.Category == category).ToList();
                    break;
                default:
                    Console.WriteLine("Некорректный выбор способа поиска.");
                    return;
            }

            if (results.Count == 0)
            {
                Console.WriteLine("Товары не найдены.");
            }
            else
            {
                Console.WriteLine("Найденные товары:");
                foreach (var p in results)
                    Console.WriteLine(p);
            }
        }

        static void ShowAllProducts()
        {
            Console.WriteLine("--- Список всех товаров ---");
            if (products.Count == 0)
            {
                Console.WriteLine("Список товаров пуст.");
                return;
            }
            foreach (var p in products.OrderBy(p => p.Code))
                Console.WriteLine(p);
        }