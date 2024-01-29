using AngularProject.CORE.Helper;
using AngularProject.CORE.Result;
using AngularProject.CORE.UnitOfWork;
using AngularProject.DTO.Dtos;
using AngularProject.EF.Models;
using AutoMapper.QueryableExtensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AngularProject.API.Services
{
    public class ProductApiService
    {
        // Önce Database Context dosyamın nesnesini oluşturuyorum.
        private static ShoppingProjectEntities shoppingProjectEntities = new ShoppingProjectEntities();

        // unitofwork nesnemi oluşturuyorum ve db context dosyamı parametre veriyorum.
        private EFUnitOfWork efUnitOfWork = new EFUnitOfWork(shoppingProjectEntities);

        //Veri tabanındaki kayıtlı tüm ürünleri listeler.
        public string GetAllProduct()
        {
            //projecTo, autommaper aracı. DB varlığımı dto ya mapler.
            var products = efUnitOfWork.ProductTemplate.GetAll(i => i.isDelete == false)
                .OrderBy(o => o.productID) // gelen datayı id ye göre sırala
                .ProjectTo<ProductDto>()
                .ToList();
            return JsonConvert.SerializeObject(products);
        }

        //kategoriye göre ürünleri listeler
        public string GetProducsBycategoryID(int categoryID)
        {

            //projecTo, autommaper aracı. DB varlığımı dto ya mapler.
            var producs = efUnitOfWork.ProductTemplate.GetAll(i => i.categoryID == categoryID && i.isDelete == false)
                .OrderBy(o => o.productID) // gelen datayı id ye göre sırala
                .ProjectTo<ProductDto>()
                .ToList();

            return JsonConvert.SerializeObject(producs);


        }

      


        ////Kullanıcı Idsine göre tarifleri listeler
        //public string GetRecipeByUserId(int userId, int page, int pageSize)
        //{

        //    //projecTo, autommaper aracı. DB varlığımı dto ya mapler.
        //    var recipes = efUnitOfWork.RecipeTemplate.GetAll(i => i.userId == userId && i.isDelete == false)
        //             .OrderBy(o => o.recipeId) // gelen datayı id ye göre sırala
        //            .ProjectTo<RecipeDto>()
        //            .ToList()
        //            .ToPaginate(page, pageSize);

        //    RecipeListModel mappedRecipeListModel = recipes.MapTo<RecipeListModel>();
        //    return JsonConvert.SerializeObject(mappedRecipeListModel);

        //}

        //Verilen id değerine sahip siparişi veritabanında bulur ve döndürür.
        public string GetProductByID(int id)
        {
            var product = efUnitOfWork.ProductTemplate.GetById(id).MapTo<ProductDto>();
            return JsonConvert.SerializeObject(product);
        }



        ////Verilen recipeName  değerine sahip tarifi veritabanında bulur ve döndürür.
        //public string GetRecipeByRecipeName(string recipeName)
        //{
        //    var recipes = efUnitOfWork.RecipeTemplate.GetAll()
        //   .Where(r => r.recipeName.ToLower().StartsWith(recipeName.ToLower()))
        //   .ToList()
        //   .Select(r => r.MapTo<RecipeDto>());

        //    return JsonConvert.SerializeObject(recipes);
        //}

        //Verilen id değerine sahip ürünü veritabanından siler.
        public Result DeleteProduct(int id)
        {
            var result = Result.Instance.Warning("HATA! Böyle bir ürün yok.");
            var product = efUnitOfWork.ProductTemplate.GetById(id);
            if (product != null)
            {
                efUnitOfWork.ProductTemplate.Delete(product);
                efUnitOfWork.SaveChanges();

                result = Result.Instance.Success("Ürün başarıyla silindi.");
                return result;
            }

            return result;
        }


        //Yeni ürün ekler
        public Result AddProduct(ProductDto productDto)
        {
            var result = Result.Instance.Warning("HATA! Girdiğiniz bilgileri kontrol ediniz.");

            if (productDto != null) //girilen bilgilerin kontrolü yapılır.
            {
                var mappedProduct = productDto.MapTo<ProductTBL>();
                efUnitOfWork.ProductTemplate.Add(mappedProduct);
                efUnitOfWork.SaveChanges();

                result = Result.Instance.Success("Sipariş kaydınız başarıyla alındı.", mappedProduct.productID);

                return result;
            }
            return result;
        }

        //Verilen id değerine sahip ürünü günceller.
        public Result UpdateProduct(int id, ProductDto productDto)
        {
            var result = Result.Instance.Warning("HATA! Güncelleme istediğiniz ürün bulunamadı.");

            // istenilen id mevcutsa güncellenecek data product ye atanır.
            var product = efUnitOfWork.ProductTemplate.GetById(id).MapTo<ProductDto>();
            if (product != null)
            {
                var mapped = productDto.MapTo<ProductTBL>();
                mapped.productID = product.productID;

                efUnitOfWork.ProductTemplate.Update(mapped);
                efUnitOfWork.SaveChanges();

                result = Result.Instance.Success("Ürün başarıyla güncellendi.");
                return result;
            }
            return result;
        }

        ////Tarif Idsine göre göre video yükler.
        //public Result UploadRecipeVideo(int id, string filePath)
        //{
        //    var result = Result.Instance.Warning("HATA! Güncellemek istediğiniz kayıt bulunamadı.");

        //    // gelen id yi sorgula db de sorgula. 
        //    var recipe = efUnitOfWork.RecipeTemplate.GetById(id).MapTo<RecipeDto>();

        //    if (recipe != null)
        //    {
        //        //gönderilen dosyanın yolunu gönder.
        //        recipe.recipeVideoUrl = filePath;

        //        //veritabanı nesnesi ile maple.
        //        var mappedRecipe = recipe.MapTo<RecipeTBL>();

        //        // yapılan değişikliği güncelle
        //        efUnitOfWork.RecipeTemplate.Update(mappedRecipe);

        //        // değişiklikleri kaydet ve veritabanına yansıt
        //        efUnitOfWork.SaveChanges();

        //        result = Result.Instance.Success("Video başarı ile yüklendi.");
        //        return result;
        //    }
        //    return result;

        //}

        //verilen ürün ıd sine göre fotoğraf ekler.
        public Result UploadProductPicture(int id, string filePath)
        {
            var result = Result.Instance.Warning("HATA! Güncellemek istediğiniz kayıt bulunamadı.");

            // gelen id yi sorgula db de sorgula. 
            var product = efUnitOfWork.ProductTemplate.GetById(id).MapTo<ProductDto>();

            if (product != null)
            {
                //gönderilen dosyanın yolunu gönder.
                product.imageURL = filePath;

                //veritabanı nesnesi ile maple.
                var mappedProduct = product.MapTo<ProductTBL>();

                // yapılan değişikliği güncelle
                efUnitOfWork.ProductTemplate.Update(mappedProduct);

                // değişiklikleri kaydet ve veritabanına yansıt
                efUnitOfWork.SaveChanges();

                result = Result.Instance.Success("Fotoğraf başarıyla eklendi.");
                return result;
            }
            return result;

        }

    }
}