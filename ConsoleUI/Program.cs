using Business.Abstract;
using Business.Concrete;
using DataAccess.Concrete.InMemory;
using Entities.Concrete;
using System;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            ProductManager productManager =
                new ProductManager(new InMemoryProductDal());

            foreach (Product product in productManager.GetAll())
            {
                Console.WriteLine("{0} {1}", product.ProductId, product.ProductName);
            }
        }
    }
}
