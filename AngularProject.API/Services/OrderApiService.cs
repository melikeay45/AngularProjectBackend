﻿using AngularProject.CORE.Helper;
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
    public class OrderApiService
    {
        // Önce Database Context dosyamın nesnesini oluşturuyorum.
        private static ShoppingProjectEntities shoppingProjectEntities = new ShoppingProjectEntities();

        // unitofwork nesnemi oluşturuyorum ve db context dosyamı parametre veriyorum.
        private EFUnitOfWork efUnitOfWork = new EFUnitOfWork(shoppingProjectEntities);

        //Veri tabanındaki kayıtlı tüm siparişleri sayfalayarak listeler.
        public string GetAllOrder()
        {
            //projecTo, autommaper aracı. DB varlığımı dto ya mapler.
            var orders = efUnitOfWork.OrderTemplate.GetAll(i => i.isDelete == false)
                .OrderBy(o => o.orderID) // gelen datayı id ye göre sırala
                .ProjectTo<OrderDto>()
                .ToList();
            return JsonConvert.SerializeObject(orders);
        }

        //kullanıcıya göre tarifleri listeler
        public string GetOrdersByUserID(int userID)
        {

            //projecTo, autommaper aracı. DB varlığımı dto ya mapler.
            var orders = efUnitOfWork.OrderTemplate.GetAll(i => i.userID == userID && i.isDelete == false)
                .OrderBy(o => o.orderID) // gelen datayı id ye göre sırala
                .ProjectTo<OrderDto>()
                .ToList();

            return JsonConvert.SerializeObject(orders);


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
        public string GetOrderByID(int id)
        {
            var order = efUnitOfWork.OrderTemplate.GetById(id).MapTo<OrderDto>();
            return JsonConvert.SerializeObject(order);
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
        public Result DeleteOrder(int id)
        {
            var result = Result.Instance.Warning("HATA! Böyle bir sipariş yok.");
            var order = efUnitOfWork.OrderTemplate.GetById(id);
            if (order != null)
            {
                efUnitOfWork.OrderTemplate.Delete(order);
                efUnitOfWork.SaveChanges();

                result = Result.Instance.Success("Sipariş başarıyla silindi.");
                return result;
            }

            return result;
        }


        //Yeni sipariş ekler
        public Result AddOrder(OrderDto orderDto)
        {
            var result = Result.Instance.Warning("HATA! Girdiğiniz bilgileri kontrol ediniz.");

            if (orderDto != null) //girilen bilgilerin kontrolü yapılır.
            {
                var mappedOrder = orderDto.MapTo<OrderTBL>();
                efUnitOfWork.OrderTemplate.Add(mappedOrder);
                efUnitOfWork.SaveChanges();

                result = Result.Instance.Success("Sipariş kaydınız başarıyla alındı.", mappedOrder.orderID);

                return result;
            }
            return result;
        }

        //Verilen id değerine sahip siparişi günceller.
        public Result UpdateOrder(int id, OrderDto orderDto)
        {
            var result = Result.Instance.Warning("HATA! Güncelleme istediğiniz tarif bulunamadı.");

            // istenilen id mevcutsa güncellenecek data order ye atanır.
            var order = efUnitOfWork.OrderTemplate.GetById(id).MapTo<OrderDto>();
            if (order != null)
            {
                var mapped = orderDto.MapTo<OrderTBL>();
                mapped.orderID = order.orderID;

                efUnitOfWork.OrderTemplate.Update(mapped);
                efUnitOfWork.SaveChanges();

                result = Result.Instance.Success("Sipariş başarıyla güncellendi.");
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