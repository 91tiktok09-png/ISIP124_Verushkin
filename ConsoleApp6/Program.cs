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
