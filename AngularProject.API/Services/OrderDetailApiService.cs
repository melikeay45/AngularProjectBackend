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
    public class OrderDetailApiService
    {
        // Önce Database Context dosyamın nesnesini oluşturuyorum.
        private static ShoppingProjectEntities shoppingProjectEntities = new ShoppingProjectEntities();

        // unitofwork nesnemi oluşturuyorum ve db context dosyamı parametre veriyorum.
        private EFUnitOfWork efUnitOfWork = new EFUnitOfWork(shoppingProjectEntities);

        //Veri tabanındaki kayıtlı tüm sipariş detaylarını listeler sayfalayarak listeler.
        public string GetAllOrderDetail()
        {
           
            //projecTo, autommaper aracı. DB varlığımı dto ya mapler.
            var orderDetails = efUnitOfWork.OrderDetailTemplate.GetAll(i => i.isDelete == false)
                .OrderBy(o => o.detailID).
                ProjectTo<OrderDetailDto>()
                .ToList();

            return JsonConvert.SerializeObject(orderDetails);
        }

        //Siparişe göre sipariş detayını listeler
        public string GetOrderDetailByOrderID(int orderID)
        {

            //projecTo, autommaper aracı. DB varlığımı dto ya mapler.
            var orderDetails = efUnitOfWork.OrderDetailTemplate.GetAll(i => i.orderID == orderID && i.isDelete == false)
                .OrderBy(o => o.detailID) // gelen datayı id ye göre sırala
                .ProjectTo<OrderDetailDto>()
                .ToList();

            return JsonConvert.SerializeObject(orderDetails);


        }


        ////Kullanıcı Idsine göre sipariş detayı listeler
        //public string GetOrderDetailByUserID(int userId)
        //{

        //    //projecTo, autommaper aracı. DB varlığımı dto ya mapler.
        //    var orderDetails = efUnitOfWork.OrderDetailTemplate.GetAll(i => i.userID == userId && i.isDelete == false)
        //             .OrderBy(o => o.detailID) // gelen datayı id ye göre sırala
        //            .ProjectTo<OrderDetailDto>()
        //            .ToList();
        //    return JsonConvert.SerializeObject(orderDetails);

        //}

        //Verilen id değerine sahip order detaili veritabanında bulur ve döndürür.
        public string GetOrderDetailByID(int id)
        {
            var orderDetail = efUnitOfWork.OrderDetailTemplate.GetById(id).MapTo<OrderDetailDto>();
            return JsonConvert.SerializeObject(orderDetail);
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

        //Verilen id değerine sahip tarifi veritabanından siler.
        public Result DeleteOrderDetail(int id)
        {
            var result = Result.Instance.Warning("HATA! Böyle bir tarif kaydı yok.");
            var orderDetail = efUnitOfWork.OrderDetailTemplate.GetById(id);
            if (orderDetail != null)
            {
                efUnitOfWork.OrderDetailTemplate.Delete(orderDetail);
                efUnitOfWork.SaveChanges();

                result = Result.Instance.Success("Tarif başarıyla silindi.");
                return result;
            }

            return result;
        }


        //Yeni tarif ekler.
        public Result AddOrderDetail(OrderDetailDto orderDetailDto)
        {
            var result = Result.Instance.Warning("HATA! Girdiğiniz bilgileri kontrol ediniz.");

            if (orderDetailDto != null) //girilen bilgilerin kontrolü yapılır.
            {
                var mappedOrderDetail = orderDetailDto.MapTo<OrderDetailTBL>();
                efUnitOfWork.OrderDetailTemplate.Add(mappedOrderDetail);
                efUnitOfWork.SaveChanges();

                result = Result.Instance.Success("Sipariş detayı kaydınız başarıyla alındı.", mappedOrderDetail.detailID);

                return result;
            }
            return result;
        }

        ////Verilen id değerine sahip tarifi günceller.
        //public Result UpdateRecipe(int id, RecipeDto recipeDto)
        //{
        //    var result = Result.Instance.Warning("HATA! Güncelleme istediğiniz tarif bulunamadı.");

        //    // istenilen id mevcutsa güncellenecek data recipe ye atanır.
        //    var recipe = efUnitOfWork.RecipeTemplate.GetById(id).MapTo<RecipeDto>();
        //    if (recipe != null)
        //    {
        //        var mapped = recipeDto.MapTo<RecipeTBL>();
        //        mapped.recipeId = recipe.recipeId;

        //        efUnitOfWork.RecipeTemplate.Update(mapped);
        //        efUnitOfWork.SaveChanges();

        //        result = Result.Instance.Success("Tarif başarıyla güncellendi.");
        //        return result;
        //    }
        //    return result;
        //}

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

        ////Verilen tarif Id sine göre fotoğraf ekler.
        //public Result UploadRecipePicture(int id, string filePath)
        //{
        //    var result = Result.Instance.Warning("HATA! Güncellemek istediğiniz kayıt bulunamadı.");

        //    // gelen id yi sorgula db de sorgula. 
        //    var recipe = efUnitOfWork.RecipeTemplate.GetById(id).MapTo<RecipeDto>();

        //    if (recipe != null)
        //    {
        //        //gönderilen dosyanın yolunu gönder.
        //        recipe.recipeImageUrl = filePath;

        //        //veritabanı nesnesi ile maple.
        //        var mappedRecipe = recipe.MapTo<RecipeTBL>();

        //        // yapılan değişikliği güncelle
        //        efUnitOfWork.RecipeTemplate.Update(mappedRecipe);

        //        // değişiklikleri kaydet ve veritabanına yansıt
        //        efUnitOfWork.SaveChanges();

        //        result = Result.Instance.Success("Fotoğraf başarıyla eklendi.");
        //        return result;
        //    }
        //    return result;

        //}

    }
}