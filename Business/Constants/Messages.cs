using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Constants
{
    public static class Messages
    {
        public static string ProductAdded = "Ürün eklendi.";
        public static string ProductNameInvalid = "Ürün ismi geçersiz.";

        public static string MaintenanceTime = "Bakım Arası";

        public static string ProductsListed = "Ürünler listelendi";

        public static string ProductsListedByCategoryId = "Ürünler kategori id'ye göre listelendi.";

        public static string ProductNameAlreadyExists = "Aynı isimde bir ürün zaten var.";
        public static string ProductCountByCategoryError = "Aynı kategoride ürün sayısı 10'u geçemez.";
        public static string CategoryCountLimitExceeded = "Kategori sayısı limiti aşıldığı için yeni ürün eklenemez.";
    }
}
