using AngularProject.API.Models;
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
using System.Web.Http;
using Result = AngularProject.CORE.Result.Result;

namespace AngularProject.API.Services
{
    public class ShoppingCartApiService
    {
        // Önce Database Context dosyamın nesnesini oluşturuyorum.
        private static ShoppingProjectEntities shoppingProjectEntities = new ShoppingProjectEntities();

        // unitofwork nesnemi oluşturuyorum ve db context dosyamı parametre veriyorum.
        private EFUnitOfWork efUnitOfWork = new EFUnitOfWork(shoppingProjectEntities);

        //Verilen id değerine sahip kategoriyi veritabanında bulur ve döndürür.
        public string GetShoppingCartById(int id)
        {
            var shoppingCarts = efUnitOfWork.ShoppingCartTemplate.GetById(id).MapTo<ShoppingCartDto>();
            return JsonConvert.SerializeObject(shoppingCarts);
        }

        public Result DeleteShoppingCart(int id)
        {
            var result = Result.Instance.Warning("HATA! Sepetinizde böyle bir ürün yok.");
            var shoppingCart = efUnitOfWork.ShoppingCartTemplate.GetById(id);
            if (shoppingCart != null)
            {
                efUnitOfWork.ShoppingCartTemplate.Delete(shoppingCart);
                efUnitOfWork.SaveChanges();

                result = Result.Instance.Success("Ürün sepetten silindi.");
                return result;
            }

            return result;
        }


        public Result AddOrUpdateShoppingCart(ShoppingCartDto shoppingCartDto)
        {
            var result = Result.Instance.Warning("HATA! Girdiğiniz bilgileri kontrol ediniz.");

            if (shoppingCartDto != null)
            {
                // Kullanıcıya ait belirli bir ürünün sepete eklenip eklenmediğini kontrol et
                var existingCart = efUnitOfWork.ShoppingCartTemplate.GetAll()
                                    .Where(x => x.userID == shoppingCartDto.userID && x.productID == shoppingCartDto.productID)
                                    .FirstOrDefault();

                if (existingCart != null)
                {
                    // Eğer varsa, miktarını artır
                    existingCart.quantity++;
                    efUnitOfWork.SaveChanges();

                    result = Result.Instance.Success("Ürün sepetinize başarıyla eklendi.", existingCart.cartID);
                }
                else
                {
                    // Yoksa, yeni bir kayıt oluştur
                    var mappedShoppingCart = shoppingCartDto.MapTo<ShoppingCartTBL>();
                    mappedShoppingCart.quantity = 1; // Yeni ürünün miktarını 1 olarak ayarlayın
                    efUnitOfWork.ShoppingCartTemplate.Add(mappedShoppingCart);
                    efUnitOfWork.SaveChanges();

                    result = Result.Instance.Success("Ürün sepetinize başarıyla eklendi.", mappedShoppingCart.cartID);
                }
            }
            return result;
        }

        public Result DeleteAll(int userID)
        {
            try
            {
                var shoppingCarts = efUnitOfWork.ShoppingCartTemplate.GetAll()
           .Where(r => r.userID == userID)
           .OrderBy(r => r.cartID)
           .ToList()
           .Select(r => r.MapTo<ShoppingCartDto>());
                //var _shoppingCarts=JsonConvert.SerializeObject(shoppingCarts);
                foreach (var shoppingCart in shoppingCarts)
                {
                    efUnitOfWork.ShoppingCartTemplate.Delete(shoppingCart.cartID);
                }

                Result.Instance.Success("Başarılı");
            }
            catch (Exception ex) { }
            return Result.Instance.Success("Başarısız");
        }


        //Verilen userID  değerine sahip sepetteki verileri veritabanında bulur ve döndürür.
        public string GetShoppingCartByUserID(int userID)
        {
            var shoppingCart = efUnitOfWork.ShoppingCartTemplate.GetAll()
           .Where(r => r.userID == userID)
           //.OrderBy(r=>r.cartID)
           .ProjectTo<ShoppingCartDto>()
           .ToList();

           //.Select(r => r.MapTo<ShoppingCartDto>());

            return JsonConvert.SerializeObject(shoppingCart);
        }

        //Sadece miktar alanını günceller
        public Result UpdateShoppingCartQuantity(int id, ShoppingCartDto shoppingCartDto)
        {
            var result = Result.Instance.Warning("HATA! Güncelleme istediğiniz ürün bulunamadı.");

            // istenilen id mevcutsa güncellenecek data shoppingcarta atanır.
            var shoppingCart = efUnitOfWork.ShoppingCartTemplate.GetById(id).MapTo<ShoppingCartDto>();
            if (shoppingCart != null)
            {

                var mapped = shoppingCartDto.MapTo<ShoppingCartTBL>();
                mapped.cartID = shoppingCart.cartID;
                efUnitOfWork.ShoppingCartTemplate.Update(mapped);
                efUnitOfWork.SaveChanges();

                result = Result.Instance.Success("Sepet bilgisi başarıyla güncellendi.");
                return result;
            }
            return result;
        }
    }
}