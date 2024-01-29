using AngularProject.API.Services;
using AngularProject.CORE.Result;
using AngularProject.DTO.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;

namespace AngularProject.API.Controllers
{
    public class OrderApiController:ApiController
    {

        private OrderApiService _orderApiService = new OrderApiService();

        [Route("api/OrderApi/GetAll")]
        [HttpGet]
        public string GetAll()
        {
            return _orderApiService.GetAllOrder();
        }

        [Route("api/OrderApi/GetOrdersByUserID")]
        [HttpGet]
        public string GetOrdersByUserID()
        {
            return _orderApiService.GetOrdersByUserID(Convert.ToInt32(User.Identity.Name));
        }


        [Route("api/OrderApi/Get")]
        [HttpGet]
        public string Get(int id)
        {
            return _orderApiService.GetOrderByID(id);
        }


        //[Route("api/OrderApi/GetRecipeByRecipeName")]
        //[HttpGet]
        //public string GetRecipeByRecipeName(string recipeName)
        //{
        //    return recipeApiService.GetRecipeByRecipeName(recipeName);
        //}


        //[Authorize]
        [Route("api/OrderApi/Add")]
        [HttpPost]
        public Result Post(OrderDto[] orderDto)
        {

                
                return _orderApiService.AddOrder(orderDto,Convert.ToInt32(User.Identity.Name));
        }

        [Authorize]
        [Route("api/OrderApi/Delete")]
        [HttpDelete]
        public Result Delete(int id)
        {

            return _orderApiService.DeleteOrder(id);
        }

        [Authorize]
        [Route("api/OrderApi/Update")]
        [HttpPut]
        public Result Update(int id, OrderDto orderDto)
        {

            return _orderApiService.UpdateOrder(id, orderDto);
        }


        public static string DecodeEncodedFileName(string encodedFileName)
        {
            var regex = new Regex(@"=\?utf-8\?B\?(.*?)\?=");
            var match = regex.Match(encodedFileName);

            if (match.Success)
            {
                var base64String = match.Groups[1].Value;
                var bytes = Convert.FromBase64String(base64String);
                return System.Text.Encoding.UTF8.GetString(bytes);
            }
            else
            {
                return encodedFileName;
            }
        }

    }
}