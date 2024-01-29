using AngularProject.API.Services;
using AngularProject.CORE.Result;
using AngularProject.DTO.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace AngularProject.API.Controllers
{
    public class ShoppingCartApiController:ApiController
    {
        private ShoppingCartApiService _shoppingCartApiService = new ShoppingCartApiService();

        [Authorize]
        [Route("api/CartApi/GetbyUserID")]
        [HttpGet]
        public string Get()
        {
            return _shoppingCartApiService.GetShoppingCartByUserID(Convert.ToInt32(User.Identity.Name));
        }
        
        [Route("api/CartApi/Get")]
        [HttpGet]
        public string GetbyId(int id)
        {
            return _shoppingCartApiService.GetShoppingCartById(id);
        }

        [Authorize]
        [Route("api/CartApi/Add")]
        [HttpPost]
        public Result Post(ShoppingCartDto shoppingCartDto)
        {
            shoppingCartDto.userID = Convert.ToInt32(User.Identity.Name);
            return _shoppingCartApiService.AddOrUpdateShoppingCart(shoppingCartDto);
        }

        [Authorize]
        [Route("api/CartApi/Delete")]
        [HttpDelete]
        public Result Delete(int id)
        {
            return _shoppingCartApiService.DeleteShoppingCart(id);
        }

        [Authorize]
        [Route("api/CartApi/DeleteAll")]
        [HttpDelete]
        public Result DeleteAll()
        {
            return _shoppingCartApiService.DeleteAll(Convert.ToInt32(User.Identity.Name));
        }

        [Authorize]
        [Route("api/CartApi/Update")]
        [HttpPut]
        public Result Update(int id, ShoppingCartDto shoppingCartDto)
        {
            shoppingCartDto.userID = Convert.ToInt32(User.Identity.Name);
            return _shoppingCartApiService.UpdateShoppingCartQuantity(id, shoppingCartDto);
        }
    }
}