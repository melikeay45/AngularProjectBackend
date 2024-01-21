using AngularProject.CORE.Helper;
using AngularProject.CORE.Result;
using AngularProject.CORE.UnitOfWork;
using AngularProject.DTO.Dtos;
using AngularProject.EF.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AngularProject.API.Services
{
    public class ShoppingCartApiService
    {
        // Önce Database Context dosyamın nesnesini oluşturuyorum.
        private static ShoppingProjectEntities shoppingProjectEntities = new ShoppingProjectEntities();

        // unitofwork nesnemi oluşturuyorum ve db context dosyamı parametre veriyorum.
        private EFUnitOfWork efUnitOfWork = new EFUnitOfWork(shoppingProjectEntities);
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
        //Yeni ürün ekler
        public Result AddShoppingCart(ShoppingCartDto shoppingCartDto)
        {
            var result = Result.Instance.Warning("HATA! Girdiğiniz bilgileri kontrol ediniz.");

            if (shoppingCartDto != null) //girilen bilgilerin kontrolü yapılır.
            {
                var mappedShoppingCart = shoppingCartDto.MapTo<ShoppingCartTBL>();
                efUnitOfWork.ShoppingCartTemplate.Add(mappedShoppingCart);
                efUnitOfWork.SaveChanges();

                result = Result.Instance.Success("Ürün sepetinize başarıyla eklendi.", mappedShoppingCart.cartID);

                return result;
            }
            return result;
        }

        //Verilen userID  değerine sahip sepetteki verileri veritabanında bulur ve döndürür.
        public string GetShoppingCartByUserID(int userID)
        {
            var shoppingCart = efUnitOfWork.ShoppingCartTemplate.GetAll()
           .Where(r => r.userID==userID)
           .ToList()
           .Select(r => r.MapTo<ShoppingCartDto>());

            return JsonConvert.SerializeObject(shoppingCart);
        }

    }
}